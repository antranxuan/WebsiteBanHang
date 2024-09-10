using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Models.EF;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class CategoryController : Controller
    {
        ApplicationDbContext dbConect = new ApplicationDbContext();
        // GET: Admin/Category
        public ActionResult Index()
        {
            var items = dbConect.Categories.ToList();
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHang.Models.Common.Filter.FilterChar(model.Title);
                dbConect.Categories.Add(model);
                dbConect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = dbConect.Categories.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                dbConect.Categories.Attach(model);
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                model.Alias = WebBanHang.Models.Common.Filter.FilterChar(model.Title);
                dbConect.Entry(model).Property(x => x.Title).IsModified = true;
                dbConect.Entry(model).Property(x => x.Description).IsModified = true;
                dbConect.Entry(model).Property(x => x.Alias).IsModified = true;
                dbConect.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                dbConect.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                dbConect.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                dbConect.Entry(model).Property(x => x.Position).IsModified = true;
                dbConect.Entry(model).Property(x => x.ModifiedBy).IsModified = true;
                dbConect.Entry(model).Property(x => x.ModifiedDate).IsModified = true;

                dbConect.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = dbConect.Categories.Find(id);
            if (item != null)
            {
                dbConect.Categories.Remove(item);
                dbConect.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}