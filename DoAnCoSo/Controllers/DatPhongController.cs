using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
namespace DoAnCoSo.Controllers
{
    public class DatPhongController : Controller
    {
        HotelModel db = new HotelModel();

        [HttpGet]
        public ActionResult DatPhong(int MaPhong)
        {
            Phong phong = db.Phongs.Where(n => n.MaPhong == MaPhong).SingleOrDefault();
            if (phong == null)
            {
                return HttpNotFound();
            }

            return View(phong);
        }

        [HttpPost]
        public ActionResult DatPhong(Phong phong,FormCollection f)
        {
            DateTime NgayDat = DateTime.Parse(f["NgayDat"]);
            DateTime NgayTra = DateTime.Parse(f["NgayTra"]);
            string HoTen = f["full_name"].ToString();
            string email = f["email"].ToString();
            string SDT = f["phone"].ToString();
            string GhiChu = f["GhiChu"];
            decimal DonGiaDat = phong.Don_Gia.Value;

            if (NgayTra < NgayDat)
            {
                Phong phongs = db.Phongs.Where(n => n.MaPhong == phong.MaPhong).SingleOrDefault();
                ViewBag.ThanhCong = "";
                ViewBag.LoiDatLich = "Ngày giờ trả phòng phải muộn hơn ngày đặt phòng";
                return View(phongs);
            }
            //Tao moi khach hang va luu csdl de lay ma khach hang
            KhachHang kh = new KhachHang();
            kh.HoTen = HoTen;
            kh.Email = email;
            kh.SDT = SDT;
            db.KhachHangs.Add(kh);
            db.SaveChanges();
            //Lấy ra thông tin khách hàng mới vừa tạo
            
            // Tạo Đơn đặt phòng
            DatPhong DonDatPhong = new DatPhong();
            DonDatPhong.MaPhong = phong.MaPhong;
            DonDatPhong.MaKH = kh.MaKH;
            DonDatPhong.NgayDat = NgayDat;
            DonDatPhong.NgayTra = NgayTra;
            DonDatPhong.DaXoa = false;
            db.DatPhongs.Add(DonDatPhong);
            db.SaveChanges();


            //Tạo chi tiết
            ChiTietDatPhong chiTietDatPhong = new ChiTietDatPhong();
            chiTietDatPhong.MaKH = DonDatPhong.MaKH.Value;
            chiTietDatPhong.MaDatPhong = DonDatPhong.MaDatPhong;
            chiTietDatPhong.DonGia_Dat = DonGiaDat;
            if (GhiChu != "")
            {
                chiTietDatPhong.GhiChu = GhiChu;
            }
            db.ChiTietDatPhongs.Add(chiTietDatPhong);
            db.SaveChanges();
            ViewBag.ThanhCong = "Bạn đã đặt phòng thành công";
            ViewBag.LoiDatLich = "";
            GuiEmail("Thư cảm ơn bạn đã đặt phòng tại Royal-Hotel", kh.Email, "daonhattin12@gmail.com", "nhattin12",
                "Đơn đặt phòng của bạn đã được Royal-hotel xác nhận, Royal-hotel xin chân thành cảm ơn bạn!");
            Phong phongss = db.Phongs.Where(n => n.MaPhong == phong.MaPhong).SingleOrDefault();
            return View(phongss);
        }

        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            // goi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chi nhận
            mail.From = new MailAddress(ToEmail); // Địa chửi gừi
            mail.Subject = Title; // tiêu đề gửi
            mail.Body = Content; // Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587;  // port của Gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);//Tài khoản password người gửi
            smtp.EnableSsl = true; //kích hoạt giao tiếp an toàn SSL
            smtp.Send(mail); //Gửi mail đi

        }
    }


}