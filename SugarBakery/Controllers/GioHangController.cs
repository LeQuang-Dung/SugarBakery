using System;
using System.Collections.Generic;
using System.Linq;
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
        //public List<GioHang> Laygiohang()
        //{
        //    List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
        //    if (lstGiohang == null)
        //    {
        //        //Neu gio hang chua ton tai thi khoi tao listGiohang
        //        lstGiohang = new List<Giohang>();
        //        Session["Giohang"] = lstGiohang;
        //    }
        //    return lstGiohang;
        //}

        //public ActionResult ThemGiohang(int iMaSP, string strURL)
        //{
        //    //Lay ra Session gio hang
        //    List<Giohang> lstGiohang = Laygiohang();
        //    //Kiem tra sp nay ton tai trong Session["Giohang"] chua?
        //    Giohang sanpham = lstGiohang.Find(n => n.iMaSP == iMaSP);
        //    if (sanpham == null)
        //    {
        //        sanpham = new Giohang(iMaSP);
        //        lstGiohang.Add(sanpham);
        //        return Redirect(strURL);
        //    }
        //    else
        //    {
        //        sanpham.iSoluong++;
        //        return Redirect(strURL);
        //    }
        //}

        //Tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            //    List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            //    if (lstGiohang != null)
            //    {
            //        iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            //    }
            return iTongSoLuong;
        }

        //Tinh tong tien
        private double TongTien()
        {
            double iTongTien = 0;
            //    List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            //    if (lstGiohang != null)
            //    {
            //        iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            //    }
            return iTongTien;
        }

        //Xay dung trang gio hang
        //public ActionResult GioHang()
        //{
        //    List<Giohang> lstGiohang = Laygiohang();
        //    if (lstGiohang.Count == 0)
        //    {
        //        return RedirectToAction("Index", "NongSanZeno");
        //    }
        //    ViewBag.TongsoLuong = TongSoLuong();
        //    ViewBag.Tongtien = TongTien();
        //    return View(lstGiohang);
        //}

        public ActionResult GiohangPartial()
        {
            //    ViewBag.Tongsoluong = TongSoLuong();
            //    ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //Xoa gio hang
        public ActionResult XoaGiohang(int iMaSP)
        {
            //    //Lay gio hang tu session
            //    List<Giohang> lstGiohang = Laygiohang();
            //    //Kiem tra san pham da co trong Session["Giohang"]
            //    Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            //    //Neu ton tai thi cho sua Soluong
            //    if (sanpham != null)
            //    {
            //        lstGiohang.RemoveAll(n => n.iMaSP == iMaSP);
            //        return RedirectToAction("GioHang");
            //    }
            //    if (lstGiohang.Count == 0)
            //    {
            //        return RedirectToAction("Index", "NongSanZeno");
            //    }
            return RedirectToAction("GioHang");
        }

        //Cap nhat gio hang
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            //Lay gio hang tu session
            //    List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra san pham da co trong session["Giohang"]
            //    Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            //Neu ton tai thi cho sua Soluong
            //    if (sanpham != null)
            //    {
            //        sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            //    }
            return RedirectToAction("Giohang");
        }

        public ActionResult XoaTatcaGiohang()
        {
            //    //Lay gio hang tu session
            //    List<Giohang> lstGiohang = Laygiohang();
            //    lstGiohang.Clear();
            return RedirectToAction("Index", "NongSanZeno");
        }

        //Hien thi view Dathang de cap nhat cac thong tin cho don hang
        [HttpGet]
        //public ActionResult DatHang()
        //{
        //    if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
        //    {
        //        return RedirectToAction("Dangnhap", "Nguoidung");
        //    }
        //    if (Session["Giohang"] == null)
        //    {
        //        return RedirectToAction("Index", "NongSanZeno");
        //    }

        //    //Lay gi hang tu session
        //    List<Giohang> lstGiohang = Laygiohang();
        //    ViewBag.Tongsoluong = TongSoLuong();
        //    ViewBag.Tongtien = TongTien();

        //    return View(lstGiohang);
        //}

        public ActionResult DatHang(FormCollection collection)
        {
            //    //Them don hang
            //    tbDonDatHang ddh = new tbDonDatHang();
            //    tbKhachHang kh = (tbKhachHang)Session["Taikhoan"];
            //    List<Giohang> gh = Laygiohang();
            //    ddh.MaKH = kh.MaKH;
            //    ddh.NgayDH = DateTime.Now;
            //    var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            //    ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            //    ddh.Tinhtranggiaohang = false;
            //    ddh.Dathanhtoan = false;
            //    data.tbDonDatHangs.InsertOnSubmit(ddh);
            //    data.SubmitChanges();
            //    //Them chi tiet don hang
            //    foreach (var item in gh)
            //    {
            //        tbChiTietDonHang ctdh = new tbChiTietDonHang();
            //        ctdh.MaDonHang = ddh.MaDonHang;
            //        ctdh.MaSP = item.iMaSP;
            //        ctdh.Soluong = item.iSoluong;
            //        ctdh.Dongia = (decimal)item.dDongia;
            //        data.tbChiTietDonHangs.InsertOnSubmit(ctdh);
            //    }
            //    data.SubmitChanges();
            //    Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }

        public ActionResult Xacnhandonhang()
        {
            return View();
        }

    }
}