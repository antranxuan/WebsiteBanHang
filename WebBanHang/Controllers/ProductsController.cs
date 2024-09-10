using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext dbConect = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index()
        {
            var items = dbConect.Products.ToList();
            
            return View(items);
        }
        public ActionResult Detail(string alias, int id)
        {
            var item = dbConect.Products.Find(id);
            if (item != null)
            {
                dbConect.Products.Attach(item);
                item.ViewCount = item.ViewCount + 1;
                dbConect.Entry(item).Property(x => x.ViewCount).IsModified = true;
                dbConect.SaveChanges();
            }
            
            return View(item);
        }
        public ActionResult ProductCategory(int? id)
        {
            var items = dbConect.Products.ToList();
            if (id != null)
            {
                items = items.Where(x => x.ProductCategoryId == id).ToList();
            }
            var cate = dbConect.ProductCategorys.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            ViewBag.CateId = id;
            return View(items);
        }
        public ActionResult Partial_ItemsByCateId()
        {
            var items = dbConect.Products.Where(x => x.IsHome && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }
        public ActionResult Partial_ProductSale()
        {
            var items = dbConect.Products.Where(x => x.IsSale && x.IsActive).Take(12).ToList();
            return PartialView(items);
        }
    }
}