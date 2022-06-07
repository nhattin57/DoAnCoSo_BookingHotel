using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
using PagedList;

namespace DoAnCoSo.Controllers
{
    public class QuanLyPhongController : Controller
    {
        HotelModel db = new HotelModel();
        // Đơn đặt phòng mặc định
        public ActionResult Index(int? page)
        {
            var lstDonDatPhong = db.DatPhongs.Where(n => n.DaXoa == false);
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int PageSize = 10;
            int PageNumber = (page ?? 1);

            return View(lstDonDatPhong.OrderByDescending(n=>n.MaDatPhong).ToPagedList(PageNumber,PageSize));
        }
        
        
        public ActionResult XoaDonDatPhong(int MaDatPhong)
        {
            DatPhong DonDatPhong = db.DatPhongs.Where(n => n.MaDatPhong == MaDatPhong).SingleOrDefault();
            if (DonDatPhong == null)
            {
                return HttpNotFound();
            }
            DonDatPhong.DaXoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //---------------------------------- Thêm, Sửa, Xóa Phòng-------------------------------------------------------------------------------------

        public ActionResult DanhSachPhong(int? page)
        {
            var lstPhong = db.Phongs.Where(n => n.DaXoa == false);
            if (lstPhong.Count() == 0)
            {
                return HttpNotFound();
            }
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }

            int PageSize = 10;
            int PageNumber = (page ?? 1);

            return View(lstPhong.OrderByDescending(n=>n.Don_Gia).ToPagedList(PageNumber,PageSize));
        }

        [HttpGet]
        public ActionResult ThemPhong()
        {
            ViewBag.MaLoaiPhong = new SelectList(db.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong");
            return View();
        }

        
        [HttpPost]
        public ActionResult ThemPhong(Phong phong, HttpPostedFileBase[] HinhAnh)
        {
            //kiểm tra file được up lên
            for (int i = 0; i <6; i++)
            {
                if (HinhAnh[i] == null)
                {
                    ViewBag.ThieuFile = "Thêm phòng thiếu file ảnh";
                    ViewBag.MaLoaiPhong = new SelectList(db.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong");
                    return View();
                }
                else
                {
                    if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/jpg" )
                    {
                        ViewBag.SaiDinhDangFile = "Có file không phải ảnh";
                        ViewBag.MaLoaiPhong = new SelectList(db.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong");
                        return View();
                    }
                }    
            }

            // thêm phòng khi đủ điều kiện
            if (ModelState.IsValid)
            {
                for (int i = 0; i < 6; i++)
                {
                    //lay ten hinh anh
                    var fileName = Path.GetFileName(HinhAnh[i].FileName);
                    //lấy hình ảnh chuyền vào thư mục hình ảnh
                    var path = Path.Combine(Server.MapPath("~/Content/image"), fileName);
                    //nếu thư mục đã có hình ảnh đó thì thông báo 
                    if (System.IO.File.Exists(path))
                    {
                        continue;
                    }
                    else
                    {
                        HinhAnh[i].SaveAs(path);
                    }
                }
                phong.HinhAnh1 = HinhAnh[0].FileName;
                phong.HinhAnh2 = HinhAnh[1].FileName;
                phong.HinhAnh3 = HinhAnh[2].FileName;
                phong.HinhAnh4 = HinhAnh[3].FileName;
                phong.HinhAnh5 = HinhAnh[4].FileName;
                phong.HinhAnh6 = HinhAnh[5].FileName;
                phong.DaXoa = false;
                db.Phongs.Add(phong);
                db.SaveChanges();
                return RedirectToAction("DanhSachPhong","QuanLyPhong");
            }

            ViewBag.MaLoaiPhong = new SelectList(db.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong");
            return View();
        }

        //------------------------------------------------Chinh sua-------------------------------------------
        public ActionResult ChinhSuaPhong(int? MaPhong)
        {
            if (MaPhong == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            Phong phong = db.Phongs.Where(n => n.MaPhong == MaPhong).SingleOrDefault();
            if (phong == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiPhong = new SelectList(db.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong",phong.LoaiPhong.TenLoaiPhong);

            return View(phong);
        }

        
        [HttpPost]
        public ActionResult ChinhSuaPhong(Phong phong, HttpPostedFileBase[] HinhAnh)
        {
            for (int i = 0; i < 6; i++)
            {
                    if ( HinhAnh[i]!=null && HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" &&
                         HinhAnh[i].ContentType != "image/jpg")
                    {
                        ViewBag.SaiDinhDangFile = "Có file không phải ảnh";
                        Phong PhongReturn = db.Phongs.Where(n => n.MaPhong == phong.MaPhong).Single();
                        ViewBag.MaLoaiPhong = new SelectList(db.LoaiPhongs, "MaLoaiPhong", "TenLoaiPhong",PhongReturn.LoaiPhong.TenLoaiPhong);
                        return View(PhongReturn);
                    }
            }

            if (HinhAnh[0] != null)
            {
                //lay ten hinh anh
                var fileName = Path.GetFileName(HinhAnh[0].FileName);
                //lấy hình ảnh chuyền vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/image"), fileName);
                //nếu thư mục đã có hình ảnh đó thì thông báo
                if (System.IO.File.Exists(path))
                {
                    phong.HinhAnh1 = fileName;
                }
                else
                {
                    HinhAnh[0].SaveAs(path);
                    phong.HinhAnh1 = fileName;
                }

            }

            if(HinhAnh[1] != null)
            {
                var fileName = Path.GetFileName(HinhAnh[1].FileName);
                var path = Path.Combine(Server.MapPath("~/Content/image"), fileName);
                if (System.IO.File.Exists(path))
                {
                    phong.HinhAnh2 = fileName;
                }
                else
                {
                    HinhAnh[1].SaveAs(path);
                    phong.HinhAnh2 = fileName;
                }
            }
            if (HinhAnh[2] != null)
            {
                var fileName = Path.GetFileName(HinhAnh[2].FileName);
                var path = Path.Combine(Server.MapPath("~/Content/image"), fileName);
                if (System.IO.File.Exists(path))
                {
                    phong.HinhAnh3 = fileName;
                }
                else
                {
                    HinhAnh[2].SaveAs(path);
                    phong.HinhAnh3 = fileName;
                }
            }
            if (HinhAnh[3] != null)
            {
                var fileName = Path.GetFileName(HinhAnh[3].FileName);
                var path = Path.Combine(Server.MapPath("~/Content/image"), fileName);
                if (System.IO.File.Exists(path))
                {
                    phong.HinhAnh4 = fileName;
                }
                else
                {
                    HinhAnh[3].SaveAs(path);
                    phong.HinhAnh4 = fileName;
                }
            }
            if (HinhAnh[4] != null)
            {
                var fileName = Path.GetFileName(HinhAnh[4].FileName);
                var path = Path.Combine(Server.MapPath("~/Content/image"), fileName);
                if (System.IO.File.Exists(path))
                {
                    phong.HinhAnh5 = fileName;
                }
                else
                {
                    HinhAnh[4].SaveAs(path);
                    phong.HinhAnh5 = fileName;
                }
            }
            if (HinhAnh[5] != null)
            {
                var fileName = Path.GetFileName(HinhAnh[5].FileName);
                var path = Path.Combine(Server.MapPath("~/Content/image"), fileName);
                if (System.IO.File.Exists(path))
                {
                    phong.HinhAnh6 = fileName;
                }
                else
                {
                    HinhAnh[5].SaveAs(path);
                    phong.HinhAnh6 = fileName;
                }
            }
            db.Entry(phong).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("DanhSachPhong", "QuanLyPhong");
        }

        [HttpGet]
        public ActionResult XoaPhong(int? MaPhong)
        {
            if (MaPhong == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Phong phong = db.Phongs.Where(n => n.MaPhong == MaPhong).SingleOrDefault();
            if (phong == null)
            {
                return HttpNotFound();
            }
            phong.DaXoa = true;
            db.SaveChanges();
            return RedirectToAction("DanhSachPhong", "QuanLyPhong");
        }
    }
}