using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;

namespace WebBanHang.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext dbConnect = new ApplicationDbContext();
        // GET: News
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_News()
        {
            var items = dbConnect.News.Take(3).ToList();
            return PartialView(items);
        }
    }
}