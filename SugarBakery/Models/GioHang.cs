﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SugarBakery.Models;

namespace SugarBakery.Models
{
    public class GioHang
    {
        dbSugarBakeryDataContext data = new dbSugarBakeryDataContext();

        public int masp { set; get; }
        public string tensp { set; get; }
        public string mota { set; get; }
        public double dongia { set; get; }
        public string anhbia { set; get; }
        public int soluong { set; get; }
        public int soluongton { set; get; }
        public double thanhtien { get { return dongia * soluong; } }
        public string diachi { set; get; }

        public string tenvc { set; get; }

        public GioHang(int MaSP)
        {
            masp = MaSP;
            tbSanPham sp = data.tbSanPhams.Single(s => s.MaSP == masp);
            // VANCHUYEN vc = context.VANCHUYENs.Single(s => s.MaVC == mavc);
            mota = sp.MoTa;
            tensp = sp.TenSP;
            anhbia = sp.AnhSP;
            dongia = double.Parse(sp.GiaBan.ToString());
            soluong = 1;
        }
    }
}