using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SugarBakery.Models;

namespace SugarBakery.Models
{
    public class Giohang
    {
        dbSugarBakeryDataContext data = new dbSugarBakeryDataContext();

        public int iMasp { set; get; }
        public string sTensp { set; get; }
        public string sMota { set; get; }
        public double dDongia { set; get; }
        public string sAnhbia { set; get; }
        public int iSoluong { set; get; }
        public int soluongton { set; get; }
        public double dThanhtien { get { return iSoluong * dDongia; } }
        public string diachi { set; get; }

        public string tenvc { set; get; }

        public Giohang(int MaSP)
        {
            iMasp = MaSP;
            tbSanPham sp = data.tbSanPhams.Single(s => s.MaSP == iMasp);
            // VANCHUYEN vc = context.VANCHUYENs.Single(s => s.MaVC == mavc);
            sMota = sp.MoTa;
            sTensp = sp.TenSP;
            sAnhbia = sp.AnhSP;
            dDongia = double.Parse(sp.GiaBan.ToString());
            iSoluong = 1;
            //tenvanchuyen = vc.TenVanChuyen;
            //soluongton = int.Parse(sp.Soluongton.ToString());
        }
    }
}