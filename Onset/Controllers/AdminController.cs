using Onset.Models;
using Onset.Models.AdminModel;
using Onset.Models.FormModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Onset.Controllers
{
    public class AdminController : Controller
    {
        ONSETDBEntities db = new ONSETDBEntities();

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["AdminEmail"] != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginForm loginForm)
        {
            if (ModelState.IsValid)
            {
                var checkUser = db.ADMINS.Where(u => u.ADMIN_EMAIL == loginForm.EMAIL).FirstOrDefault();

                if (checkUser == null)
                {
                    ViewBag.Error = "User Don't Exists.";
                    return View();
                }

                if (checkUser.ADMIN_PASSWORD != loginForm.PASSWORD)
                {
                    ViewBag.Error = "Wrong Password.";
                    return View();
                }

                Session["AdminId"] = checkUser.ADMIN_ID;
                Session["AdminEmail"] = loginForm.EMAIL;

                return RedirectToAction("Index");
            }
            ViewBag.Error = "Error in Database.";
            return View();
        }

        public ActionResult Logout()
        {
            Session["AdminEmail"] = null;
            Session["AdminId"] = null;

            return RedirectToAction("Login");
        }

        public ActionResult Users ()
        {
            if (Session["AdminEmail"] == null)
            {
                return RedirectToAction("Login");
            }

            var emplist = db.EMPLOYEES.ToList();
            var manlist = db.MANAGERS.ToList();

            int userid = Int32.Parse(Session["AdminId"].ToString());
            UsersModel view = new UsersModel();
            view.NAME = db.ADMINS.Where(u => u.ADMIN_ID == userid).FirstOrDefault().ADMIN_NAME;
            view.EMPLIST = emplist;
            view.MANLIST = manlist;

            return View(view);
        }
        
        public ActionResult ViewManager (int? id)
        {
            if (Session["AdminEmail"] == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var man = db.MANAGERS.Where(u => u.MANAGER_ID == id.Value).FirstOrDefault();
            int userid = Int32.Parse(Session["AdminId"].ToString());

            var adminname = db.ADMINS.Where(u => u.ADMIN_ID == userid).FirstOrDefault().ADMIN_NAME;

            ViewProfileModel view = new ViewProfileModel();
            view.NAME = adminname;
            view.MAN = man;

            return View(view);
        }

        public ActionResult ViewEmployee(int? id)
        {
            if (Session["AdminEmail"] == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var man = db.EMPLOYEES.Where(u => u.EMPLOYEE_ID == id.Value).FirstOrDefault();
            int userid = Int32.Parse(Session["AdminId"].ToString());

            var adminname = db.ADMINS.Where(u => u.ADMIN_ID == userid).FirstOrDefault().ADMIN_NAME;

            ViewProfileModel view = new ViewProfileModel();
            view.NAME = adminname;
            view.EMP = man;

            return View(view);
        }

        public ActionResult Reports ()
        {
            if (Session["AdminEmail"] == null)
            {
                return RedirectToAction("Login");
            }

            int userid = Int32.Parse(Session["AdminId"].ToString());
            var adminname = db.ADMINS.Where(u=>u.ADMIN_ID == userid).FirstOrDefault().ADMIN_NAME;

            var reports = db.REPORTUSERS.ToList();

            ReportsModel view = new ReportsModel();
            view.FROM = new List<string>();
            view.TO = new List<string>();
            view.ID = new List<int>();
            view.NAME = adminname;

            foreach (var e in reports)
            {
                int fromid = e.USER_ID_BY;
                int toid = e.USER_ID_FOR;

                var fromuser = db.USERS.Where(u => u.USER_ID == fromid).FirstOrDefault();
                var touser = db.USERS.Where(u => u.USER_ID == toid).FirstOrDefault();

                if (fromuser.USER_TYPE == "Manager")
                {
                    var temp = db.MANAGERS.Where(u => u.MANAGER_EMAIL == fromuser.USER_EMAIL).FirstOrDefault();
                    var name = temp.MANAGER_FIRSTNAME + " " + temp.MANAGER_LASTNAME;

                    view.FROM.Add(name);
                } else
                {
                    var temp = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == fromuser.USER_EMAIL).FirstOrDefault();
                    var name = temp.EMPLOYEE_FIRSTNAME + " " + temp.EMPLOYEE_LASTNAME;

                    view.FROM.Add(name);
                }

                if (touser.USER_TYPE == "Manager")
                {
                    var temp = db.MANAGERS.Where(u => u.MANAGER_EMAIL == touser.USER_EMAIL).FirstOrDefault();
                    var name = temp.MANAGER_FIRSTNAME + " " + temp.MANAGER_LASTNAME;

                    view.TO.Add(name);
                }
                else
                {
                    var temp = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == touser.USER_EMAIL).FirstOrDefault();
                    var name = temp.EMPLOYEE_FIRSTNAME + " " + temp.EMPLOYEE_LASTNAME;

                    view.TO.Add(name);
                }

                view.ID.Add(e.REPORT_ID);
            }

            return View(view);
        }

        public ActionResult ViewReport (int? id)
        {
            if (Session["AdminEmail"] == null)
            {
                return RedirectToAction("Login");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            ViewReportModel view = new ViewReportModel();

            int userid = Int32.Parse(Session["AdminId"].ToString());
            var adminname = db.ADMINS.Where(u => u.ADMIN_ID == userid).FirstOrDefault().ADMIN_NAME;

            view.NAME = adminname;

            var rpt = db.REPORTS.Where(u => u.REPORT_ID == id.Value).FirstOrDefault();
            view.RPT = rpt;

            var e = db.REPORTUSERS.Where(u => u.REPORT_ID == id.Value).FirstOrDefault();

            int fromid = e.USER_ID_BY;
            int toid = e.USER_ID_FOR;

            var fromuser = db.USERS.Where(u => u.USER_ID == fromid).FirstOrDefault();
            var touser = db.USERS.Where(u => u.USER_ID == toid).FirstOrDefault();

            if (fromuser.USER_TYPE == "Manager")
            {
                var temp = db.MANAGERS.Where(u => u.MANAGER_EMAIL == fromuser.USER_EMAIL).FirstOrDefault();
                var name = temp.MANAGER_FIRSTNAME + " " + temp.MANAGER_LASTNAME;

                view.FROMNAME = name;
            }
            else
            {
                var temp = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == fromuser.USER_EMAIL).FirstOrDefault();
                var name = temp.EMPLOYEE_FIRSTNAME + " " + temp.EMPLOYEE_LASTNAME;

                view.FROMNAME = name;
            }

            view.FROMEMAIL = fromuser.USER_EMAIL;

            if (touser.USER_TYPE == "Manager")
            {
                var temp = db.MANAGERS.Where(u => u.MANAGER_EMAIL == touser.USER_EMAIL).FirstOrDefault();
                var name = temp.MANAGER_FIRSTNAME + " " + temp.MANAGER_LASTNAME;

                view.TONAME = name;
            }
            else
            {
                var temp = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == touser.USER_EMAIL).FirstOrDefault();
                var name = temp.EMPLOYEE_FIRSTNAME + " " + temp.EMPLOYEE_LASTNAME;

                view.TONAME = name;
            }

            view.TOEMAIL = touser.USER_EMAIL;

            return View(view);
        }

        public ActionResult Index()
        {
            if (Session["AdminEmail"] == null)
            {
                return RedirectToAction("Login");
            }

            int adminid = Int32.Parse(Session["AdminId"].ToString());
            var adminname = db.ADMINS.Where(u => u.ADMIN_ID == adminid).FirstOrDefault().ADMIN_NAME;
            var usersize = db.USERS.ToList().Count;
            var empsize = db.EMPLOYEES.ToList().Count;
            var mansize = db.MANAGERS.ToList().Count;
            var tasksize = db.TASKS.ToList().Count;
            var reportsize = db.REPORTS.ToList().Count;

            IndexModel view = new IndexModel();
            view.NAME = adminname;
            view.USERSIZE = usersize;
            view.EMPSIZE = empsize;
            view.MANSIZE = mansize;
            view.TASKSIZE = tasksize;
            view.REPORTSIZE = reportsize;

            return View(view);
        }
    }
}