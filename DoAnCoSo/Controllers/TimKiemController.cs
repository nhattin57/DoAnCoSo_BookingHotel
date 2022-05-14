using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
namespace DoAnCoSo.Controllers
{
    public class TimKiemController : Controller
    {
        HotelModel db = new HotelModel();

        [HttpPost]
        public ActionResult TimPhong( FormCollection f)
        {
            if(f["SoNguoiLon"]==null || f["SoTreEm"]==null)
            {
                int MaLoaiPhongs = int.Parse(f["loaiphong"].ToString());
                var lstPhongs = db.Phongs.Where(n => n.MaLoaiPhong == MaLoaiPhongs);
                Phong phongs = lstPhongs.First();
                return RedirectToAction("ChitietPhong", "KhachSan", new { @MaPhong = phongs.MaPhong });
            }

            //int SoNguoiLon = int.Parse(f["SoNguoiLon"].ToString());
            //int SoTreEm = int.Parse(f["SoTreEm"].ToString());
            int MaLoaiPhong = int.Parse(f["loaiphong"].ToString());

            var lstPhong = db.Phongs.Where(n => n.MaLoaiPhong == MaLoaiPhong);
            if (lstPhong.Count() == 0)
            {
                RedirectToAction("Index", "Home");
            }
            Phong phong = lstPhong.First();
            return RedirectToAction("ChitietPhong", "KhachSan", new { @MaPhong = phong.MaPhong });
        }
    }
}