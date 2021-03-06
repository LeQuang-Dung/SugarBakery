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

        // GET: SugarBakery
        private List<tbSanPham> Laysanpham(int count)
        {
            return data.tbSanPhams.OrderByDescending(a => a.MaSP).Take(count).ToList();
        }
        public ActionResult TrangChu()
        {
            //Lay top 6 san pham ban chay nhat
            var sanpham = Laysanpham(4);
            string s = Request.QueryString["s"];
            if (!string.IsNullOrEmpty(s)) sanpham = data.tbSanPhams.OrderByDescending(a => a.MaSP).Take(4).Where(w => w.TenSP.Contains(s)).ToList();
            return View(sanpham);
        }

        public ActionResult GioiThieu()
        {
            return View();
        }

        public ActionResult TinTuc()
        {
            return View();
        }

        public ActionResult KhuyenMai()
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
                return RedirectToAction("Dangnhap", "NguoiDung");
            }
            else
            {
                tbPhanHoiKH ht = new tbPhanHoiKH();
                tbKhachHang kh = (tbKhachHang)Session["Taikhoan"];
                ht.MaKH = kh.MaKH;
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
                    return RedirectToAction("LienHe", "Home");
                }
            }
        }
    }
}