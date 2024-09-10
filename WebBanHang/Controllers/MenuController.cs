using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext dbConect = new ApplicationDbContext();
        // GET: Menu
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult MenuTop()
        {
            var items = dbConect.Categories.OrderBy(x => x.Position).ToList();
            return PartialView("_MenuTop", items);
        }

        public ActionResult MenuProductCategory()
        {
            var items = dbConect.ProductCategorys.ToList();
            return PartialView("_MenuProductCategory", items);
        }
        public ActionResult MenuLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            var items = dbConect.ProductCategorys.ToList();
            return PartialView("_MenuLeft", items);
        }
        public ActionResult MenuArrivals()
        {
            var items = dbConect.ProductCategorys.ToList();
            return PartialView("_MenuArrivals", items);
        }
    }
}