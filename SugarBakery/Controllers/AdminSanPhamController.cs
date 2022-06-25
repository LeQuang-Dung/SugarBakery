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
    public class AdminSanPhamController : Controller
    {
        dbSugarBakeryDataContext data = new dbSugarBakeryDataContext();
        // GET: AdminSanPham
        //------------------------------ Sản Phẩm ---------------------------------------vinh

        public ActionResult DSsanpham(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            int pagesize = 10;
            int pageNum = (page ?? 1);
            var list = data.tbSanPhams.OrderByDescending(s => s.MaSP).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        public ActionResult Loaisp(int id)
        {
            var list = data.tbLoaiSanPhams.Where(n => n.MaLoaiSP == id);
            return View(list.Single());
        }
        public ActionResult Banhkem(int id)
        {
            var list = data.tbBanhKems.Where(n => n.MaBK == id);
            return View(list.Single());
        }
        public ActionResult Nhacungcap(int id)
        {
            var list = data.tbNhaCungCaps.Where(n => n.MaNCC == id);
            return View(list.Single());
        }
        public ActionResult Donvitinh(int id)
        {
            var list = data.tbDonViTinhs.Where(n => n.MaDVT == id);
            return View(list.Single());
        }


        [HttpGet]
        public ActionResult Themsanpham()
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            ViewBag.Loai = new SelectList(data.tbLoaiSanPhams.ToList().OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaBK = new SelectList(data.tbBanhKems.ToList().OrderBy(n => n.TenBK), "MaBK", "TenBK");
            ViewBag.MaDVT = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.MaDVT), "MaDVT", "TenDVT");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themsanpham(tbSanPham sp, FormCollection collection, HttpPostedFileBase fileUpload)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            ViewBag.Loai = new SelectList(data.tbLoaiSanPhams.ToList().OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaBK = new SelectList(data.tbBanhKems.ToList().OrderBy(n => n.TenBK), "MaBK", "TenBK");
            ViewBag.MaDVT = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.MaDVT), "MaDVT", "TenDVT");
            var Tensp = collection["Ten"];
            var gia = collection["Gia"];
            var Mota = collection["textarea"];
            var Date = collection["Date"];
            var loai = collection["Loai"];
            var Banh = collection["MaBK"];
            var Dvt = collection["Dvt"];

            var filename = Path.GetFileName(fileUpload.FileName);
            var path = Path.Combine(Server.MapPath("~/images/sanpham"), filename);
            if (System.IO.File.Exists(path))
            {
                ViewBag.ThongBaoAnh = "Hình Ảnh Đã Tồn Tại";
                return View();
            }
            else
            {
                fileUpload.SaveAs(path);
            }

            sp.TenSP = Tensp;
            sp.GiaBan = decimal.Parse(gia);
            sp.MoTa = Mota;
            sp.NgayCapNhat = DateTime.Parse(Date);
            sp.MaLoaiSP = Int32.Parse(loai);
            sp.MaBK = Int32.Parse(Banh);
            sp.MaDVT = Int32.Parse(Dvt);
            sp.AnhSP = filename;
            data.tbSanPhams.InsertOnSubmit(sp);
            data.SubmitChanges();
            return RedirectToAction("DSsanpham", "AdminSanPham");
        }

        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            else
            {
                tbSanPham sp = data.tbSanPhams.SingleOrDefault(n => n.MaSP == id);
                ViewBag.MaSP = sp.MaSP;
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sp);
            }
        }
        [HttpPost, ActionName("Xoasanpham")]
        public ActionResult XacNhanXoasanpham(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            else
            {
                tbSanPham sp = data.tbSanPhams.SingleOrDefault(n => n.MaSP == id);
                ViewBag.MaSP = sp.MaSP;
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbSanPhams.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("DSsanpham");
            }
        }


        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            tbSanPham sp = data.tbSanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.Loai = new SelectList(data.tbLoaiSanPhams.ToList().OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP", sp.MaLoaiSP);
            ViewBag.MaBK = new SelectList(data.tbBanhKems.ToList().OrderBy(n => n.TenBK), "MaBK", "TenBK", sp.MaBK);
            ViewBag.MaDVT = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.MaDVT), "MaDVT", "TenDVT", sp.MaDVT);

            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        [HttpPost, ActionName("Suasanpham")]
        public ActionResult XacNhanSuasanpham(FormCollection collection, int id, HttpPostedFileBase fileUpload)
        {
            var img = "";
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }
            if (fileUpload != null)
            {
                img = Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/images/sanpham"), img);
                if (!System.IO.File.Exists(path))//Sản Phẩm Chưa Tồn Tại
                {
                    fileUpload.SaveAs(path);
                }
            }
            else
            {
                img = collection["Anh"];
            }
            tbSanPham sp = data.tbSanPhams.SingleOrDefault(n => n.MaSP == id);
            sp.AnhSP = img;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(sp);
            data.SubmitChanges();
            return RedirectToAction("DSsanpham");

        }


        public ActionResult Chitiet(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "SugarBakery");
            }


            tbSanPham ncc = data.tbSanPhams.SingleOrDefault(n => n.MaSP == id);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }

        //----------------------------------- Loại Sản Phẩm ------------------------------------
        public ActionResult DSloaisanpham()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Themlsp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Themlsp( tbLoaiSanPham lsp)
        {
            return View();
        }

        public ActionResult Sualsp()
        {
            return View();
        }

        [HttpPost, ActionName("SuaLSP")]
        public ActionResult XacNhanSualsp()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Xoalsp()
        {
            return View();
        }
        [HttpPost, ActionName("XoaLSP")]
        public ActionResult XacNhanXoalsp()
        {
            return View();
        }



        //----------------------------------- Bánh Kem ------------------------------------
        public ActionResult DSbanhkem()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Thembanhkem()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Thembanhkem(tbBanhKem bk)
        {
            return View();
        }

        public ActionResult Suabanhkem()
        {
            return View();
        }

        [HttpPost, ActionName("Suabanhkem")]
        public ActionResult XacNhanSuabanhkem()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Xoabanhkem(int id)
        {
            return View();
        }
        [HttpPost, ActionName("Xoabanhkem")]
        public ActionResult XacNhanXoabanhkem()
        {
            return View();
        }



        //-----------------------------------Nhà Cung Cấp---------------------------------------
        public ActionResult DSncc()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ThemNcc()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemNcc(tbNhaCungCap ncc)
        {
            return View();
        }

        [HttpGet]
        public ActionResult XoaNcc()
        {
            return View();
        }
        [HttpPost, ActionName("XoaNcc")]
        public ActionResult XacNhanXoaNcc()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SuaNcc()
        {
            return View();
        }

        [HttpPost, ActionName("SuaNcc")]
        public ActionResult XacNhanSuaNcc()
        {
            return View();
        }



        //----------------------------------- Đơn Vị Tính ---------------------------------------
        public ActionResult DSdvt()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ThemDvt()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemDvt(tbDonViTinh dvt)
        {
            return View();
        }

        [HttpGet]
        public ActionResult XoaDvt()
        {
            return View();
        }
        [HttpPost, ActionName("XoaDvt")]
        public ActionResult XacNhanXoaDvt()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SuaDvt()
        {
            return View();
        }

        [HttpPost, ActionName("SuaDvt")]
        public ActionResult XacNhanSuaDvt()
        {
            return View();
        }
    }
}