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
        public ActionResult TimPhong(FormCollection f)
        {

            int MaLoaiPhong = int.Parse(f["loaiphong"].ToString());
            var phongChuaBaoGioDuotDat = db.Phongs.Where(n => !n.DatPhongs.Any(x => x.MaPhong == n.MaPhong && n.MaLoaiPhong == MaLoaiPhong)
                                 && n.MaLoaiPhong == MaLoaiPhong);

            if (phongChuaBaoGioDuotDat.Count() != 0)
            {
                int ma = phongChuaBaoGioDuotDat.First().MaPhong;
                Phong PhongTraVe = db.Phongs.Where(n => n.MaPhong == ma).Single();
                return RedirectToAction("ChitietPhong", "KhachSan", new { @MaPhong = PhongTraVe.MaPhong });
            }

            if (f["SoNguoiLon"] == null && f["SoTreEm"] == null && f["ngaynhan"] == "" && f["ngaytra"] == "")
            {
                var lstPhongs = db.Phongs.Where(n => n.MaLoaiPhong == MaLoaiPhong);
                Phong phongs = lstPhongs.First();
                return RedirectToAction("ChitietPhong", "KhachSan", new { @MaPhong = phongs.MaPhong });
            }

            DateTime NgayNhan = DateTime.Parse(f["ngaynhan"]);
            DateTime NgayTra = DateTime.Parse(f["ngaytra"]);

            if (NgayTra < NgayNhan)
            {
                return Content("<script language='javascript' type='text/javascript'>" +
                    "alert('Ngày giờ trả phòng phải muộn hơn ngày nhận phòng');" +
                    "</script>");
            }

           
                var query = from a in db.Phongs
                            from b in db.DatPhongs
                            where b.NgayTra < NgayNhan && b.NgayDat < NgayTra && a.MaPhong == b.MaPhong && a.MaLoaiPhong == MaLoaiPhong
                            select a.MaPhong;
                int soPhongTimDuoc = query.Count();
                if (soPhongTimDuoc == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                int maphong = query.FirstOrDefault();


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
