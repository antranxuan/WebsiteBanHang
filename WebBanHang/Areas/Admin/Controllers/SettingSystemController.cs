using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHang.Models;
using WebBanHang.Models.EF;

namespace WebBanHang.Areas.Admin.Controllers
{
    public class SettingSystemController : Controller
    {
        private ApplicationDbContext dbConnect = new ApplicationDbContext();
        // GET: Admin/SettingSystem
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_Setting()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddSetting(SettingSystemViewModel req)
        {
            SettingSystem set = null;
            var checkTitle = dbConnect.SettingSystems.FirstOrDefault(x => x.SettingKey.Contains("SettingTitle"));
            if (checkTitle == null)
            {
                set = new SettingSystem();
                set.SettingKey = "SettingTitle";
                set.SettingValue = req.SettingTitle;
                dbConnect.SettingSystems.Add(set);
            }
            else
            {
                checkTitle.SettingValue = req.SettingTitle;
                dbConnect.Entry(checkTitle).State = System.Data.Entity.EntityState.Modified;
            }
            //logo
            var checkLogo = dbConnect.SettingSystems.FirstOrDefault(x => x.SettingKey.Contains("SettingLogo"));
            if (checkLogo == null)
            {
                set = new SettingSystem();
                set.SettingKey = "SettingLogo";
                set.SettingValue = req.SettingLogo;
                dbConnect.SettingSystems.Add(set);
            }
            else
            {
                checkLogo.SettingValue = req.SettingLogo;
                dbConnect.Entry(checkLogo).State = System.Data.Entity.EntityState.Modified;
            }
            //Email
            var email = dbConnect.SettingSystems.FirstOrDefault(x => x.SettingKey.Contains("SettingEmail"));
            if (email == null)
            {
                set = new SettingSystem();
                set.SettingKey = "SettingEmail";
                set.SettingValue = req.SettingEmail;
                dbConnect.SettingSystems.Add(set);
            }
            else
            {
                email.SettingValue = req.SettingEmail;
                dbConnect.Entry(email).State = System.Data.Entity.EntityState.Modified;
            }
            //Hotline
            var Hotline = dbConnect.SettingSystems.FirstOrDefault(x => x.SettingKey.Contains("SettingHotline"));
            if (Hotline == null)
            {
                set = new SettingSystem();
                set.SettingKey = "SettingHotline";
                set.SettingValue = req.SettingHotline;
                dbConnect.SettingSystems.Add(set);
            }
            else
            {
                Hotline.SettingValue = req.SettingHotline;
                dbConnect.Entry(Hotline).State = System.Data.Entity.EntityState.Modified;
            }
            //TitleSeo
            var TitleSeo = dbConnect.SettingSystems.FirstOrDefault(x => x.SettingKey.Contains("SettingTitleSeo"));
            if (TitleSeo == null)
            {
                set = new SettingSystem();
                set.SettingKey = "SettingTitleSeo";
                set.SettingValue = req.SettingTitleSeo;
                dbConnect.SettingSystems.Add(set);
            }
            else
            {
                TitleSeo.SettingValue = req.SettingTitleSeo;
                dbConnect.Entry(TitleSeo).State = System.Data.Entity.EntityState.Modified;
            }
            //DessSeo
            var DessSeo = dbConnect.SettingSystems.FirstOrDefault(x => x.SettingKey.Contains("SettingDesSeo"));
            if (DessSeo == null)
            {
                set = new SettingSystem();
                set.SettingKey = "SettingDesSeo";
                set.SettingValue = req.SettingDesSeo;
                dbConnect.SettingSystems.Add(set);
            }
            else
            {
                DessSeo.SettingValue = req.SettingDesSeo;
                dbConnect.Entry(DessSeo).State = System.Data.Entity.EntityState.Modified;
            }
            //KeySeo
            var KeySeo = dbConnect.SettingSystems.FirstOrDefault(x => x.SettingKey.Contains("SettingKeySeo"));
            if (KeySeo == null)
            {
                set = new SettingSystem();
                set.SettingKey = "SettingKeySeo";
                set.SettingValue = req.SettingKeySeo;
                dbConnect.SettingSystems.Add(set);
            }
            else
            {
                KeySeo.SettingValue = req.SettingKeySeo;
                dbConnect.Entry(KeySeo).State = System.Data.Entity.EntityState.Modified;
            }
            dbConnect.SaveChanges();

            return View("Partial_Setting");
        }
    }
}