using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fakulteti.Models.dbKonektimi;
using Fakulteti.Models;
using System.Data.Entity;
using System.Globalization;

namespace Fakulteti.Controllers
{
    public class StudentetController : Controller
    {
        FakultetiEntities1 db;
        public StudentetController()
        {
            db = new FakultetiEntities1();
        }

        // GET: Studentet
        public ActionResult Index()
        {
            if (Session["PerdoruesiID"]==null)
            {
                return RedirectToAction("Login", "Account");
            }
            int? PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
            int? RoliID = int.Parse(Session["RoliID"].ToString());
            if (RoliID != 2)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.RoliID = RoliID;
            if (Session["Mesazhi"]==null)
            {
                ViewBag.Mesazhi = "";
            }
            else
            {
                ViewBag.Mesazhi = Session["Mesazhi"].ToString();
                Session["Mesazhi"] = null;
            }

            var studentet = (from x in db.tblPerdoruesi.Where(x=>x.RoliID==4)
                             join k in db.tblKomuna on x.KomunaID equals k.KomunaID
                             join d in db.tblDepartamenti on x.DepartamentiID equals d.DepartamentiID
                             select new
                             {
                                 StudentiID = x.PerdoruesiID,
                                 Emri = x.Emri,
                                 Mbiemri = x.Mbiemri,
                                 Perdoruesi = x.Perdoruesi,
                                 Telefoni = x.Telefoni,
                                 Datelindja = x.Datelindja,
                                 Departamenti = d.Departamenti,
                                 Komuna = k.Komua
                             }).OrderByDescending(x => x.StudentiID).ToList();


            List<StudentiLista> modeli = new List<StudentiLista>();

            foreach (var item in studentet)
            {
                modeli.Add(new StudentiLista
                {
                    StudentiID = item.StudentiID.ToString(),
                    Emri = item.Emri,
                    Mbiemri = item.Mbiemri,
                    Perdoruesi = item.Perdoruesi,
                    Telefoni = item.Telefoni,
                    Datelindja = item.Datelindja.ToString("dd-MM-yyy"),
                    Departamenti = item.Departamenti,
                    Komuna = item.Komuna
                });
            }

            return View(modeli);
        }

        public ActionResult RegjistroStudentinGet()
        {
            var komunat = (from k in db.tblKomuna
                           select new
                           {
                               id = k.KomunaID,
                               pershkrimi = k.Komua
                           }).ToList();

            var departamentet = (from d in db.tblDepartamenti
                           select new
                           {
                               id = d.DepartamentiID,
                               pershkrimi = d.Departamenti
                           }).ToList();

            ViewBag.Komunat = new SelectList(komunat, "id", "pershkrimi");

            ViewBag.Departamenti = new SelectList(departamentet, "id", "pershkrimi");

            return PartialView("_RegjistroStudentinGet");
        }

        public ActionResult RegjistroStudentinPost(RegjistroStudentin modeli)
        {
            //insertimi ne database
            try
            {
                if(ModelState.IsValid)
                {
                    int dita = int.Parse(modeli.Datelindja.Split('-')[0]);
                    int muaji = int.Parse(modeli.Datelindja.Split('-')[1]);
                    int viti = int.Parse(modeli.Datelindja.Split('-')[2]);
                    DateTime dataLindjes = new DateTime(viti, muaji, dita);
                    var studenti = new tblPerdoruesi();
                    studenti.Emri = modeli.Emri;
                    studenti.Mbiemri = modeli.Mbiemri;
                    studenti.RoliID = 4;
                    studenti.Datelindja = dataLindjes;
                    //studenti.DataRegjistrimit = DateTime.Now;
                    studenti.KomunaID = modeli.KomunaID;
                    studenti.DepartamentiID = modeli.DepartamentiID;
                    studenti.Perdoruesi = modeli.Perdoruesi;
                    studenti.Fjalekalimi = modeli.Fjalekalimi;
                    studenti.Telefoni = modeli.Telefoni;
                    studenti.Salt = "Test";
                    studenti.Aktiv = true;
                    db.tblPerdoruesi.Add(studenti);
                    db.SaveChanges();

                    Session["Mesazhi"] = "Studenti u regjistrua me sukses.";
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["Mesazhi"] = "Te dhenat jo-valide.";
                    return RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {
                Session["Mesazhi"] = "Ka ndodhur nje gabim.";
                return RedirectToAction("Index");
            }
            return null;
        }

        public ActionResult ModifikoStudentinGet(int StudentiID)
        {
            var studenti = db.tblPerdoruesi.Find(StudentiID);
            var komunat = (from k in db.tblKomuna
                           select new
                           {
                               id = k.KomunaID,
                               pershkrimi = k.Komua
                           }).ToList();

            var departamentet = (from d in db.tblDepartamenti
                                 select new
                                 {
                                     id = d.DepartamentiID,
                                     pershkrimi = d.Departamenti
                                 }).ToList();

            ViewBag.Komunat = new SelectList(komunat, "id", "pershkrimi");

            ViewBag.Departamentet = new SelectList(departamentet, "id", "pershkrimi");

            if (studenti == null)
            {
                Session["mesazhi"] = "Studenti me kete numer nuk eshte gjetur.";
                return RedirectToAction("index");
            }
            else
            {
                var mbushModelin = new StudentiLista();
                mbushModelin.Emri = studenti.Emri;
                mbushModelin.Mbiemri = studenti.Mbiemri;
                mbushModelin.Telefoni = studenti.Telefoni;
                mbushModelin.Perdoruesi = studenti.Perdoruesi;
                mbushModelin.Fjalekalimi = studenti.Fjalekalimi;
                mbushModelin.Datelindja = studenti.Datelindja.ToString("dd-MM-yyyy");
                mbushModelin.DepartamentiID = studenti.DepartamentiID;
                mbushModelin.KomunaID = studenti.KomunaID;
  
                return PartialView("_ModifikoStudentinGet", mbushModelin);
            }
        }

        public ActionResult ModifikoStudentinPost(StudentiLista modeli)
        {
            try
            {
            var studentiDB = db.tblPerdoruesi.Find(int.Parse(modeli.StudentiID));
            if (studentiDB == null)
            {
                Session["mesazhi"] = "Studenti nuk ekziston";
                return RedirectToAction("Index");
            }
            CultureInfo myCItrad = new CultureInfo("bg-BG", false);
            DateTime parsedDate = DateTime.ParseExact(modeli.Datelindja,"dd-MM-yyyy",myCItrad);
            studentiDB.Emri = modeli.Emri;
            studentiDB.Mbiemri = modeli.Mbiemri;
            studentiDB.Datelindja = parsedDate;
            studentiDB.Telefoni = modeli.Telefoni;
            studentiDB.Perdoruesi = modeli.Perdoruesi;
            studentiDB.Fjalekalimi = modeli.Fjalekalimi;
            studentiDB.KomunaID = modeli.KomunaID;
            studentiDB.DepartamentiID = modeli.DepartamentiID;
            
            studentiDB.Datelindja = parsedDate;
            db.Entry(studentiDB).State = EntityState.Modified;
            db.SaveChanges();
            Session["mesazhi"] = "Studenti u modifikua me sukses.";

            return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                Session["Mesazhi"] = "Ka ndodhur nje gabim.";
                return RedirectToAction("Index");
            }

        }


    }

}