using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SugarBakery.Models;

namespace SugarBakery.Controllers
{
    public class AdminController : Controller
    {
        dbSugarBakeryDataContext data = new dbSugarBakeryDataContext();
        // GET: Admin
        public ActionResult index()
        {
            return View();
        }

        public ActionResult Sanpham()
        {
            return View(data.tbSanPhams.ToList());
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
        //        ViewData["Loi1"] = "Phải nhập tên đăng nhập*";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
        //        ViewData["Loi2"] = "Phải nhập mật khẩu*";
            }
            else
            {
        //        //Gán giá trị cho đối tượng được tạo mới (ad)
        //        tbUser ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
        //        if (ad != null)
        //        {
        //            Session["Taikhoanadmin"] = ad;
        //            return RedirectToAction("Index", "Admin");
        //        }
        //        else
        //        {
        //            ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng*";
        //        }
            }
            return View();
        }
    }
}