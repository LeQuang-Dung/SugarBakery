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
            int pageSize = 5;
            //tao bien so trang
            int pageNum = (page ?? 1);

            //Lay top 5 san pham ban chay nhat
            var sanphammoi = Laysanpham(15);
            return View(sanphammoi.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Details(int id)
        {
            var sanpham = from s in data.tbSanPhams
                          where s.MaSP == id
                          select s;
            return View(sanpham.Single());
        }

        public ActionResult BanhKem()
        {
            var banhkem = from cd in data.tbBanhKems select cd;
            return PartialView(banhkem);
        }

        public ActionResult LoaiSP()
        {
            var loaisp = from cd in data.tbLoaiSanPhams select cd;
            return PartialView(loaisp);
        }

        public ActionResult SPTheoBanhKem(int id)
        {
            var sanpham = from s in data.tbSanPhams where s.MaBK == id select s;
            return View(sanpham);
        }

        public ActionResult SPTheoLoai(int id)
        {
            var sanpham = from s in data.tbSanPhams where s.MaLoaiSP == id select s;
            return View(sanpham);
        }

        public ActionResult Home()
        {
            return View();
        }
    }
}