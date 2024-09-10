using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using PagedList;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private ApplicationDbContext dbConect = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            var items = dbConect.Orders.OrderByDescending(x => x.CreatedDate).ToList();
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult View(int id)
        {
            var item = dbConect.Orders.Find(id);
            return View(item);
        }

        public ActionResult Partial_Sanpham(int id)
        {
            
            var items = dbConect.OrderDetails.Where(x => x.OrderId == id).ToList();
            return PartialView(items);
        }
        [HttpPost]
        public ActionResult UpdateTT(int id, int trangthai)
        {
            var item = dbConect.Orders.Find(id);
            if (item != null)
            {
                dbConect.Orders.Attach(item);
                item.TypePayment = trangthai;
                dbConect.Entry(item).Property(x => x.TypePayment).IsModified = true;
                dbConect.SaveChanges();
                return Json(new { messages = "Cập nhật thành công", success = true });
            }
            return Json(new { messages = "Cập nhật không thành công", success = false });
        }
    }
}