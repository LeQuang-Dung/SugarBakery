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
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstgh = Session["GioHang"] as List<GioHang>;
            if (lstgh == null)
            {
                //Neu gio hang chua ton tai thi khoi tao listGiohang
                lstgh = new List<GioHang>();
                Session["GioHang"] = lstgh;
            }
            return lstgh;
        }

        public ActionResult ThemGiohang(int masp, string strURL)
        {
            //Lay ra Session gio hang
            List<GioHang> lstgh = Laygiohang();
            //Kiem tra sp nay ton tai trong Session["Giohang"] chua?
            GioHang sp = lstgh.Find(n => n.masp == masp);
            if (sp == null)
            {
                sp = new GioHang(masp);
                lstgh.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.soluong++;
                return Redirect(strURL);
            }
        }

        //Tong so luong
        private int TongSoLuong()
        {
            int Tongsoluong = 0;
            List<GioHang> lstgh = Session["GioHang"] as List<GioHang>;
            if (lstgh != null)
            {
                Tongsoluong = lstgh.Sum(n => n.soluong);
            }
            Session["TongSoLuong"] = Tongsoluong;
            return Tongsoluong;
        }

        //Tinh tong tien
        private double TongTien()
        {
            double tongtien = 0;
            List<GioHang> lstgh = Session["GioHang"] as List<GioHang>;
            if (lstgh != null)
            {
                tongtien = lstgh.Sum(n => n.thanhtien);
            }
            return tongtien;
        }

        //Xay dung trang gio hang
        public ActionResult Giohang()
        {
            List<GioHang> lstgh = Laygiohang();
            if (lstgh.Count == 0)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstgh);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.TongSoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //Xoa gio hang
        public ActionResult XoaGiohang(int iMasp)
        {
            //Lay gio hang tu session
            List<GioHang> lstgh = Laygiohang();
            //Kiem tra san pham da co trong Session["Giohang"]
            GioHang gh = lstgh.SingleOrDefault(n => n.masp == iMasp);
            //Neu ton tai thi cho sua Soluong
            if (gh != null)
            {
                lstgh.RemoveAll(n => n.masp == iMasp);
                return RedirectToAction("GioHang");
            }
            if (lstgh.Count == 0)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            return RedirectToAction("GioHang");
        }

        //Cap nhat gio hang
        public ActionResult CapnhatGiohang(int iMasp, FormCollection f)
        {
            //Lay gio hang tu session
            List<GioHang> lstgh = Laygiohang();
            //Kiem tra san pham da co trong session["Giohang"]
            GioHang gh = lstgh.SingleOrDefault(n => n.masp == iMasp);
            //Neu ton tai thi cho sua Soluong
            if (gh != null)
            {
                gh.soluong = int.Parse(f["Soluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }

        public ActionResult XoatatcaGiohang()
        {
            //Lay gio hang tu session
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("SanPham", "SugarBakery");
        }

        //Hien thi view Dathang de cap nhat cac thong tin cho don hang
        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }

            //Lay gi hang tu session
            List<GioHang> lstgh = Laygiohang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(lstgh);
        }

        [HttpPost]
        public ActionResult Dathang(FormCollection collection)
        {
            //Them don hang
            tbTinhTrangHoaDon tthd = new tbTinhTrangHoaDon();
            tbDonHang ddh = new tbDonHang();
            tbKhachHang kh = (tbKhachHang)Session["Taikhoan"];
            List<GioHang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            string DiaChi = collection["DiaChi"];
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.MaTTHD = 1;
            //ddh.Dathanhtoan = false;
            data.tbDonHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //Them chi tiet don hang
            foreach (var item in gh)
            {
                tbChiTietDonHang ctdh = new tbChiTietDonHang();
                ctdh.MaDH = ddh.MaDH;
                ctdh.MaSP = item.masp;
                ctdh.SoLuong = item.soluong;
                ctdh.DonGia = (decimal)item.dongia;
                ctdh.ThanhTien = (decimal)item.thanhtien;
                ddh.DiaChi = DiaChi;
                data.tbChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Xacnhandonhang", "GioHang");
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }

    }
}