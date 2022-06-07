using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
namespace DoAnCoSo.Controllers
{
    public class ThongKeController : Controller
    {
        HotelModel db = new HotelModel();
        // GET: ThongKe
        public ActionResult ThongKe()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//lấy số lượng người truy cập từ 
            ViewBag.SoNguoiDangOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//lấy số lượng người online
            ViewBag.TongDoanhThu = TinhTongDoanhThu();
            ViewBag.TongDonDatPhong = TinhTongSoDonDatPhong();
            

            return View();
        }

        public decimal TinhTongDoanhThu()
        {
            decimal tong = 0;
            foreach (var item in db.DatPhongs.Where(n=>n.DaXoa==false))
            {
                tong += item.Phong.Don_Gia.Value;
            }
            return tong;
        }

        public int TinhTongSoDonDatPhong()
        {
            return db.DatPhongs.Count(n=>n.DaXoa==false);
        }


    }
}