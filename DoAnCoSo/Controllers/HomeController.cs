using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using PagedList;
using DoAnCoSo.Models;

namespace DoAnCoSo.Controllers
{
    public class HomeController : Controller
    {
        HotelModel db = new HotelModel();
        // GET: Home
        public ActionResult Index(int? page)
        {
            ViewBag.loaiphong = new SelectList(db.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong");
            var listPhong = db.Phongs.Where(n=>n.DaXoa==false);
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }

            int PageSize = 4;
            //tao bien so trang hien tai cua trang web
            int PageNumber = (page ?? 1);

            return View(listPhong.OrderBy(n => n.MaPhong).ToPagedList(PageNumber, PageSize));
        }
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact( PhanHoiTuKH phanhoi)
        {
            db.PhanHoiTuKHs.Add(phanhoi);
            db.SaveChanges();
            ViewBag.PhanHoiThanhCong = "Ý kiến của bạn đã được gửi đi";
            return View();
        }

        [HttpGet]
        public ActionResult Policy()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string EmailDangNhap = f["username"].ToString();
            string matKhau = f["password"].ToString();

            ThanhVien thanhVien = db.ThanhViens.Where(n => n.Email == EmailDangNhap && n.MatKhau == matKhau).SingleOrDefault();
            if(thanhVien == null)
            {
                ViewBag.DangNhapKhongThanhCong = "Sai tài khoản hoặc mật khẩu";
                return View();
            }
            return RedirectToAction("Index","QuanLyPhong");
        }

    }
}