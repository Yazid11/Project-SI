using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Gestion_repos.Controllers
{
    public class DefaultController : Controller
    {
        ReposEntities bd = new ReposEntities();
        private string a;

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Planing()
        {
            return View();
        }
        public ActionResult Login(string Mail,string Mdp)
        {
            Responsable res;
            Employe emp;
           string a=" Select * from Responsable where Mail = '" + Mail + "' AND Mdp = '" + Mdp + "'";
                var respo = bd.Responsable
             .SqlQuery(a);
            ViewBag.e = a;
               res = new Responsable();
                emp = new Employe();
                 res = bd.Responsable.Where(m => m.Mail == Mail).Where(m=>m.Mdp==Mdp).SingleOrDefault();
                emp = bd.Employe.Where(m => m.Nom.ToString() == Mail).SingleOrDefault();
            if(respo.Count()!=0)
            {
                HttpCookie cookie = new HttpCookie("cookie");
                cookie.Value = Mail;
                this.ControllerContext.HttpContext.Response.Cookies.
                Add(cookie);
                string s = "Select * from Responsable where Mail='" + Mail + "' AND Mdp='" + Mdp + "' ";
                return RedirectToAction("Acceuil");
            }
            else if(emp!=null)
            {
                HttpCookie cookie = new HttpCookie("cookie");
                cookie.Value = emp.Nom.ToString();
                this.ControllerContext.HttpContext.Response.Cookies.
                Add(cookie);
                return RedirectToAction("Demande_congé");
            }
            else
            {

            }
            return View();
        }

        public ActionResult Demande_congé()
        {
            ViewBag.c = this.ControllerContext.HttpContext.Request.Cookies["cookie"].Value;
            if (Request.Form["Date"] != null)
            {
                Demande_congé demande = new Gestion_repos.Demande_congé();
                ViewBag.t = this.ControllerContext.HttpContext.Request.Cookies["cookie"].Value;
                demande.Id = ViewBag.t;
                demande.Id_demende = new Random().Next(888);
                DateTime date = new DateTime();
                date = DateTime.Parse(Request.Form["Date"]);
                int num = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                bd.Demande_congé.Add(demande);
                bd.SaveChanges();
            }
            return View();
        }
        public ActionResult Deco()
        {
            HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["cookie"];
            cookie.Expires = DateTime.Now.AddDays(-1);
            this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            return RedirectToAction("Login");
        }
        public ActionResult Acceuil()
        {
            int num = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            Dictionary<string,string> dict=new Dictionary<string, string>();
            dict.Add("Monday","Lundi");
            dict.Add("Tuesday", "Mardi");
            dict.Add("Wednesday", "Mercredi");
            dict.Add("Thursday", "Jeudi");
            dict.Add("Friday", "Vendreri");
            dict.Add("Saturday", "Samedi");
            dict.Add("Sunday", "Dimanche");
            DateTime date = new DateTime();
            date = DateTime.Now;
            string jour=date.DayOfWeek.ToString();
            string jou;
            bool a= dict.TryGetValue(jour,out jou);
            ViewBag.e = jou;
            try
            {
                ViewBag.c = this.ControllerContext.HttpContext.Request.Cookies["cookie"].Value;
            }
            catch(Exception rx)
            {
                RedirectToAction("Login");
            }
            return View();
        }
        public static DateTime FirstDateOfWeek(int year, int weekNum)
        {
            CalendarWeekRule rule = new CalendarWeekRule();
            Debug.Assert(weekNum >= 1);
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
            DateTime firstMonday = jan1.AddDays(daysOffset);
            Debug.Assert(firstMonday.DayOfWeek == DayOfWeek.Monday);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstMonday, rule, DayOfWeek.Monday);
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            DateTime result = firstMonday.AddDays(weekNum * 7);
            return result;
        }
        public ActionResult List_congés()
        {
            if(Request.Form["fe"]!=null)
            {
                foreach(var x in bd.Demande_congé)
                {
                    if(Request.Form[x.Id_demende.ToString()]!=null)
                    {
                        Repos rep = new Repos();
                        Demande_congé dem = new Gestion_repos.Demande_congé();
                        int aa=Int32.Parse(Request.Form[x.Id_demende.ToString()]);
                        dem = bd.Demande_congé.Where(m=>m.Id_demende==aa).SingleOrDefault();
                        rep.congé = 1;
                        rep.jour = "";
                        rep.Id_repos = new Random().Next(777);
                        rep.shift = 0;
                        rep.Id = dem.Id;
                        rep.num_sem = dem.semaine;
                        rep.Type_congé = "f";
                        bd.Repos.Add(rep);
                    }
                }
                bd.SaveChanges();
            }
            int num = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Monday", "Lundi");
            dict.Add("Tuesday", "Mardi");
            dict.Add("Wednesday", "Mercredi");
            dict.Add("Thursday", "Jeudi");
            dict.Add("Friday", "Vendreri");
            dict.Add("Saturday", "Samedi");
            dict.Add("Sunday", "Dimanche");
            DateTime date = new DateTime();
            date = DateTime.Now;
            string jour = date.DayOfWeek.ToString();
            string jou;
            bool a = dict.TryGetValue(jour, out jou);
            ViewBag.e = jou;
            ViewBag.c = this.ControllerContext.HttpContext.Request.Cookies["cookie"].Value;
            return View();
        }
        public ActionResult Ajouter()
        {
            if (Request.Form["nom"] != null)
            {
                Employe emp = new Employe();
                emp.Id = new Random().Next(998);
                emp.Nom = Request.Form["nom"];
                emp.Préonm = Request.Form["prénom"];
                emp.Age = Request.Form["age"];
                bd.Employe.Add(emp);
                /*foreach (HttpPostedFileBase v in fileselect)
                {

                    {
                        try
                        {
                            string filename = Path.GetFileName(v.FileName);
                            string path = Path.Combine(Server.MapPath("~/images/"), v.FileName);
                            v.SaveAs(path);
                            emp.photo += "~/images/" + v.FileName + ";";
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }*/
            }
            try
            {
                ViewBag.c = this.ControllerContext.HttpContext.Request.Cookies["cookie"].Value;
            }
            catch (Exception rx)
            {
                RedirectToAction("Login");
            }
            bd.SaveChanges();
            return View();
        }
        public ActionResult profil(int id)
        {
            return View();
        }
        public ActionResult Repos()
        {
            return View(bd.Employe);
        }
        public ActionResult Da()
        {
            ReposEntities bd = new ReposEntities();
            DateTime enteredDate = DateTime.Parse(Request.Form["debut"]);
            int num=CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(enteredDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            foreach (var x in bd.Employe)
            {
                string jour="";
               
                if (Request.Form[x.Id+"+L"]!=null)
                {
                    jour +="Lundi/";
                }
                if (Request.Form[x.Id + "+M"] != null)
                {
                    jour +="Mardi/";
                }
                if (Request.Form[x.Id + "+Me"] != null)
                {
                    jour += "Mercredi/";
                }
                if (Request.Form[x.Id + "+J"] != null)
                {
                    jour += "Jeudi/";
                }
                if (Request.Form[x.Id + "+V"] != null)
                {
                    jour += "Vendredi/";
                }
                if (Request.Form[x.Id + "+S"] != null)
                {
                    jour += "Samedi/";
                }
                if (Request.Form[x.Id + "+D"] != null)
                {
                    jour += "Dimanche/" ;
                }
                Repos em=new Repos();
                em.jour = jour;
                em.Id = x.Id;
                em.num_sem = num;
                em.shift = Int32.Parse(Request.Form["shift+"+ x.Id]) ;
                em.Id_repos = 55;
                if (Request.Form["conge+" + x.Id] != null)
                {
                    em.congé = 1;
                }
                else
                {
                    em.congé = 0;
                }
                em.Type_congé = "f";
                IQueryable<int> num_test = bd.Repos.Where(m => m.Id == x.Id).Select(m=>m.num_sem);
                if (!num_test.Contains(em.num_sem))
                { 
                    bd.Repos.Add(em);
                    
                }
               }
            bd.SaveChanges();
            return RedirectToAction("Acceuil");
        }
    }
}