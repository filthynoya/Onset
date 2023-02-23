using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Onset.Models;
using Onset.Models.FormModel;
using Onset.Controllers;

namespace Onset.Controllers
{
    public class AuthController : Controller
    {
        ONSETDBEntities db = new ONSETDBEntities();

        public ActionResult Logout ()
        {
            Session["UserId"] = null;
            Session["Email"] = null;
            Session["UserType"] = null;

            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Registration ()
        {
            if (Session["Email"] != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationForm registrationForm)
        {
            if (ModelState.IsValid)
            {
                var check_email = db.USERS.Where(u => u.USER_EMAIL == registrationForm.EMAIL).FirstOrDefault();

                if (check_email != null)
                {
                    ViewBag.Error = "Email Exists.";
                    return View();
                }

                double ok;

                if (double.TryParse(registrationForm.PHONE, out ok) == false)
                {
                    ViewBag.Error = "Incorrent Phone Number.";
                    return View();
                }

                if (registrationForm.USERTYPE == "Employee")
                {
                    db.EMPLOYEES.Add(new EMPLOYEE() {
                        EMPLOYEE_FIRSTNAME = registrationForm.FIRSTNAME,
                        EMPLOYEE_LASTNAME = registrationForm.LASTNAME,
                        EMPLOYEE_EMAIL = registrationForm.EMAIL,
                        EMPLOYEE_DATEOFBIRTH = registrationForm.DATEOFBIRTH,
                        EMPLOYEE_DESIGNATION = registrationForm.DESIGNATION,
                        EMPLOYEE_GENDER = registrationForm.GENDER,
                        EMPLOYEE_LOCATION = registrationForm.LOCATION,
                        EMPLOYEE_ORGANIZATION = registrationForm.ORGANIZATION,
                        EMPLOYEE_PHONE = registrationForm.PHONE,
                        EMPLOYEE_REGISTRATIONDATE = System.DateTime.Today
                    });
                } else
                {
                    db.MANAGERS.Add(new MANAGER()
                    {
                        MANAGER_FIRSTNAME = registrationForm.FIRSTNAME,
                        MANAGER_LASTNAME = registrationForm.LASTNAME,
                        MANAGER_EMAIL = registrationForm.EMAIL,
                        MANAGER_DATEOFBIRTH = registrationForm.DATEOFBIRTH,
                        MANAGER_DESIGNATION = registrationForm.DESIGNATION,
                        MANAGER_GENDER = registrationForm.GENDER,
                        MANAGER_LOCATION = registrationForm.LOCATION,
                        MANAGER_ORGANIZATION = registrationForm.ORGANIZATION,
                        MANAGER_PHONE = registrationForm.PHONE,
                        MANAGER_REGISTRATIONDATE = System.DateTime.Today
                    });
                }

                db.USERS.Add(new USER() {
                    USER_EMAIL = registrationForm.EMAIL,
                    USER_PASSWORD = registrationForm.PASSWORD,
                    USER_TYPE = registrationForm.USERTYPE
                });

                db.SaveChanges();

                return RedirectToAction("Login");
            }

            ViewBag.Error = "Error in Database.";
            return View();
        }

        [HttpGet]
        public ActionResult Login ()
        {
            if (Session["Email"] != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login (LoginForm loginForm)
        {
            if (ModelState.IsValid)
            {
                var checkUser = db.USERS.Where(u => u.USER_EMAIL == loginForm.EMAIL).FirstOrDefault();

                if (checkUser == null)
                {
                    ViewBag.Error = "User Don't Exists.";
                    return View();
                }

                if (checkUser.USER_PASSWORD != loginForm.PASSWORD)
                {
                    ViewBag.Error = "Wrong Password.";
                    return View();
                }

                Session["UserId"] = checkUser.USER_ID;
                Session["Email"] = loginForm.EMAIL;
                Session["UserType"] = checkUser.USER_TYPE;

                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Error = "Error in Database.";
            return View();
        }
    }
}