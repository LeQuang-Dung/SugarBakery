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
    public class SugarBakeryController : Controller
    {
        dbSugarBakeryDataContext data = new dbSugarBakeryDataContext();
        // GET: SugarBakery
        private List<tbSanPham> Laysanpham(int count)
        {
            return data.tbSanPhams.OrderByDescending(a => a.MaSP).Take(count).ToList();
        }
        public ActionResult Index(int? page)
        {
            //Tao bien quy dinh so san pham tren moi trang
            int pageSize = 6;
            //tao bien so trang
            int pageNum = (page ?? 1);

            //Lay top 6 san pham ban chay nhat
            var sanphammoi = Laysanpham(15);
            return View(sanphammoi.ToPagedList(pageNum, pageSize));
        }

        //==================== chi tiet san pham ====================
        public ActionResult ChiTiet(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var chitietSP = from s in data.tbSanPhams where s.MaSP == id select s;
            return View(chitietSP.Single());
        }

        //==================== tat ca san pham ====================
        public ActionResult TatCaSanPham(int? page)
        {
            var allSP = from sp in data.tbSanPhams select sp;

            int pagesize = 6;
            int pageNum = (page ?? 1);
            return View(allSP.ToPagedList(pageNum, pagesize));
        }

        //==================== san pham theo loai banh ====================
        public ActionResult BanhKem()
        {
            var banhkem = from cd in data.tbBanhKems select cd;
            return PartialView(banhkem);
        }

        public ActionResult SPTheoBanhKem(int? id, int? page)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            int pagesize = 6;
            int pageNum = (page ?? 1);
            var SPbk = from sp in data.tbSanPhams where sp.MaBK == id select sp;
            return View(SPbk.ToPagedList(pageNum, pagesize));
        }

        //==================== san pham theo loai ====================
        public ActionResult LoaiSP()
        {
            var loaisp = from cd in data.tbLoaiSanPhams select cd;
            return PartialView(loaisp);
        }

        public ActionResult SPTheoLoai(int? id, int? page)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            int pagesize = 6;
            int pageNum = (page ?? 1);
            var SPLoai = from sp in data.tbSanPhams where sp.MaLoaiSP == id select sp;
            return View(SPLoai.ToPagedList(pageNum, pagesize));
        }
    }
}