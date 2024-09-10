using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Models.EF;

namespace WebBanHang.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext dbConect = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null&&cart.items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        public ActionResult Partial_Item_Thanhtoan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.items);
            }
            return PartialView();
        }
        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return PartialView(cart.items);
            }
            return PartialView();
        }
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { count = cart.items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel order)
        {
            var code = new { Success = false, code = -1 };
            
            if (ModelState.IsValid)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    Order od = new Order();
                    od.CustomerName = order.CustomerName;
                    od.Phone = order.Phone;
                    od.Address = order.Address;
                    od.Email = order.Email;
                    cart.items.ForEach(x =>
                    {
                        od.OrderDetails.Add(new OrderDetail
                        {
                            ProductId = x.ProductId,
                            Quantity = x.Quantity,
                            Price = x.Price
                        });
                        cart.RemoveQuantity(x.ProductId, x.Quantity);
                    });
                    od.TotalAmount = cart.items.Sum(x => (x.Price * x.Quantity));
                    od.TypePayment = order.TypePayment;
                    od.CreatedDate = DateTime.Now;
                    od.ModifiedDate = DateTime.Now;
                    od.CreatedBy = order.CustomerName;
                    
                    Random rd = new Random();
                    od.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    dbConect.Orders.Add(od);
                    dbConect.SaveChanges();
                    //send mail 
                    var strSanPham = "";
                    var thanhtien = decimal.Zero;
                    var tongtien = decimal.Zero;
                    foreach(var sp in cart.items)
                    {
                        strSanPham += "<tr>";
                        strSanPham += "<td>"+sp.ProductName+"</td>";
                        strSanPham += "<td>" + sp.Quantity + "</td>";
                        strSanPham += "<td>" + WebBanHang.Common.Common.FormatNumber(sp.Price,0) + "</td>";
                        //strSanPham += "<td>" + WebBanHang.Common.Common.FormatNumber(sp.Total, 0) + "</td>";
                        strSanPham += "</tr>";
                        thanhtien += sp.Price * sp.Quantity;
                    }
                    tongtien = thanhtien;
                    var ngaydat = od.CreatedDate.ToString();
                    string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    contentCustomer = contentCustomer.Replace("{{MaDon}}", od.Code);
                    contentCustomer = contentCustomer.Replace("{{NgayDat}}", ngaydat);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", od.CustomerName);
                    contentCustomer = contentCustomer.Replace("{{DienThoai}}", od.Phone);
                    contentCustomer = contentCustomer.Replace("{{Email}}", od.Email);
                    contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", od.Address);
                    contentCustomer = contentCustomer.Replace("{{ThanhTien}}", WebBanHang.Common.Common.FormatNumber(thanhtien, 0));
                    contentCustomer = contentCustomer.Replace("{{TongTien}}", WebBanHang.Common.Common.FormatNumber(tongtien, 0));
                    WebBanHang.Common.Common.SendMail("ANSHOP", "Don hang  #" + od.Code, contentCustomer.ToString(), od.Email);
                    //gui email cho admin
                    string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    contentAdmin = contentAdmin.Replace("{{MaDon}}", od.Code);
                    contentAdmin = contentAdmin.Replace("{{NgayDat}}", ngaydat);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", od.CustomerName);
                    contentAdmin = contentAdmin.Replace("{{DienThoai}}", od.Phone);
                    contentAdmin = contentAdmin.Replace("{{Email}}", od.Email);
                    contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", od.Address);
                    contentAdmin = contentAdmin.Replace("{{ThanhTien}}", WebBanHang.Common.Common.FormatNumber(thanhtien, 0));
                    contentAdmin = contentAdmin.Replace("{{TongTien}}", WebBanHang.Common.Common.FormatNumber(tongtien, 0));
                    WebBanHang.Common.Common.SendMail("TOADMIN", "Don hang moi #" + od.Code, contentCustomer.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                    cart.ClearCart();
                    return Json(new { redirectUrl = Url.Action("CheckOutSuccess", "ShoppingCart"), Success = true });
                }
            }
            return Json(code);
        }
        public ActionResult Partial_CheckOut()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = "false", msg = "", code = -1,count=0 };
            var dbConnect = new ApplicationDbContext();
            var checkProduct = dbConnect.Products.FirstOrDefault(x => x.Id==id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias=checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(X => X.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    var sale = (decimal)checkProduct.PriceSale;
                    var pricesale = (item.Price)-(sale * 1/100 * item.Price);
                    item.Price = pricesale;
                }
                item.Total = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = "true", msg = "Thêm sản phẩm vào giỏ hàng thành công", code = 1,count=cart.items.Count};
            }
            return Json(code);
        }

        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = "false", msg = "", code = -1, count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = "true", msg = "", code = 1, count = cart.items.Count };
                }
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
    }
}