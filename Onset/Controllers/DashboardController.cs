using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Onset.Models;
using Onset.Models.ProfileModel;
using Onset.Models.DashboardModel;

namespace Onset.Controllers
{
    public class DashboardController : Controller
    {
        ONSETDBEntities db = new ONSETDBEntities();

        public ActionResult Index()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string email = Session["Email"].ToString();
            string usertype = Session["UserType"].ToString();

            DashboardModel dashInfo = generateDashboardInfo();

            IndexModel info = new IndexModel();
            info.NAME = dashInfo.NAME;
            info.USERTYPE = dashInfo.USERTYPE;
            info.PROFILEPIC = dashInfo.PROFILEPIC;

            info.DASHINFO = dashInfo;

            if (usertype == "Manager")
            {
                var check = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault();
                int id = check.MANAGER_ID;

                var tasklist = db.TASKMANAGERS.Where(u => u.MANAGER_ID == id).ToList();

                var taskinfo = new List<TASK>();
                var empinfo = new List<EMPLOYEE>();
                var propics = new List<String>();
                var perc = new List<float>();
                var userids = new List<int>();

                foreach (var e in tasklist)
                {
                    int taskid = e.TASK_ID;

                    var taskok = db.TASKS.Where(u => u.TASK_ID == taskid).FirstOrDefault();
                    if (taskok.TASK_DEADLINE.Year < System.DateTime.Now.Year) continue;
                    if (taskok.TASK_DEADLINE.Month < System.DateTime.Now.Month) continue;
                    if (taskok.TASK_DEADLINE.Day < System.DateTime.Now.Day && taskok.TASK_DEADLINE.Month == System.DateTime.Now.Month) continue;

                    taskinfo.Add(taskok);

                    var ep = db.TASKEMPLOYEES.Where(u => u.TASK_ID == taskid).FirstOrDefault();
                    var temp_emp = db.EMPLOYEES.Where(u => u.EMPLOYEE_ID == ep.EMPLOYEE_ID).FirstOrDefault();

                    empinfo.Add(temp_emp);

                    var temp_email = temp_emp.EMPLOYEE_EMAIL;

                    var temp_user = db.USERS.Where(u => u.USER_EMAIL == temp_email).FirstOrDefault();

                    userids.Add(temp_user.USER_ID);

                    var temp_pic = db.USERPICS.Where(u => u.USER_ID == temp_user.USER_ID).FirstOrDefault();

                    if (temp_pic != null)
                    {
                        propics.Add("/Upload/ProfilePicture/" + temp_pic.USER_PIC);
                    } else
                    {
                        propics.Add("https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava2-bg.webp");
                    }

                    var progresses = db.TASKPROGRESSES.Where(u => u.TASK_ID == taskid).ToList();
                    float percent = 0f;

                    if (progresses != null)
                    {
                        foreach (var f in progresses)
                        {
                            int proid = f.PROGRESS_ID;
                            var chk = db.PROGRESSES.Where(u => u.PROGRESS_ID == proid).FirstOrDefault();

                            percent += chk.PROGRESS_PERCENTAGE;
                        }
                    }

                    perc.Add(percent);
                }

                info.TASKS = taskinfo;
                info.EMPS = empinfo;
                info.PROPICS = propics;
                info.PERCENTS = perc;
                info.USERIDS = userids;
            } else
            {
                var check = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault();
                int id = check.EMPLOYEE_ID;

                var tasklist = db.TASKMANAGERS.Where(u => u.MANAGER_ID == id).ToList();

                var taskinfo = new List<TASK>();
                var maninfo = new List<MANAGER>();
                var propics = new List<String>();
                var perc = new List<float>();
                var userids = new List<int>();

                foreach (var e in tasklist)
                {
                    int taskid = e.TASK_ID;

                    var taskok = db.TASKS.Where(u => u.TASK_ID == taskid).FirstOrDefault();
                    if (taskok.TASK_DEADLINE.Year < System.DateTime.Now.Year) continue;
                    if (taskok.TASK_DEADLINE.Month < System.DateTime.Now.Month) continue;
                    if (taskok.TASK_DEADLINE.Day < System.DateTime.Now.Day && taskok.TASK_DEADLINE.Month == System.DateTime.Now.Month) continue;

                    taskinfo.Add(taskok);

                    var ep = db.TASKMANAGERS.Where(u => u.TASK_ID == taskid).FirstOrDefault();
                    var temp_emp = db.MANAGERS.Where(u => u.MANAGER_ID == ep.MANAGER_ID).FirstOrDefault();

                    maninfo.Add(temp_emp);

                    var temp_email = temp_emp.MANAGER_EMAIL;

                    var temp_user = db.USERS.Where(u => u.USER_EMAIL == temp_email).FirstOrDefault();

                    userids.Add(temp_user.USER_ID);

                    var temp_pic = db.USERPICS.Where(u => u.USER_ID == temp_user.USER_ID).FirstOrDefault();

                    if (temp_pic != null)
                    {
                        propics.Add("/Upload/ProfilePicture/" + temp_pic.USER_PIC);
                    }
                    else
                    {
                        propics.Add("https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava2-bg.webp");
                    }

                    var progresses = db.TASKPROGRESSES.Where(u => u.TASK_ID == taskid).ToList();
                    float percent = 0f;

                    if (progresses != null)
                    {
                        foreach (var f in progresses)
                        {
                            int proid = f.PROGRESS_ID;
                            var chk = db.PROGRESSES.Where(u => u.PROGRESS_ID == proid).FirstOrDefault();

                            percent += chk.PROGRESS_PERCENTAGE;
                        }
                    }

                    perc.Add(percent);
                }

                info.TASKS = taskinfo;
                info.MANS = maninfo;
                info.PROPICS = propics;
                info.PERCENTS = perc;
                info.USERIDS = userids;
            }

            return View(info);
        }

        [HttpGet]
        public ActionResult AddTask ()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["UserType"].ToString() != "Manager")
            {
                return RedirectToAction("Index");
            }

            AddTaskModel list = new AddTaskModel();
            list.CATEGORIES = new List<SelectListItem>();

            list.CATEGORIES.Add(new SelectListItem() { Text = "UI Design", Value = "UI Design", Selected = true });
            list.CATEGORIES.Add(new SelectListItem() { Text = "Bug Fix", Value = "Bug Fix", Selected = false });
            list.CATEGORIES.Add(new SelectListItem() { Text = "Add Feature", Value = "Add Feature", Selected = false });

            var email = Session["Email"].ToString();

            var org = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault().MANAGER_ORGANIZATION;

            List<EMPLOYEE> le = db.EMPLOYEES.Where(u => u.EMPLOYEE_ORGANIZATION == org).ToList();

            list.EMPLOYEES = new List<SelectListItem>();
            bool isf = true;

            foreach (EMPLOYEE e in le)
            {
                var nam = e.EMPLOYEE_FIRSTNAME + " " + e.EMPLOYEE_LASTNAME;
                var id = e.EMPLOYEE_ID;

                if (isf)
                {
                    list.EMPLOYEES.Add(new SelectListItem() { Text = nam, Value = id.ToString(), Selected = true });
                } else
                {
                    list.EMPLOYEES.Add(new SelectListItem() { Text = nam, Value = id.ToString(), Selected = false });
                }
            }

            string usertype = Session["UserType"].ToString();
            DashboardModel dashInfo = generateDashboardInfo();

            list.NAME = dashInfo.NAME;
            list.USERTYPE = dashInfo.USERTYPE;
            list.PROFILEPIC = dashInfo.PROFILEPIC;

            list.DASHINFO = dashInfo;

            return View(list);
        }

        [HttpPost]
        public ActionResult AddTask (AddTaskModel info)
        {
            string name = info.TASKNAME;
            string cat = info.CATEGORY;
            int empid = info.EMPLOYEEID;
            string des = info.DESCRIPTION;
            var date = info.DEADLINE;

            TASK newtask = new TASK()
            {
                TASK_NAME = name,
                TASK_DEADLINE = date,
                TASK_DESCRIPTION = des,
                TASK_CATEGORY = cat
            };

            db.TASKS.Add(newtask);

            db.SaveChanges();

            int id = newtask.TASK_ID;

            db.TASKEMPLOYEES.Add(new TASKEMPLOYEE() {
                EMPLOYEE_ID = empid,
                TASK_ID = id
            });

            db.SaveChanges();

            var email = Session["Email"].ToString();

            var man = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault();
            int manid = man.MANAGER_ID;

            db.TASKMANAGERS.Add(new TASKMANAGER()
            {
                MANAGER_ID = manid,
                TASK_ID = id
            });

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditTask (int? id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["UserType"].ToString() != "Manager")
            {
                return RedirectToAction("Index");
            }

            var task = db.TASKS.Where(u => u.TASK_ID == id).FirstOrDefault();

            if (task == null)
            {
                return RedirectToAction("Index");
            }

            var taskemp = db.TASKEMPLOYEES.Where(u => u.TASK_ID == id).FirstOrDefault();

            Session["TaskID"] = id;

            EditTaskModel edit = new EditTaskModel();
            edit.TASKNAME = task.TASK_NAME;
            edit.CATEGORY = task.TASK_CATEGORY;
            edit.DEADLINE = task.TASK_DEADLINE;
            edit.DESCRIPTION = task.TASK_DESCRIPTION;

            edit.CATEGORIES = new List<SelectListItem>();
            edit.EMPLOYEES = new List<SelectListItem>();

            var man_email = Session["Email"].ToString();

            var org = db.MANAGERS.Where(u => u.MANAGER_EMAIL == man_email).FirstOrDefault().MANAGER_ORGANIZATION;

            List<EMPLOYEE> le = db.EMPLOYEES.Where(u => u.EMPLOYEE_ORGANIZATION == org).ToList();

            foreach (var e in le)
            {
                var nm = e.EMPLOYEE_FIRSTNAME + " " + e.EMPLOYEE_LASTNAME;
                if (e.Equals(taskemp.EMPLOYEE))
                {
                    edit.EMPLOYEES.Add(new SelectListItem() { Text = nm, Value = e.EMPLOYEE_ID.ToString(), Selected = true });
                } else
                {
                    edit.EMPLOYEES.Add(new SelectListItem() { Text = nm, Value = e.EMPLOYEE_ID.ToString(), Selected = false });
                }
            }

            if (edit.CATEGORY == "UI Design")
            {
                edit.CATEGORIES.Add(new SelectListItem() { Text = "UI Design", Value = "UI Design", Selected = true });
            } else
            {
                edit.CATEGORIES.Add(new SelectListItem() { Text = "UI Design", Value = "UI Design", Selected = false });
            }

            if (edit.CATEGORY == "Bug Fix")
            {
                edit.CATEGORIES.Add(new SelectListItem() { Text = "Bug Fix", Value = "Bug Fix", Selected = true });
            }
            else
            {
                edit.CATEGORIES.Add(new SelectListItem() { Text = "Bug Fix", Value = "Bug Fix", Selected = false });
            }

            if (edit.CATEGORY == "Add Feature")
            {
                edit.CATEGORIES.Add(new SelectListItem() { Text = "Add Feature", Value = "Add Feature", Selected = true });
            }
            else
            {
                edit.CATEGORIES.Add(new SelectListItem() { Text = "Add Feature", Value = "Add Feature", Selected = false });
            }

            DashboardModel dashInfo = generateDashboardInfo();
            edit.NAME = dashInfo.NAME;
            edit.USERTYPE = dashInfo.USERTYPE;
            edit.PROFILEPIC = dashInfo.PROFILEPIC;

            edit.DASHINFO = dashInfo;

            return View(edit);
        }

        [HttpPost]
        public ActionResult EditTask (EditTaskModel edit)
        {
            int id = Int32.Parse(Session["TaskID"].ToString());
            Session["TaskID"] = null;

            var task = db.TASKS.Where(u => u.TASK_ID == id).FirstOrDefault();

            task.TASK_NAME = edit.TASKNAME;
            task.TASK_DESCRIPTION = edit.DESCRIPTION;
            task.TASK_CATEGORY = edit.CATEGORY;
            task.TASK_DEADLINE = edit.DEADLINE;

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UploadTask (int? id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["UserType"].ToString() != "Employee")
            {
                return RedirectToAction("Index");
            }

            DashboardModel dashInfo = generateDashboardInfo();
            UploadTaskModel upload = new UploadTaskModel();

            upload.NAME = dashInfo.NAME;
            upload.USERTYPE = dashInfo.USERTYPE;
            upload.PROFILEPIC = dashInfo.PROFILEPIC;
            upload.DASHINFO = dashInfo;

            var alltask = db.TASKS.ToList();
            bool isf = true;

            upload.TASKS = new List<SelectListItem>();

            foreach (var e in alltask)
            {
                if (isf)
                {
                    isf = false;
                    upload.TASKS.Add(new SelectListItem() { Text = e.TASK_NAME, Value = e.TASK_ID.ToString(), Selected = true});
                } else
                {
                    upload.TASKS.Add(new SelectListItem() { Text = e.TASK_NAME, Value = e.TASK_ID.ToString(), Selected = false });
                }
            }

            return View(upload);
        }

        [HttpPost]
        public ActionResult UploadTask (UploadTaskModel upload)
        {
            if (upload.TASKFFILE != null)
            {
                string hash = RandomString(10);
                string path = hash + upload.TASKFFILE.FileName;
                upload.TASKFFILE.SaveAs(Server.MapPath("~/Upload/Task/") + path);

                db.TASKFILES.Add(new TASKFILE() {
                    FILES = path,
                    TASK_ID = upload.TASKID
                });

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddProgress(int? id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (Session["UserType"].ToString() != "Employee")
            {
                return RedirectToAction("Index");
            }

            DashboardModel dashInfo = generateDashboardInfo();
            AddProgessModel add = new AddProgessModel();

            add.NAME = dashInfo.NAME;
            add.USERTYPE = dashInfo.USERTYPE;
            add.PROFILEPIC = dashInfo.PROFILEPIC;
            add.DASHINFO = dashInfo;

            var alltask = db.TASKS.ToList();
            bool isf = true;

            add.TASKS = new List<SelectListItem>();

            foreach (var e in alltask)
            {
                if (isf)
                {
                    isf = false;
                    add.TASKS.Add(new SelectListItem() { Text = e.TASK_NAME, Value = e.TASK_ID.ToString(), Selected = true });
                }
                else
                {
                    add.TASKS.Add(new SelectListItem() { Text = e.TASK_NAME, Value = e.TASK_ID.ToString(), Selected = false });
                }
            }

            return View(add);
        }

        [HttpPost]
        public ActionResult AddProgress (AddProgessModel add)
        {
            if (add.PROGRESSFILE != null)
            {
                string hash = RandomString(10);
                string path = hash + add.PROGRESSFILE.FileName;
                add.PROGRESSFILE.SaveAs(Server.MapPath("~/Upload/Progress/") + path);

                PROGRESS progress = new PROGRESS() {
                    PROGRESS_DESCRIPTION = add.DESCRIPTION,
                    PROGRESS_PERCENTAGE = add.PERCENT
                };

                db.PROGRESSES.Add(progress);

                db.SaveChanges();

                db.TASKPROGRESSES.Add(new TASKPROGRESS() {
                    PROGRESS_ID = progress.PROGRESS_ID,
                    TASK_ID = add.TASKID
                });

                db.SaveChanges();

                db.PROGRESSFILES.Add(new PROGRESSFILE() {
                    PROGRESS_ID = progress.PROGRESS_ID,
                    FILES = path
                });

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult ViewTask (int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var task = db.TASKS.Where(u => u.TASK_ID == id).FirstOrDefault();
            var taskemp = db.TASKEMPLOYEES.Where(u => u.TASK_ID == id).FirstOrDefault();

            DashboardModel dashInfo = generateDashboardInfo();
            ViewTaskModel view = new ViewTaskModel();

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;
            view.DASHINFO = dashInfo;

            view.TASKID = id;
            view.TASKNAME = task.TASK_NAME;
            view.CATEGORY = task.TASK_CATEGORY;
            view.DESCRIPTION = task.TASK_DESCRIPTION;
            view.DEADLINE = task.TASK_DEADLINE;
            view.EMPLOYEENAME = taskemp.EMPLOYEE.EMPLOYEE_FIRSTNAME + " " + taskemp.EMPLOYEE.EMPLOYEE_LASTNAME;

            var percentlist = db.TASKPROGRESSES.Where(u => u.TASK_ID == id).ToList();

            view.PERCENT = new List<int>();

            foreach (var e in percentlist)
            {
                var pid = e.PROGRESS.PROGRESS_PERCENTAGE;
                view.PERCENT.Add((int)pid);
            }

            view.FILES = new List<string>();

            var files = db.TASKFILES.Where(u => u.TASK_ID == id).ToList();

            foreach (var e in files)
            {
                view.FILES.Add(e.FILES);
            }

            return View(view);
        }

        public ActionResult ViewProgress (int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }          

            DashboardModel dashInfo = generateDashboardInfo();

            var progresslist = db.TASKPROGRESSES.Where(u => u.TASK_ID == id).ToList();

            ViewProgressModel view = new ViewProgressModel();

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;
            view.DASHINFO = dashInfo;

            view.DESCRIPTION = new List<string>();
            view.IMAGES = new List<string>();

            foreach (var e in progresslist)
            {
                var des = e.PROGRESS.PROGRESS_DESCRIPTION;
                var img = db.PROGRESSFILES.Where(u => u.PROGRESS_ID == e.PROGRESS.PROGRESS_ID).FirstOrDefault();
                var image = img.FILES;

                view.DESCRIPTION.Add(des);
                view.IMAGES.Add(image);
            }

            return View(view);
        }

        public ActionResult ViewTable ()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string org = "";
            var email = Session["Email"].ToString();
            var usertype = Session["UserType"].ToString();

            if (usertype == "Manager")
            {
                org = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault().MANAGER_ORGANIZATION;
            } else
            {
                org = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault().EMPLOYEE_ORGANIZATION;
            }

            DashboardModel dashInfo = generateDashboardInfo();
            ViewTable view = new ViewTable();

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;

            view.DASHINFO = dashInfo;

            view.EMPS = db.EMPLOYEES.Where(u => u.EMPLOYEE_ORGANIZATION == org).ToList();
            view.MANS = db.MANAGERS.Where(u => u.MANAGER_ORGANIZATION == org).ToList();

            view.TASKS = new List<TASK>();
            view.TASKUSER = new List<string>();
            view.PERCENT = new List<int>();

            if (usertype == "Manager")
            {
                int id = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault().MANAGER_ID;

                var tasks = db.TASKMANAGERS.Where(u => u.MANAGER_ID == id).ToList();

                foreach (var e in tasks)
                {
                    int taskid = e.TASK_ID;
                    view.TASKS.Add(db.TASKS.Where(u=>u.TASK_ID == taskid).FirstOrDefault());
                    int empid = db.TASKEMPLOYEES.Where(u => u.TASK_ID == taskid).FirstOrDefault().EMPLOYEE_ID;
                    var ep = db.EMPLOYEES.Where(u => u.EMPLOYEE_ID == empid).FirstOrDefault();

                    view.TASKUSER.Add(ep.EMPLOYEE_FIRSTNAME + " " + ep.EMPLOYEE_LASTNAME);

                    var allpercent = db.TASKPROGRESSES.Where(u => u.TASK_ID == taskid).ToList();
                    int per = 0;

                    foreach (var f in allpercent)
                    {
                        per += (int)f.PROGRESS.PROGRESS_PERCENTAGE;
                    }

                    view.PERCENT.Add(per);
                }
            } else
            {
                int id = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault().EMPLOYEE_ID;

                var tasks = db.TASKEMPLOYEES.Where(u => u.EMPLOYEE_ID == id).ToList();

                foreach (var e in tasks)
                {
                    int taskid = e.TASK_ID;
                    view.TASKS.Add(db.TASKS.Where(u => u.TASK_ID == taskid).FirstOrDefault());
                    int empid = db.TASKMANAGERS.Where(u => u.TASK_ID == taskid).FirstOrDefault().MANAGER_ID;
                    var ep = db.MANAGERS.Where(u => u.MANAGER_ID == empid).FirstOrDefault();

                    view.TASKUSER.Add(ep.MANAGER_FIRSTNAME + " " + ep.MANAGER_LASTNAME);

                    var allpercent = db.TASKPROGRESSES.Where(u => u.TASK_ID == taskid).ToList();
                    int per = 0;

                    foreach (var f in allpercent)
                    {
                        per += (int)f.PROGRESS.PROGRESS_PERCENTAGE;
                    }

                    view.PERCENT.Add(per);
                }
            }

            int userid = Int32.Parse(Session["UserId"].ToString());

            var event_list = db.EVENTUSERS.ToList();

            view.EVENTS = new List<EVENT>();

            foreach (var e in event_list)
            {
                if (e.USER_ID == userid)
                {
                    int nowy = System.DateTime.Now.Year;
                    int nowm = System.DateTime.Now.Month;
                    int nowd = System.DateTime.Now.Day;

                    int y = e.EVENT.EVENT_DEADLINE.Year;
                    int m = e.EVENT.EVENT_DEADLINE.Month;
                    int d = e.EVENT.EVENT_DEADLINE.Day;

                    if (y >= nowy)
                    {
                        if (y == nowy)
                        {
                            if (m >= nowm)
                            {
                                if (m == nowm)
                                {
                                    if (d >= nowd)
                                    {
                                        view.EVENTS.Add(e.EVENT);
                                    }
                                } else
                                {
                                    view.EVENTS.Add(e.EVENT);
                                }
                            }
                        } else
                        {
                            view.EVENTS.Add(e.EVENT);
                        }
                    }
                }

                // view.EVENTS.Add(e.EVENT);
            }

            return View(view);
        }

        public ActionResult ViewUser ()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string org = "";
            var email = Session["Email"].ToString();
            var usertype = Session["UserType"].ToString();

            if (usertype == "Manager")
            {
                org = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault().MANAGER_ORGANIZATION;
            }
            else
            {
                org = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault().EMPLOYEE_ORGANIZATION;
            }

            DashboardModel dashInfo = generateDashboardInfo();
            ViewTable view = new ViewTable();

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;

            view.DASHINFO = dashInfo;

            view.EMPS = db.EMPLOYEES.Where(u => u.EMPLOYEE_ORGANIZATION == org).ToList();
            view.MANS = db.MANAGERS.Where(u => u.MANAGER_ORGANIZATION == org).ToList();

            view.USERID = new List<int>();

            foreach (var e in view.MANS)
            {
                var emails = e.MANAGER_EMAIL;
                int id = db.USERS.Where(u => u.USER_EMAIL == emails).FirstOrDefault().USER_ID;
                view.USERID.Add(id);
            }

            foreach (var e in view.EMPS)
            {
                var emails = e.EMPLOYEE_EMAIL;
                int id = db.USERS.Where(u => u.USER_EMAIL == emails).FirstOrDefault().USER_ID;
                view.USERID.Add(id);
            }

            return View(view);
        }

        public ActionResult ViewTaskAll ()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string org = "";
            var email = Session["Email"].ToString();
            var usertype = Session["UserType"].ToString();

            if (usertype == "Manager")
            {
                org = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault().MANAGER_ORGANIZATION;
            }
            else
            {
                org = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault().EMPLOYEE_ORGANIZATION;
            }

            DashboardModel dashInfo = generateDashboardInfo();
            ViewTable view = new ViewTable();

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;

            view.DASHINFO = dashInfo;

            view.EMPS = db.EMPLOYEES.Where(u => u.EMPLOYEE_ORGANIZATION == org).ToList();
            view.MANS = db.MANAGERS.Where(u => u.MANAGER_ORGANIZATION == org).ToList();

            view.TASKS = new List<TASK>();
            view.TASKUSER = new List<string>();
            view.PERCENT = new List<int>();

            if (usertype == "Manager")
            {
                int id = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault().MANAGER_ID;

                var tasks = db.TASKMANAGERS.Where(u => u.MANAGER_ID == id).ToList();

                foreach (var e in tasks)
                {
                    int taskid = e.TASK_ID;
                    view.TASKS.Add(db.TASKS.Where(u => u.TASK_ID == taskid).FirstOrDefault());
                    int empid = db.TASKEMPLOYEES.Where(u => u.TASK_ID == taskid).FirstOrDefault().EMPLOYEE_ID;
                    var ep = db.EMPLOYEES.Where(u => u.EMPLOYEE_ID == empid).FirstOrDefault();

                    view.TASKUSER.Add(ep.EMPLOYEE_FIRSTNAME + " " + ep.EMPLOYEE_LASTNAME);

                    var allpercent = db.TASKPROGRESSES.Where(u => u.TASK_ID == taskid).ToList();
                    int per = 0;

                    foreach (var f in allpercent)
                    {
                        per += (int)f.PROGRESS.PROGRESS_PERCENTAGE;
                    }

                    view.PERCENT.Add(per);
                }
            }
            else
            {
                int id = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault().EMPLOYEE_ID;

                var tasks = db.TASKEMPLOYEES.Where(u => u.EMPLOYEE_ID == id).ToList();

                foreach (var e in tasks)
                {
                    int taskid = e.TASK_ID;
                    view.TASKS.Add(db.TASKS.Where(u => u.TASK_ID == taskid).FirstOrDefault());
                    int empid = db.TASKMANAGERS.Where(u => u.TASK_ID == taskid).FirstOrDefault().MANAGER_ID;
                    var ep = db.MANAGERS.Where(u => u.MANAGER_ID == empid).FirstOrDefault();

                    view.TASKUSER.Add(ep.MANAGER_FIRSTNAME + " " + ep.MANAGER_LASTNAME);

                    var allpercent = db.TASKPROGRESSES.Where(u => u.TASK_ID == taskid).ToList();
                    int per = 0;

                    foreach (var f in allpercent)
                    {
                        per += (int)f.PROGRESS.PROGRESS_PERCENTAGE;
                    }

                    view.PERCENT.Add(per);
                }
            }

            return View(view);
        }

        [HttpGet]
        public ActionResult AddEvent ()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            DashboardModel dashInfo = generateDashboardInfo();
            AddEventModel view = new AddEventModel();

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;

            view.DASHINFO = dashInfo;

            view.CATEGORIES = new List<SelectListItem>();

            view.CATEGORIES.Add(new SelectListItem() { Text = "Default", Value = "Default", Selected = true });
            view.CATEGORIES.Add(new SelectListItem() { Text = "Meeting", Value = "Meeting", Selected = false });
            view.CATEGORIES.Add(new SelectListItem() { Text = "Holiday", Value = "Holiday", Selected = false });
            view.CATEGORIES.Add(new SelectListItem() { Text = "Interview", Value = "Interview", Selected = false });

            return View(view);
        }

        [HttpPost]
        public ActionResult AddEvent (AddEventModel view)
        {
            EVENT evt = new EVENT();
            evt.EVENT_NAME = view.EVENTNAME;
            evt.EVENT_CATEGORY = view.CATEGORY;
            evt.EVENT_DESCRIPTION = view.DESCRIPTION;
            evt.EVENT_DEADLINE = view.DEADLINE;

            db.EVENTS.Add(evt);

            db.SaveChanges();

            int evtid = evt.EVENT_ID;

            int userid = Int32.Parse(Session["UserId"].ToString());

            db.EVENTUSERS.Add(new EVENTUSER() {
                EVENT_ID = evtid,
                USER_ID = userid
            });

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ViewEventAll ()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            DashboardModel dashInfo = generateDashboardInfo();
            ViewTable view = new ViewTable();

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;

            view.DASHINFO = dashInfo;

            int userid = Int32.Parse(Session["UserId"].ToString());

            var event_list = db.EVENTUSERS.ToList();

            view.EVENTS = new List<EVENT>();

            foreach (var e in event_list)
            {
                if (e.USER_ID == userid)
                {
                    view.EVENTS.Add(e.EVENT);
                }
            }

            view.EVENTS = view.EVENTS.OrderBy(p => p.EVENT_DEADLINE).ToList();

            return View(view);
        }

        public ActionResult ViewEvent (int? id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            DashboardModel dashInfo = generateDashboardInfo();
            ViewEventModel view = new ViewEventModel();

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;

            view.DASHINFO = dashInfo;

            view.EVT = db.EVENTS.Where(u => u.EVENT_ID == id).FirstOrDefault();

            return View(view);
        }

        [HttpGet]
        public ActionResult EditEvent (int? id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            DashboardModel dashInfo = generateDashboardInfo();
            AddEventModel view = new AddEventModel();

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;

            view.DASHINFO = dashInfo;

            Session["Id"] = id;

            var evt = db.EVENTS.Where(u => u.EVENT_ID == id).FirstOrDefault();

            view.EVENTNAME = evt.EVENT_NAME;
            view.CATEGORY = evt.EVENT_CATEGORY;
            view.DEADLINE = evt.EVENT_DEADLINE;
            view.DESCRIPTION = evt.EVENT_DESCRIPTION;

            view.CATEGORIES = new List<SelectListItem>();

            if (evt.EVENT_CATEGORY == "Default")
            {
                view.CATEGORIES.Add(new SelectListItem() { Text = "Default", Value = "Default", Selected = true });
            }
            else
            {
                view.CATEGORIES.Add(new SelectListItem() { Text = "Default", Value = "Default", Selected = false });
            }

            if (evt.EVENT_CATEGORY == "Meeting")
            {
                view.CATEGORIES.Add(new SelectListItem() { Text = "Meeting", Value = "Meeting", Selected = true });
            } else
            {
                view.CATEGORIES.Add(new SelectListItem() { Text = "Meeting", Value = "Meeting", Selected = false });
            }

            if (evt.EVENT_CATEGORY == "Holiday")
            {
                view.CATEGORIES.Add(new SelectListItem() { Text = "Holiday", Value = "Holiday", Selected = true });
            }
            else
            {
                view.CATEGORIES.Add(new SelectListItem() { Text = "Holiday", Value = "Holiday", Selected = false });
            }

            if (evt.EVENT_CATEGORY == "Interview")
            {
                view.CATEGORIES.Add(new SelectListItem() { Text = "Interview", Value = "Interview", Selected = true });
            }
            else
            {
                view.CATEGORIES.Add(new SelectListItem() { Text = "Interview", Value = "Interview", Selected = false });
            }

            return View(view);
        }

        [HttpPost]
        public ActionResult EditEvent (AddEventModel view)
        {
            int id = Int32.Parse(Session["Id"].ToString());
            Session["Id"] = null;

            var evt = db.EVENTS.Where(u => u.EVENT_ID == id).FirstOrDefault();

            evt.EVENT_NAME = view.EVENTNAME;
            evt.EVENT_DESCRIPTION = view.DESCRIPTION;
            evt.EVENT_DEADLINE = view.DEADLINE;
            evt.EVENT_CATEGORY = view.CATEGORY;

            db.Entry(evt).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Notification ()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(generateDashboardInfo());
        }

        public ActionResult DeleteEventFromToDoList (int? id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            DashboardModel dash = generateDashboardInfo();

            int indx = id.Value;
            var evt = db.EVENTS.Where(u=>u.EVENT_ID == id).FirstOrDefault();

            var event_id = evt.EVENT_ID;
            var user_id = int.Parse(Session["UserId"].ToString());

            var one = db.EVENTUSERS.Where(u=>u.USER_ID == user_id && u.EVENT_ID == event_id).FirstOrDefault();
            db.EVENTUSERS.Remove(one);
            db.SaveChanges();

            db.EVENTS.Remove(evt);
            db.SaveChanges();

            return View();
        }

        public ActionResult CreateToDoList (string id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            EVENT newevt = new EVENT();
            newevt.EVENT_NAME = id;
            newevt.EVENT_CATEGORY = "Default";
            newevt.EVENT_DESCRIPTION = "Default";
            newevt.EVENT_DEADLINE = System.DateTime.Now;
            newevt.EVENT_DEADLINE = newevt.EVENT_DEADLINE.AddDays(2);

            db.EVENTS.Add(newevt);
            db.SaveChanges();

            int userid = Int32.Parse(Session["UserId"].ToString());

            db.EVENTUSERS.Add(new EVENTUSER() {
                EVENT_ID = newevt.EVENT_ID,
                USER_ID = userid
            });

            db.SaveChanges();

            return View();
        }

        public ActionResult Calendar (int? id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var ok = generateDashboardInfo();
            ok.NOWDATE = id.Value;

            return View(ok);
        }

        public ActionResult Rooms ()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            int userid = Int32.Parse(Session["UserId"].ToString());
            var rooms = db.CHATROOMS.Where(u => u.USER_ID == userid).ToList();

            DashboardModel dashInfo = generateDashboardInfo();
            RoomsModel view = new RoomsModel();
            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;
            view.DASHINFO = dashInfo;

            view.ROOMS = new List<ROOM>();

            foreach (var e in rooms)
            {
                view.ROOMS.Add(e.ROOM);
            }

            return View(view);
        }

        [HttpGet]
        public ActionResult AddRoom ()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            DashboardModel dashInfo = generateDashboardInfo();
            AddRoomModel view = new AddRoomModel();

            int userid = Int32.Parse(Session["UserId"].ToString());

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;
            view.DASHINFO = dashInfo;

            view.ROOMNAME = "Sample Room";
            view.USERS = new List<SelectListItem>();

            var alluser = db.USERS.ToList();

            bool f = true;

            var email = Session["Email"].ToString();
            var org = "";
            if (view.USERTYPE == "Manager")
            {
                org = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault().MANAGER_ORGANIZATION;
            } else
            {
                org = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault().EMPLOYEE_ORGANIZATION;
            }

            foreach (var e in alluser)
            {
                if (e.USER_ID == userid) continue;

                if (e.USER_TYPE == "Manager")
                {
                    var newuser = db.MANAGERS.Where(u => u.MANAGER_EMAIL == e.USER_EMAIL).FirstOrDefault();
                    var neworg = newuser.MANAGER_ORGANIZATION;

                    if (neworg == org)
                    {
                        if (f)
                        {
                            f = false;
                            view.USERS.Add(new SelectListItem() { Text = newuser.MANAGER_FIRSTNAME + " " + newuser.MANAGER_LASTNAME, Value = e.USER_ID.ToString(), Selected = true});
                        } else
                        {
                            view.USERS.Add(new SelectListItem() { Text = newuser.MANAGER_FIRSTNAME + " " + newuser.MANAGER_LASTNAME, Value = e.USER_ID.ToString(), Selected = false });
                        }
                    }
                } else
                {
                    var newuser = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == e.USER_EMAIL).FirstOrDefault();
                    var neworg = newuser.EMPLOYEE_ORGANIZATION;

                    if (neworg == org)
                    {
                        if (f)
                        {
                            f = false;
                            view.USERS.Add(new SelectListItem() { Text = newuser.EMPLOYEE_FIRSTNAME + " " + newuser.EMPLOYEE_LASTNAME, Value = e.USER_ID.ToString(), Selected = true });
                        }
                        else
                        {
                            view.USERS.Add(new SelectListItem() { Text = newuser.EMPLOYEE_FIRSTNAME + " " + newuser.EMPLOYEE_LASTNAME, Value = e.USER_ID.ToString(), Selected = false });
                        }
                    }
                }
            }

            return View(view);
        }

        [HttpPost]
        public ActionResult AddRoom (AddRoomModel view)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            int userid = Int32.Parse(Session["UserId"].ToString());

            ROOM newroom = new ROOM();
            newroom.ROOM_NAME = view.ROOMNAME;

            db.ROOMS.Add(newroom);
            db.SaveChanges();

            int roomid = newroom.ROOM_ID;

            db.CHATROOMS.Add(new CHATROOM() {
                ROOM_ID = roomid,
                USER_ID = userid
            });

            db.SaveChanges();

            db.CHATROOMS.Add(new CHATROOM()
            {
                ROOM_ID = roomid,
                USER_ID = view.USERID
            });

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ViewRoom (int? id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var roomname = db.ROOMS.Where(u => u.ROOM_ID == id.Value).FirstOrDefault().ROOM_NAME;
            int userid = Int32.Parse(Session["UserId"].ToString());

            if (db.CHATROOMS.Where(u=>u.USER_ID == userid).FirstOrDefault() == null)
            {
                return RedirectToAction("Index");
            }

            var msg = db.CHATMESSAGES.Where(u => u.ROOM_ID == id.Value).ToList();
            List<int> sent, received;
            sent = new List<int>();
            received = new List<int>();

            foreach (var e in msg)
            {
                if (e.USER_ID == userid)
                {
                    sent.Add(e.CHATMESSAGE_ID);
                } else
                {
                    received.Add(e.CHATMESSAGE_ID);
                }
            }

            DashboardModel dashInfo = generateDashboardInfo();
            ViewRoomModel view = new ViewRoomModel();

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;
            view.DASHINFO = dashInfo;

            view.SENT = sent;
            view.RECEIVED = received;

            view.ROOMNAME = roomname;

            return View(view);
        }

        [HttpGet]
        public ActionResult SendMsg ()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            DashboardModel dashInfo = generateDashboardInfo();
            bool f = true;

            int userid = Int32.Parse(Session["UserId"].ToString());
            var rooms = db.CHATROOMS.Where(u => u.USER_ID == userid).ToList();

            List<SelectListItem> selection = new List<SelectListItem>();

            foreach (var e in rooms)
            {
                if (f)
                {
                    f = false;
                    selection.Add(new SelectListItem() { Text = e.ROOM.ROOM_NAME, Value = e.ROOM_ID.ToString(), Selected = true});
                } else
                {
                    selection.Add(new SelectListItem() { Text = e.ROOM.ROOM_NAME, Value = e.ROOM_ID.ToString(), Selected = true });
                }
            }

            SendMsgModel view = new SendMsgModel();
            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;
            view.DASHINFO = dashInfo;

            view.ROOMS = selection;

            return View(view);
        }

        [HttpPost]
        public ActionResult SendMsg (SendMsgModel view)
        {
            int userid = Int32.Parse(Session["UserId"].ToString());

            db.CHATMESSAGES.Add(new CHATMESSAGE() {
                ROOM_ID = view.ROOMID,
                USER_ID = userid,
                MESSAGE = view.MSG
            });

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ViewMsg (int? id)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var msg = db.CHATMESSAGES.Where(u => u.CHATMESSAGE_ID == id).FirstOrDefault();
            var roomid = msg.ROOM_ID;
            int userid = Int32.Parse(Session["UserId"].ToString());

            if (db.CHATROOMS.Where(u=>u.USER_ID == userid && u.ROOM_ID == roomid).FirstOrDefault() == null)
            {
                return RedirectToAction("Index");
            }

            var senderid = msg.USER_ID;

            var senderuser = db.USERS.Where(u => u.USER_ID == senderid).FirstOrDefault();
            var name = "";

            if (senderuser.USER_TYPE == "Manager")
            {
                var man = db.MANAGERS.Where(u => u.MANAGER_EMAIL == senderuser.USER_EMAIL).FirstOrDefault();
                name = man.MANAGER_FIRSTNAME + " " + man.MANAGER_LASTNAME;
            } else
            {
                var man = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == senderuser.USER_EMAIL).FirstOrDefault();
                name = man.EMPLOYEE_FIRSTNAME + " " + man.EMPLOYEE_LASTNAME;
            }

            DashboardModel dashInfo = generateDashboardInfo();
            ViewMsgModel view = new ViewMsgModel();

            view.NAME = dashInfo.NAME;
            view.USERTYPE = dashInfo.USERTYPE;
            view.PROFILEPIC = dashInfo.PROFILEPIC;
            view.DASHINFO = dashInfo;

            view.USERNAME = name;
            view.EMAIL = senderuser.USER_EMAIL;
            view.MSG = msg.MESSAGE;

            return View(view);
        }

        public ActionResult ViewProfile (int? id)
        {
            if (id == null)
            {
                if (Session["Email"] == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var email = Session["Email"].ToString();
                var user_type = Session["UserType"].ToString();
                ProfileModel user_info = null;

                if (user_type == "Manager")
                {
                    var user_check = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault();
                    user_info = generateUserProfile(user_check);
                }
                else
                {
                    var user_check = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault();
                    user_info = generateUserProfile(user_check);
                }

                user_info.OTHERPROFILE = false;

                return View(user_info);
            } else
            {
                var temp_user = db.USERS.Where(u=>u.USER_ID == id).FirstOrDefault();

                ProfileModel user_info = null;

                if (temp_user.USER_TYPE == "Manager")
                {
                    var user_check = db.MANAGERS.Where(u => u.MANAGER_EMAIL == temp_user.USER_EMAIL).FirstOrDefault();
                    user_info = generateUserProfile(user_check);
                }
                else
                {
                    var user_check = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == temp_user.USER_EMAIL).FirstOrDefault();
                    user_info = generateUserProfile(user_check);
                }

                user_info.OTHERPROFILE = true;
                user_info.OTHERID = id.Value;

                return View(user_info);
            }
        }

        [HttpGet]
        public ActionResult EditProfile ()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var email = Session["Email"].ToString();
            var user_type = Session["UserType"].ToString();
            ProfileModel user_info = null;

            if (user_type == "Manager")
            {
                var user_check = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault();
                user_info = generateUserProfile(user_check);
            }
            else
            {
                var user_check = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault();
                user_info = generateUserProfile(user_check);
            }

            var edit = generateEditProfile(user_info);

            return View(edit);
        }

        [HttpPost]
        public ActionResult EditProfile (EditProfileModel edit)
        {
            int userid = Int32.Parse(Session["UserId"].ToString());
            string usertype = Session["UserType"].ToString();
            string email = Session["Email"].ToString();

            if (usertype == "Manager")
            {
                var cuser = db.MANAGERS.Where(u=>u.MANAGER_EMAIL == email).FirstOrDefault();

                cuser.MANAGER_FIRSTNAME = edit.FIRSTNAME;
                cuser.MANAGER_LASTNAME = edit.LASTNAME;
                cuser.MANAGER_PHONE = edit.PHONE;
                cuser.MANAGER_DATEOFBIRTH = edit.DATEOFBIRTH;
                cuser.MANAGER_LOCATION = edit.LOCATION;
                cuser.MANAGER_ORGANIZATION = edit.ORGANIZATION;
                cuser.MANAGER_DESIGNATION = edit.DESIGNATION;

                db.Entry(cuser).State = EntityState.Modified;
                db.SaveChanges();
            } else
            {
                var cuser = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault();

                cuser.EMPLOYEE_FIRSTNAME = edit.FIRSTNAME;
                cuser.EMPLOYEE_LASTNAME = edit.LASTNAME;
                cuser.EMPLOYEE_PHONE = edit.PHONE;
                cuser.EMPLOYEE_DATEOFBIRTH = edit.DATEOFBIRTH;
                cuser.EMPLOYEE_LOCATION = edit.LOCATION;
                cuser.EMPLOYEE_ORGANIZATION = edit.ORGANIZATION;
                cuser.EMPLOYEE_DESIGNATION = edit.DESIGNATION;

                db.Entry(cuser).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (edit.PICTURE != null)
            {
                string hash = RandomString(10);
                string path = hash + edit.PICTURE.FileName;
                edit.PICTURE.SaveAs(Server.MapPath("~/Upload/ProfilePicture/") + path);

                var userpic = db.USERPICS.Where(u => u.USER_ID == userid).FirstOrDefault();

                if (userpic == null)
                {
                    db.USERPICS.Add(new USERPIC() { USER_ID = userid, USER_PIC = path});
                } else
                {
                    userpic.USER_PIC = path;
                    db.Entry(userpic).State = EntityState.Modified;
                }

                db.SaveChanges();
            }

            if (edit.DESCRIPTION != null)
            {
                var userdes = db.USERBIOS.Where(u => u.USER_ID == userid).FirstOrDefault();

                if (userdes == null)
                {
                    db.USERBIOS.Add(new USERBIO() { USER_ID = userid, USER_BIO = edit.DESCRIPTION });
                }
                else
                {
                    userdes.USER_BIO = edit.DESCRIPTION;
                    db.Entry(userdes).State = EntityState.Modified;
                }

                db.SaveChanges();
            } else
            {
                var userdes = db.USERBIOS.Where(u => u.USER_ID == userid).FirstOrDefault();

                if (userdes != null)
                {
                    db.USERBIOS.Remove(userdes);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("ViewProfile");
        }

        public ActionResult Report (int? id, string msg)
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            REPORT rpt = new REPORT();
            rpt.REPORT_BODY = msg;

            db.REPORTS.Add(rpt);
            db.SaveChanges();

            int userid = Int32.Parse(Session["UserId"].ToString());

            db.REPORTUSERS.Add(new REPORTUSER() {
                REPORT_ID = rpt.REPORT_ID,
                USER_ID_BY = userid,
                USER_ID_FOR = id.Value
            });

            db.SaveChanges();

            return View();
        }

        public DashboardModel generateDashboardInfo ()
        {
            DashboardModel list = new DashboardModel();
            string usertype = Session["UserType"].ToString();
            string name = "";
            string email = Session["Email"].ToString();

            if (usertype == "Manager")
            {
                var check = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault();

                name = check.MANAGER_FIRSTNAME + " " + check.MANAGER_LASTNAME;
            }
            else
            {
                var check = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault();

                name = check.EMPLOYEE_FIRSTNAME + " " + check.EMPLOYEE_LASTNAME;
            }

            list.NAME = name;
            list.USERTYPE = usertype;

            int userid = Int32.Parse(Session["UserId"].ToString());

            var pic = db.USERPICS.Where(u => u.USER_ID == userid).FirstOrDefault();

            if (pic != null) list.PROFILEPIC = "/Upload/ProfilePicture/" + pic.USER_PIC;

            list.TASKS = new List<TASK>();
            list.EVENTS = new List<EVENT>();

            if (usertype == "Manager")
            {
                var check = db.MANAGERS.Where(u => u.MANAGER_EMAIL == email).FirstOrDefault();
                int manid = check.MANAGER_ID;

                var task_list = db.TASKMANAGERS.Where(u => u.MANAGER_ID == manid).ToList();

                foreach (var e in task_list)
                {
                    int nowy = System.DateTime.Now.Year;
                    int nowm = System.DateTime.Now.Month;
                    int nowd = System.DateTime.Now.Day;

                    int y = e.TASK.TASK_DEADLINE.Year;
                    int m = e.TASK.TASK_DEADLINE.Month;
                    int d = e.TASK.TASK_DEADLINE.Day;

                    if (y == nowy && m == nowm)
                    {
                        if (d >= nowd && d - nowd <= 2)
                        {
                            list.TASKS.Add(e.TASK);
                        }
                    }
                }
            } else
            {
                var check = db.EMPLOYEES.Where(u => u.EMPLOYEE_EMAIL == email).FirstOrDefault();
                int manid = check.EMPLOYEE_ID;

                var task_list = db.TASKEMPLOYEES.Where(u => u.EMPLOYEE_ID == manid).ToList();

                foreach (var e in task_list)
                {
                    int nowy = System.DateTime.Now.Year;
                    int nowm = System.DateTime.Now.Month;
                    int nowd = System.DateTime.Now.Day;

                    int y = e.TASK.TASK_DEADLINE.Year;
                    int m = e.TASK.TASK_DEADLINE.Month;
                    int d = e.TASK.TASK_DEADLINE.Day;

                    if (y == nowy && m == nowm)
                    {
                        if (d >= nowd && d - nowd <= 2)
                        {
                            list.TASKS.Add(e.TASK);
                        }
                    }
                }
            }

            var event_list = db.EVENTUSERS.Where(u => u.USER_ID == userid).ToList();

            foreach (var e in event_list)
            {
                int nowy = System.DateTime.Now.Year;
                int nowm = System.DateTime.Now.Month;
                int nowd = System.DateTime.Now.Day;

                int y = e.EVENT.EVENT_DEADLINE.Year;
                int m = e.EVENT.EVENT_DEADLINE.Month;
                int d = e.EVENT.EVENT_DEADLINE.Day;

                if (y == nowy && m == nowm)
                {
                    if (d >= nowd && d - nowd <= 2)
                    {
                        list.EVENTS.Add(e.EVENT);
                    }
                }
            }

            list.TODOLIST = new List<EVENT>();

            foreach (var e in event_list)
            {
                int nowy = System.DateTime.Now.Year;
                int nowm = System.DateTime.Now.Month;
                int nowd = System.DateTime.Now.Day;

                int y = e.EVENT.EVENT_DEADLINE.Year;
                int m = e.EVENT.EVENT_DEADLINE.Month;
                int d = e.EVENT.EVENT_DEADLINE.Day;

                if (y == nowy && m == nowm)
                {
                    if (d >= nowd && d - nowd <= 2 && e.EVENT.EVENT_CATEGORY != "Holiday")
                    {
                        list.TODOLIST.Add(e.EVENT);
                    }
                }
            }

            list.TASKS = list.TASKS.OrderBy(u => u.TASK_DEADLINE).ToList();
            list.EVENTS = list.EVENTS.OrderBy(u => u.EVENT_DEADLINE).ToList();
            list.TODOLIST = list.TODOLIST.OrderBy(u => u.EVENT_DEADLINE).ToList();

            return list;
        }

        public EditProfileModel generateEditProfile (ProfileModel pro)
        {
            var split = pro.NAME.Split(' ');
            var fname = split[0];
            var lname = split[1];

            int userid = Int32.Parse(Session["UserId"].ToString());

            var checkDes = db.USERBIOS.Where(u => u.USER_ID == userid).FirstOrDefault();

            string des = "";

            if (checkDes != null)
            {
                des = checkDes.USER_BIO;
            }

            var checkPro = db.USERPICS.Where(u => u.USER_ID == userid).FirstOrDefault();

            string pic = null;

            if (checkPro != null)
            {
                pic = "/Upload/ProfilePicture/" + checkPro.USER_PIC;
            }

            return new EditProfileModel()
            {
                FIRSTNAME = fname,
                LASTNAME = lname,
                EMAIL = pro.EMAIL,
                PHONE = pro.PHONE,
                DESIGNATION = pro.DESIGNATION,
                ORGANIZATION = pro.ORGANIZATION,
                LOCATION = pro.LOCATION,
                DATEOFBIRTH = pro.DATEOFBIRTH,
                DESCRIPTION = des,
                PICTUREPATH = pic
            };
        }

        public ProfileModel generateUserProfile (EMPLOYEE emp)
        {
            var check_user = db.USERS.Where(u => u.USER_EMAIL == emp.EMPLOYEE_EMAIL).FirstOrDefault();
            int userid = check_user.USER_ID;
            string profilepic = null, description = null;

            var checkPic = db.USERPICS.Where(u => u.USER_ID == userid).FirstOrDefault();
            var checkDes = db.USERBIOS.Where(u => u.USER_ID == userid).FirstOrDefault();

            if (checkPic != null) profilepic = "/Upload/ProfilePicture/" + checkPic.USER_PIC;
            if (checkDes != null) description = checkDes.USER_BIO;

            return new ProfileModel()
            {
                ID = emp.EMPLOYEE_ID,
                NAME = emp.EMPLOYEE_FIRSTNAME + " " + emp.EMPLOYEE_LASTNAME,
                EMAIL = emp.EMPLOYEE_EMAIL,
                PHONE = emp.EMPLOYEE_PHONE,
                DESIGNATION = emp.EMPLOYEE_DESIGNATION,
                ORGANIZATION = emp.EMPLOYEE_ORGANIZATION,
                LOCATION = emp.EMPLOYEE_LOCATION,
                DATEOFBIRTH = emp.EMPLOYEE_DATEOFBIRTH,
                PICTURE = profilepic,
                DESCRIPTION = description,
                REGTIME = emp.EMPLOYEE_REGISTRATIONDATE
            };
        }

        public ProfileModel generateUserProfile (MANAGER man)
        {
            var check_user = db.USERS.Where(u => u.USER_EMAIL == man.MANAGER_EMAIL).FirstOrDefault();
            int userid = check_user.USER_ID;
            string profilepic = null, description = null;

            var checkPic = db.USERPICS.Where(u => u.USER_ID == userid).FirstOrDefault();
            var checkDes = db.USERBIOS.Where(u => u.USER_ID == userid).FirstOrDefault();

            if (checkPic != null) profilepic = "/Upload/ProfilePicture/" + checkPic.USER_PIC;
            if (checkDes != null) description = checkDes.USER_BIO;

            return new ProfileModel()
            {
                ID = man.MANAGER_ID,
                NAME = man.MANAGER_FIRSTNAME + " " + man.MANAGER_LASTNAME,
                EMAIL = man.MANAGER_EMAIL,
                PHONE = man.MANAGER_PHONE,
                DESIGNATION = man.MANAGER_DESIGNATION,
                ORGANIZATION = man.MANAGER_ORGANIZATION,
                LOCATION = man.MANAGER_LOCATION,
                DATEOFBIRTH = man.MANAGER_DATEOFBIRTH,
                PICTURE = profilepic,
                DESCRIPTION = description,
                REGTIME = man.MANAGER_REGISTRATIONDATE
            };
        }

        public static string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}