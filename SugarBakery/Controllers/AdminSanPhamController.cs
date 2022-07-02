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
                return RedirectToAction("SanPham", "SugarBakery");
            }
            int pagesize = 8;
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
                return RedirectToAction("SanPham", "SugarBakery");
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
                return RedirectToAction("SanPham", "SugarBakery");
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
            var dvt = collection["Dvt"];

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
            //sp.MaDVT = Int32.Parse(dvt);
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
                return RedirectToAction("SanPham", "SugarBakery");
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
                return RedirectToAction("SanPham", "SugarBakery");
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
                return RedirectToAction("SanPham", "SugarBakery");
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
                return RedirectToAction("SanPham", "SugarBakery");
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


        public ActionResult Chitietsanpham(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
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
        public ActionResult DSbanhkem(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbBanhKems.OrderByDescending(s => s.MaBK).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }

        [HttpGet]
        public ActionResult Thembanhkem()
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            ViewBag.MaBK = new SelectList(data.tbBanhKems.ToList().OrderBy(n => n.TenBK), "MaBK", "TenBK");
            return View();
        }
        [HttpPost]
        public ActionResult Thembanhkem(tbBanhKem bk, FormCollection collection, HttpPostedFileBase fileUpload)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            ViewBag.MaBK = new SelectList(data.tbBanhKems.ToList().OrderBy(n => n.TenBK), "MaBK", "TenBK");
            if (ModelState.IsValid)
            {  
                data.tbBanhKems.InsertOnSubmit(bk);
                data.SubmitChanges();
            }
            return RedirectToAction("DSbanhkem", "AdminSanPham");
        }

        public ActionResult Suabanhkem(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            tbBanhKem bk = data.tbBanhKems.SingleOrDefault(n => n.MaBK == id);
            ViewBag.MaBK = new SelectList(data.tbBanhKems.ToList().OrderBy(n => n.TenBK), "MaBK", "TenBK", bk.MaBK);
            if(bk == null )
            {
                Response.StatusCode = 404;
                return null;
            }    
            
            return View(bk);
        }

        [HttpPost, ActionName("Suabanhkem")]
        public ActionResult XacNhanSuabanhkem(FormCollection collection, int id)
        {
            var img = "";
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }

            tbBanhKem bk = data.tbBanhKems.SingleOrDefault(n => n.MaBK == id);

            if (bk == null )
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(bk);
            data.SubmitChanges();
            return RedirectToAction("DSbanhkem");
        }

        [HttpGet]
        public ActionResult Xoabanhkem(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            else
            {
                tbBanhKem bk = data.tbBanhKems.SingleOrDefault(n => n.MaBK == id);

                ViewBag.MaBK = bk.MaBK;
                if (bk == null  )
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(bk);
            }
        }
        [HttpPost, ActionName("Xoabanhkem")]
        public ActionResult XacNhanXoabanhkem(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            else
            {
                tbBanhKem bk = data.tbBanhKems.SingleOrDefault(n => n.MaBK == id);
                ViewBag.MaBK = bk.MaBK;
                if (bk == null )
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbBanhKems.DeleteOnSubmit(bk);
                data.SubmitChanges();
                return RedirectToAction("DSbanhkem");
            }
        }



        //-----------------------------------Nhà Cung Cấp---------------------------------------
        public ActionResult DSncc(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbNhaCungCaps.OrderByDescending(s => s.MaNCC).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
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
        public ActionResult DSdvt(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbDonViTinhs.OrderByDescending(s => s.MaDVT).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        [HttpGet]
        public ActionResult ThemDvt()
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            ViewBag.MaDVT = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.TenDVT), "MaDVT", "TenDVT","MoTa");
            return View();
        }
        [HttpPost]
        public ActionResult ThemDvt(tbDonViTinh dvt)
        {
            if(Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            ViewBag.MaDVT = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.TenDVT), "MaDVT", "TenDVT", "MoTa");
            if (ModelState.IsValid)
            {
                data.tbDonViTinhs.InsertOnSubmit(dvt);
                data.SubmitChanges();
            }
            return RedirectToAction("DSdvt", "AdminSanPham");
        }

        public ActionResult Suadvt(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            tbDonViTinh dvt = data.tbDonViTinhs.SingleOrDefault(n => n.MaDVT == id);
            ViewBag.MaBK = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.TenDVT), "MaDVT", "TenDVT", "MoTa", dvt.MaDVT);
            if (dvt == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(dvt);
        }

        [HttpPost, ActionName("Suadvt")]
        public ActionResult XacNhanSuadvt(FormCollection collection, int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }

            tbDonViTinh dvt = data.tbDonViTinhs.SingleOrDefault(n => n.MaDVT == id);

            if (dvt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(dvt);
            data.SubmitChanges();
            return RedirectToAction("DSdvt");
        }

        [HttpGet]
        public ActionResult Xoadvt(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            else
            {
                tbDonViTinh dvt = data.tbDonViTinhs.SingleOrDefault(n => n.MaDVT == id);

                ViewBag.MaBK = dvt.MaDVT;
                if (dvt == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(dvt);
            }
        }
        [HttpPost, ActionName("Xoadvt")]
        public ActionResult XacNhanXoadvt(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            else
            {
                tbDonViTinh dvt = data.tbDonViTinhs.SingleOrDefault(n => n.MaDVT == id);
                ViewBag.MaDVT = dvt.MaDVT;
                if (dvt == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbDonViTinhs.DeleteOnSubmit(dvt);
                data.SubmitChanges();
                return RedirectToAction("DSdvt");
            }
        }
    }
}