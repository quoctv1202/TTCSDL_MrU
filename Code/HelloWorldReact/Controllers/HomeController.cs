using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IS.uni;
using IS.Base;
using IS.authen;
using IS.Sess;

namespace HelloWorldReact.Controllers
{
    public class HomeController : Controller
    {
        session ses = new session();
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult dologin()
        {
            //Kiểm tra mật khâu cơ sở dữ liệu ở đây
            STAFF_INFO staff = new STAFF_INFO();
            staff.CODE = "ABC";
            staff.NAME="Đã đăng nhập";
            staff.LOGTIME = DateTime.Now;
            ses.login(staff);

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Nation()
        {
            NATION_BUS bus = new NATION_BUS();
            List<NATION_OBJ> li = bus.getAll(); 
            return View(li);
        }
        [CustomAuthentication]
        public ActionResult Nationreact()
        {
            return View();
        }

        public JsonResult GetName()
        {
            return Json(new { name = "World from server side" }, JsonRequestBehavior.AllowGet);
        }
        
	}
}