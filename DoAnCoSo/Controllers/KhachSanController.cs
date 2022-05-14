using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
namespace DoAnCoSo.Controllers
{
    public class KhachSanController : Controller
    {
        HotelModel db = new HotelModel();
        // GET: KhachSan
        public ActionResult ChitietPhong(int MaPhong)
        {
            Phong phong = db.Phongs.Where(n => n.MaPhong == MaPhong && n.DaXoa == false).Single();
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }
    }
}