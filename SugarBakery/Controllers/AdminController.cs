using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SugarBakery.Models;
using PagedList;
using PagedList.Mvc;

namespace SugarBakery.Controllers
{
    public class AdminController : Controller
    {
        dbSugarBakeryDataContext data = new dbSugarBakeryDataContext();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            return View();
        }
        [HttpGet]
        public ActionResult DangNhapAD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapAD(FormCollection collection)
        {
            string user = collection["form-username"];
            string pass = collection["form-password"];

            tbAdmin ad = data.tbAdmins.SingleOrDefault(a => a.UserAdmin == user && a.PassAdmin == pass );
            if (ad == null)
            {
                ViewBag.ThongBaoAdmin = "Tài Khoản Hoặc Mật Khẩu Sai";
                return this.DangNhapAD();
            }
            Session["TKadmin"] = ad;
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult QuenMatKhauAD()
        {
            return View();
        }
    }
}