using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using SugarBakery.Models;

namespace SugarBakery.Controllers
{
    public class GioHangController : Controller
    {
        //Tao doi tuong data chua du lieu tu model dbnongsan da tao
        dbSugarBakeryDataContext data = new dbSugarBakeryDataContext();
        // GET: GioHang
        public List<GioHang> LayGioHang()
        {
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list == null)
            {
                list = new List<GioHang>();
                Session["GioHang"] = list;
            }
            return list;
        }

        public ActionResult ThemGioHang(int masp, string strUrl)
        {

            List<GioHang> gioHangs = LayGioHang();

            GioHang sp = gioHangs.Find(n => n.masp == masp);
            if (sp == null)
            {
                sp = new GioHang(masp);
                gioHangs.Add(sp);
                return Redirect(strUrl);
            }
            else
            {
                sp.soluong++;
                return Redirect(strUrl);
            }
        }

        private int TongSoLuong()
        {
            int Tongsoluong = 0;
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs != null)
            {
                Tongsoluong = gioHangs.Sum(n => n.soluong);
            }
            Session["TongSoLuong"] = Tongsoluong;
            return Tongsoluong;
        }

        private double TongTien()
        {
            double tongtien = 0;
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs != null)
            {
                tongtien = gioHangs.Sum(n => n.thanhtien);
            }
            return tongtien;
        }

        public ActionResult Giohang()
        {
            List<GioHang> gioHangs = LayGioHang();
            if (gioHangs.Count == 0)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(gioHangs);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> gioHangs = LayGioHang();
            GioHang sessiongiohang = gioHangs.SingleOrDefault(n => n.masp == id);
            if (sessiongiohang != null)
            {
                gioHangs.RemoveAll(n => n.masp == id);
                return RedirectToAction("GioHang");
            }
            if (gioHangs.Count == 0)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int id, FormCollection f)
        {
            List<GioHang> gioHangs = LayGioHang();
            GioHang sessiongiohang = gioHangs.SingleOrDefault(n => n.masp == id);
            if (sessiongiohang != null)
            {
                sessiongiohang.soluong = int.Parse(f["Soluong"].ToString());

            }
            return RedirectToAction("Giohang");
        }

        public ActionResult RemoveAll()
        {
            List<GioHang> gioHangs = LayGioHang();
            gioHangs.Clear();
            return RedirectToAction("SanPham", "SugarBakery");
        }
        [HttpGet]
        public ActionResult DatHang()
        {

            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            List<GioHang> gioHangs = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(gioHangs);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            tbSanPham sANPHAM = new tbSanPham();
            tbDonHang dONDATHANG = new tbDonHang();
            tbChiTietDonHang CTDH = new tbChiTietDonHang();
            tbKhachHang kh = (tbKhachHang)Session["Taikhoan"];
            List<GioHang> gioHangs = LayGioHang();
            dONDATHANG.MaKH = kh.MaKH;
            dONDATHANG.NgayDat = DateTime.Now;
            string DiaChi = collection["DiaChi"];
<<<<<<< HEAD
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.MaTTHD = 1;
            tthd.MaTTHD = (int)ddh.MaTTHD;
            data.tbDonHangs.InsertOnSubmit(ddh);
            data.tbTinhTrangHoaDons.InsertOnSubmit(tthd);
=======
            //string nvc = collection["TenVanChuyen"];
            //dONDATHANG.Ngaygiao = DateTime.Parse(ngaygiao);
            dONDATHANG.TongTien = Decimal.Parse(TongTien().ToString());
            dONDATHANG.MaTTHD = '1';
            //dONDATHANG.Dathanhtoan = false;
            data.tbDonHangs.InsertOnSubmit(dONDATHANG);
>>>>>>> 8a0c76bc855100d9789c483ea546a8fa427ec19c
            data.SubmitChanges();
            foreach (var item in gioHangs)
            {
                tbChiTietDonHang CT = new tbChiTietDonHang();
                tbSanPham SP = new tbSanPham();
                //DONDATHANG dONDATHANG = new DONDATHANG();
                CT.MaDH = dONDATHANG.MaDH;
                CT.MaSP = item.masp;
                CT.SoLuong = item.soluong;
                CT.DonGia = (decimal)item.dongia;
                CT.ThanhTien = (decimal)item.thanhtien;
                dONDATHANG.DiaChi = DiaChi;
                //              dONDATHANG.MaVC = Int32.Parse(nvc);
                data.tbChiTietDonHangs.InsertOnSubmit(CT);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
   
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}