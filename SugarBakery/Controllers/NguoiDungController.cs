using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SugarBakery.Models;

namespace SugarBakery.Controllers
{
    public class NguoiDungController : Controller
    {
        dbSugarBakeryDataContext data = new dbSugarBakeryDataContext();

        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        //post: ham dang ky(post) nhap du lieu tu trang dangky
        //va thuc hien viec tao moi du lieu
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, tbKhachHang kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống*";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập*";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu*";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu*";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được bỏ trống*";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Phải nhập số điện thoại*";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (KH)
                //kh.TenKH = hoten;
                //kh.TaiKhoan = tendn;
                //kh.MatKhau = matkhau;
                //kh.Email = email;
                //kh.DiaChiKH = diachi;
                //kh.DienThoaiKH = dienthoai;
                //kh.NgaySinh = DateTime.Parse(ngaysinh);
                data.tbKhachHangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }

        //public ActionResult Dangnhap(FormCollection collection)
        //{
        //    //Gan cac gia tri nguoi dung nhap lieu cho cac bien
        //    var tendn = collection["TenDN"];
        //    var matkhau = collection["Matkhau"];
        //    if (string.IsNullOrEmpty(tendn))
        //    {
        //        ViewData["Loi1"] = "Phải nhập tên đang nhập*";
        //    }
        //    else if (string.IsNullOrEmpty(matkhau))
        //    {
        //        ViewData["Loi2"] = "Phải nhập mật khẩu*";
        //    }
        //    else
        //    {
        //        //Gán giá trị cho đối tượng được tạo mới (KH)
        //        tbKhachHang kh = data.tbKhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
        //        if (kh != null)
        //        {
        //            //ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
        //            Session["Taikhoan"] = kh;
        //            return RedirectToAction("Index", "NongSanZeno");
        //        }
        //        else
        //        {
        //            ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
        //        }
        //    }
        //    return View();
        //}
    }
}