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
    public class HomeController : Controller
    {
        dbSugarBakeryDataContext data = new dbSugarBakeryDataContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GioiThieu()
        {
            return View();
        }

        public ActionResult TinTuc()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LienHe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LienHe(FormCollection collection)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "NguoiDung");
            }
            else
            {
                tbPhanHoiKH ht = new tbPhanHoiKH();
                tbKhachHang kh = (tbKhachHang)Session["Taikhoan"];
                ht.MaKH = kh.MaKH;
                ht.HoTen = kh.TenKH;
                ht.Email = kh.Email;
                string lydo = collection["LyDo"];
                ht.LyDo = lydo;
                if (lydo == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    data.tbPhanHoiKHs.InsertOnSubmit(ht);
                    data.SubmitChanges();
                    return RedirectToAction("Contact", "Home");
                }
            }
        }
    }
}