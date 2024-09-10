using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{

    public class BlogsController : Controller
    {
        private ApplicationDbContext dbConect = new ApplicationDbContext();
        // GET: Blogs
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Blog()
        {
            var items = dbConect.News.Where(x => x.IsActive).Take(3).ToList();
            return PartialView(items);
        }
    }
}