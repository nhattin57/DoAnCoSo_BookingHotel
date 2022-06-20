using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
using DoAnCoSo.Models.vn_pay;
using System.Text.RegularExpressions;
namespace DoAnCoSo.Controllers
{
    public class DatPhongController : Controller
    {
        public static DatPhong DonDatPhong_GloBal;
        public static KhachHang KhachHang_GloBal;
        public static ChiTietDatPhong ChiTietDatPhong_GloBal;

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
            
            var phongDaDuotDat = (from a in db.DatPhongs
                        orderby a.MaPhong
                        where a.MaPhong == phong.MaPhong
                        select a).Where((n=> n.NgayDat <= NgayDat && n.NgayTra >= NgayDat))
                        .ToList();

            var phongDaDuotDat1 = (from a in db.DatPhongs
                                  orderby a.MaPhong
                                  where a.MaPhong == phong.MaPhong
                                  select a).Where((n => n.NgayDat < NgayTra && n.NgayTra >= NgayTra))
                        .ToList();

            if (phongDaDuotDat.Count()!=0 || phongDaDuotDat1.Count()!=0)
            {

                Phong phongrt = db.Phongs.Where(n => n.MaPhong == phong.MaPhong).SingleOrDefault();

                ViewBag.LoiDatLich = "Từ ngày "+NgayDat +" đến ngày"+ NgayTra +" đã có người đặt trước";
                return View(phongrt);
            }

            int soNgayDat = NgayTra.Subtract(NgayDat).Days;
            if (soNgayDat == 0)
            {
                soNgayDat = 1;
            }
            //Tao moi khach hang va luu csdl de lay ma khach hang
            KhachHang kh = new KhachHang();
            kh.HoTen = HoTen;
            kh.Email = email;
            kh.SDT = SDT;
            KhachHang_GloBal = kh;
            //db.KhachHangs.Add(kh);
            // db.SaveChanges();
            //Lấy ra thông tin khách hàng mới vừa tạo
            
            // Tạo Đơn đặt phòng
            DatPhong DonDatPhong = new DatPhong();
            DonDatPhong.MaPhong = phong.MaPhong;
            //DonDatPhong.MaKH = kh.MaKH;
            DonDatPhong.NgayDat = NgayDat;
            DonDatPhong.NgayTra = NgayTra;
            DonDatPhong.GhiChu = GhiChu;
           
            DonDatPhong.DaXoa = false;
            DonDatPhong_GloBal = DonDatPhong;
            //db.DatPhongs.Add(DonDatPhong);
            //db.SaveChanges();

            decimal soTienThanhToanOnline= DonGiaDat * soNgayDat * 50 / 100;
            //Tạo chi tiết
            ChiTietDatPhong chiTietDatPhong = new ChiTietDatPhong();
            // chiTietDatPhong.MaKH = DonDatPhong.MaKH.Value;
            // chiTietDatPhong.MaDatPhong = DonDatPhong.MaDatPhong;
            chiTietDatPhong.DonGia_Dat = soTienThanhToanOnline;
            ChiTietDatPhong_GloBal = chiTietDatPhong;
           // db.ChiTietDatPhongs.Add(chiTietDatPhong);
           // db.SaveChanges();
            //ViewBag.ThanhCong = "Bạn đã đặt phòng thành công";
            ViewBag.LoiDatLich = "";
           

            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            VnPayLibrary pay = new VnPayLibrary();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0 or 2.0.1
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount",(soTienThanhToanOnline*100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn
            
            
            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);

            /* Phong phongss = db.Phongs.Where(n => n.MaPhong == phong.MaPhong).SingleOrDefault();
             return View(phongss);*/
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

        public ActionResult Payment()
        {
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", "1000000"); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                VnPayLibrary pay = new VnPayLibrary();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        db.KhachHangs.Add(KhachHang_GloBal);
                        db.SaveChanges();

                        DonDatPhong_GloBal.MaKH = KhachHang_GloBal.MaKH;
                        db.DatPhongs.Add(DonDatPhong_GloBal);
                        db.SaveChanges();

                        ChiTietDatPhong_GloBal.MaKH = KhachHang_GloBal.MaKH;
                        ChiTietDatPhong_GloBal.MaDatPhong = DonDatPhong_GloBal.MaDatPhong;
                        db.ChiTietDatPhongs.Add(ChiTietDatPhong_GloBal);
                        db.SaveChanges();
                        GuiEmail("Thư cảm ơn bạn đã đặt phòng tại Royal-Hotel", KhachHang_GloBal.Email, "daonhattin12@gmail.com", "sdwittwgafezbkfx",
               "Đơn đặt phòng của bạn với mã đơn đặt phòng:" + DonDatPhong_GloBal.MaDatPhong + "đã được Royal-hotel xác nhận," +
               " Royal-hotel xin chân thành cảm ơn bạn!");
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
    }


}