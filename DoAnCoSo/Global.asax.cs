using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoAnCoSo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Application["SoNguoiTruyCap"] = 0;
            Application["SoNguoiDangOnline"] = 0;
        }

    protected void Session_Start()
    {
        Application.Lock(); // Dùng để đong bộ hóa
        Application["SoNguoiTruyCap"] = (int)Application["SoNguoiTruyCap"] + 1;
        Application["SoNguoiDangOnline"] = (int)Application["SoNguoiDangOnline"] + 1;
        //Application["Online"]=(int) Application["Online"] + 1;
       Application.UnLock();
    }

    protected void Session_End()
    {
        Application.Lock(); // Dùng để đong bộ hóa
        Application["SoNguoiDangOnline"] = (int)Application["SoNguoiDangOnline"] - 1;

        Application.UnLock();
    }
}
}
