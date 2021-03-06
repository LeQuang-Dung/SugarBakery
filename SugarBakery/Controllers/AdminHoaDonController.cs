using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using SugarBakery.Models;
using PagedList;
using System.Configuration;
using System.Data.SqlClient;

namespace SugarBakery.Controllers
{
    public class AdminHoaDonController : Controller
    {
        dbSugarBakeryDataContext data = new dbSugarBakeryDataContext();


        public string ReturnDateForDisplay
        {
            get
            {
                return this.ReturnDateForDisplay.ToString();
            }
        }


        // GET: AdminHoaDon
        //------------------------------ Phản  Hồi Khách Hàng ------------------------------------
        public ActionResult DSPhanHoi(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbPhanHoiKHs.OrderByDescending(n => n.STT).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }

        public ActionResult ChiTietPhanHoi(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            tbPhanHoiKH ph = data.tbPhanHoiKHs.SingleOrDefault(n => n.STT == id);
            if (ph == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ph);
        }
        [HttpGet]
        public ActionResult XoaPhanHoi(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            else
            {
                tbPhanHoiKH ph = data.tbPhanHoiKHs.SingleOrDefault(n => n.STT == id);
                ViewBag.STT = ph.STT;
                if (ph == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ph);
            }
        }
        [HttpPost, ActionName("XoaPhanHoi")]
        public ActionResult XacNhanXoaPhanHoi(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            else
            {
                tbPhanHoiKH ph = data.tbPhanHoiKHs.SingleOrDefault(n => n.STT == id);
                ViewBag.STT = ph.STT;
                if (ph == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbPhanHoiKHs.DeleteOnSubmit(ph);
                data.SubmitChanges();
                return RedirectToAction("DSPhanHoi"); ;
            }
        }

        //--------------------------- Khách Hàng ----------------------------------
        public ActionResult DSkh(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbKhachHangs.OrderByDescending(n => n.MaKH).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        public ActionResult ChiTietKH(int id , FormCollection collection)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            tbKhachHang kh = data.tbKhachHangs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpGet]
        public ActionResult Xoakh(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            else
            {
                tbKhachHang kh = data.tbKhachHangs.SingleOrDefault(n => n.MaKH == id);
                ViewBag.MaKH = kh.MaKH;
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
            }
        }
        [HttpPost, ActionName("Xoakh")]
        public ActionResult XacNhanXoakh(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            else
            {
                tbKhachHang kh = data.tbKhachHangs.SingleOrDefault(n => n.MaKH == id);
                ViewBag.MaKH = kh.MaKH;
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbKhachHangs.DeleteOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DSkh"); ;
            }
        }

        //--------------------------- Đơn Đặt Hàng ----------------------------------
        [HttpGet]
        public ActionResult DSdonhang(int? page)
        {
            int pagesize = 5;
            int pageNum = (page ?? 1);
            var GioHienTai = DateTime.Today;
            var list = data.tbDonHangs.Where(s => s.NgayDat >= GioHienTai).OrderByDescending(i => i.NgayDat).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        [HttpPost]
        public ActionResult DSdonhang(string date, string date2, int? page)
        {
            int pagesize = 5;
            int pageNum = (page ?? 1);
            var Date = DateTime.Parse(date);

            if (date2 == "")
            {
                var listdate = data.tbDonHangs.Where(s => s.NgayDat >= Date).OrderByDescending(i => i.NgayDat).ToList();
                return View(listdate.ToPagedList(pageNum, pagesize));
            }
            var Date2 = DateTime.Parse(date2);
            var list = data.tbDonHangs.Where(s => s.NgayDat >= Date && s.NgayDat <= Date2).OrderByDescending(i => i.NgayDat).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }

        public ActionResult ChiTietdonhang(int? id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            if (id == null)
            {
                return HttpNotFound();
            }
            var list = data.tbChiTietDonHangs.Where(s => s.MaDH == id).OrderByDescending(s => s.MaSP).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Suadonhang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }

            tbDonHang dh = data.tbDonHangs.SingleOrDefault(n => n.MaDH == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dh);
        }

        [HttpPost, ActionName("Suadonhang")]
        public ActionResult XacNhanSuaDonDatHang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            else
            {
                tbDonHang dh = data.tbDonHangs.SingleOrDefault(n => n.MaDH == id);
                if (dh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                UpdateModel(dh);
                data.SubmitChanges();
                return RedirectToAction("DSdonhang");
            }
        }
        public List<tbChiTietDonHang> LayChiTietDonDatHang()
        {
            List<tbChiTietDonHang> list = Session["chitietdonhang"] as List<tbChiTietDonHang>;
            if (list == null)
            {
                list = new List<tbChiTietDonHang>();
                Session["chitietdonhang"] = list;
            }
            return list;
        }

        public ActionResult XoaTatCaChiTiet()
        {
            List<tbChiTietDonHang> list = LayChiTietDonDatHang();
            list.Clear();
            return RedirectToAction("DonDatHang");
        }
        public ActionResult XoaChiTietDonDatHang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            else
            {
                tbChiTietDonHang ctdh = data.tbChiTietDonHangs.Where(n => n.MaDH == id).FirstOrDefault();
                ViewBag.MaDonHang = ctdh.MaDH;
                if (ctdh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ctdh);
            }
        }
        [HttpPost, ActionName("XoaChiTietDonDatHang")]
        public ActionResult XacNhanXoaChiTietDonDatHang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            else
            {
                tbChiTietDonHang ctdh = data.tbChiTietDonHangs.Where(n => n.MaDH == id).FirstOrDefault();

                ViewBag.MaDonHang = ctdh.MaDH;
                if (ctdh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbChiTietDonHangs.DeleteOnSubmit(ctdh);
                data.SubmitChanges();
                return RedirectToAction("DSdonhang", "AdminHoaDon");
            }
        }
        public ActionResult Xoadonhang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            else
            {
                tbDonHang dh = data.tbDonHangs.Where(n => n.MaDH == id).FirstOrDefault();
                // DONDATHANG ncc = context.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
                ViewBag.MaDonHang = dh.MaDH;
                if (dh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(dh);
            }
        }
        [HttpPost, ActionName("Xoadonhang")]
        public ActionResult XacNhanXoadonhang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            else
            {
                tbDonHang dh = data.tbDonHangs.Where(n => n.MaDH == id).FirstOrDefault();

                ViewBag.MaDonHang = dh.MaDH;
                if (dh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbDonHangs.DeleteOnSubmit(dh);
                data.SubmitChanges();
                return RedirectToAction("DSdonhang");
            }
        }
    }
}