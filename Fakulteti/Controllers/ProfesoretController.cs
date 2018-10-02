using Fakulteti.Models.dbKonektimi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fakulteti.Models;
using System.Globalization;
using System.Data.Entity;

namespace Fakulteti.Controllers
{
    public class ProfesoretController : Controller
    {
        FakultetiEntities1 db;
        public ProfesoretController()
        {
            db = new FakultetiEntities1();
        }

   
        // GET: Profesoret
        public ActionResult Index()
        {
            int? PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
            int? RoliID = int.Parse(Session["RoliID"].ToString());
            if (RoliID != 2)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.RoliID = RoliID;
            if (Session["Mesazhi"] == null)
            {
                ViewBag.Mesazhi = "";
            }
            else
            {
                ViewBag.Mesazhi = Session["Mesazhi"].ToString();
                Session["Mesazhi"] = "";
            }
            var profesoret = (from x in db.tblPerdoruesi.Where(x => x.RoliID == 3)
                             join k in db.tblKomuna on x.KomunaID equals k.KomunaID
                             join d in db.tblDepartamenti on x.DepartamentiID equals d.DepartamentiID
                             select new
                             {
                                 ProfesoriID = x.PerdoruesiID,
                                 Emri = x.Emri,
                                 Mbiemri = x.Mbiemri,
                                 Perdoruesi = x.Perdoruesi,
                                 Telefoni = x.Telefoni,
                                 Datelindja = x.Datelindja,
                                 Departamenti = d.Departamenti,
                                 Komuna = k.Komua
                             }).OrderByDescending(x => x.ProfesoriID).ToList();

            List<ProfesoriLista> modeli = new List<ProfesoriLista>();

            foreach(var item in profesoret)
            {
                modeli.Add(new ProfesoriLista
                {
                    ProfesoriID = item.ProfesoriID.ToString(),
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

        public ActionResult RegjistroProfesorinGet()
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
            return PartialView("_RegjistroProfesorinGet");
        }

        public ActionResult RegjistroProfesorinPost(RegjistroProfesorin modeli)
        {
            //insertimi ne database
            try
            {
                if (ModelState.IsValid)
                {
                    var profesori = new tblPerdoruesi();
                    int dita = int.Parse(modeli.Datelindja.Split('-')[0]);
                    int muaji = int.Parse(modeli.Datelindja.Split('-')[1]);
                    int viti = int.Parse(modeli.Datelindja.Split('-')[2]);
                    DateTime dataLindjes = new DateTime(viti, muaji, dita);
                    profesori.Emri = modeli.Emri;
                    profesori.Mbiemri = modeli.Mbiemri;
                    profesori.RoliID = 3;
                    profesori.Datelindja = dataLindjes;
                    //profesori.DataRegjistrimit = DateTime.Now;
                    profesori.KomunaID = modeli.KomunaID;
                    profesori.DepartamentiID = modeli.DepartamentiID;
                    profesori.Perdoruesi = modeli.Perdoruesi;
                    profesori.Fjalekalimi = modeli.Fjalekalimi;
                    profesori.Telefoni = modeli.Telefoni;
                    profesori.Salt = "Test";
                    profesori.Aktiv = true;
                    db.tblPerdoruesi.Add(profesori);
                    db.SaveChanges();

                    Session["Mesazhi"] = "Profesori u regjistrua me sukses.";
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["Mesazhi"] = "Te dhenat jo-valide.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Session["Mesazhi"] = "Ka ndodhur nje gabim.";
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult ModifikoProfesorinGet(int ProfesoriID)
        {
            var profesori = db.tblPerdoruesi.Find(ProfesoriID);
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
            if (profesori == null)
            {
                Session["mesazhi"] = "Profesori me kete numer nuk eshte gjetur.";
                return RedirectToAction("index");
            }
            else
            {
                var mbushModelin = new ProfesoriLista();
                mbushModelin.Emri = profesori.Emri;
                mbushModelin.Mbiemri = profesori.Mbiemri;
                mbushModelin.Telefoni = profesori.Telefoni;
                mbushModelin.Perdoruesi = profesori.Perdoruesi;
                mbushModelin.Fjalekalimi = profesori.Fjalekalimi;
                mbushModelin.Datelindja = profesori.Datelindja.ToString("dd-MM-yyyy");
                mbushModelin.DepartamentiID = profesori.DepartamentiID;
                mbushModelin.KomunaID = profesori.KomunaID;

                return PartialView("_ModifikoProfesorinGet", mbushModelin);
            }
        }

        public ActionResult ModifikoProfesorinpOST(ProfesoriLista modeli)
        {
            try
            {
                var profesoriDB = db.tblPerdoruesi.Find(int.Parse(modeli.ProfesoriID));
                if (profesoriDB == null)
                {
                    Session["mesazhi"] = "Profesori nuk ekziston";
                    return RedirectToAction("Index");
                }
                CultureInfo myCItrad = new CultureInfo("bg-BG", false);
                DateTime parsedDate = DateTime.ParseExact(modeli.Datelindja, "dd-MM-yyyy", myCItrad);
                profesoriDB.Emri = modeli.Emri;
                profesoriDB.Mbiemri = modeli.Mbiemri;
                profesoriDB.Datelindja = parsedDate;
                profesoriDB.Telefoni = modeli.Telefoni;
                profesoriDB.Perdoruesi = modeli.Perdoruesi;
                profesoriDB.Fjalekalimi = modeli.Fjalekalimi;
                profesoriDB.KomunaID = modeli.KomunaID;
                profesoriDB.DepartamentiID = modeli.DepartamentiID;

                profesoriDB.Datelindja = parsedDate;
                db.Entry(profesoriDB).State = EntityState.Modified;
                db.SaveChanges();
                Session["mesazhi"] = "Profesori u modifikua me sukses.";

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Session["Mesazhi"] = "Ka ndodhur nje gabim.";
                return RedirectToAction("Index");
            }

        }


    }
}