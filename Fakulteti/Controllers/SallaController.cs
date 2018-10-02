using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fakulteti.Models.Sallat;
using Fakulteti.Models.dbKonektimi;
using System.Data.Entity;

namespace Fakulteti.Controllers
{
    public class SallaController : Controller
    {
        FakultetiEntities1 db;
        public SallaController()
        {
            db = new FakultetiEntities1();
        }

        // GET: Salla
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
            }

            if (Session["Check"] == null)
            {
                ViewBag.Check = "";
            }
            else
            {
                ViewBag.Check = Session["Check"].ToString();
            }
            Session["Mesazhi"] = null;
            Session["Check"] = null;
            var sallat = db.tblKlasa.ToList();
            List<SallatList> lista = new List<SallatList>();
            foreach(var item in sallat)
            {
                lista.Add(new SallatList
                {
                    KlasaID = item.KlasaID,
                    Numri = item.Numri.ToString()
                });
            }
        
            return View(lista);
        }

        public ActionResult RegjistroSallenGet()
        {
            return PartialView("_RegjistroSallenGet");
        }

        public ActionResult RegjistroSallenPost(RegjistroSallen modeli)
        {
            //insertimi ne database
            try
            {
                if (ModelState.IsValid)
                {
                    var salla = new tblKlasa();
                    salla.Numri = int.Parse(modeli.Numri);
                    db.tblKlasa.Add(salla);
                    db.SaveChanges();

                    Session["Mesazhi"] = "Salla u regjistrua me sukses.";
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

        public ActionResult ModifikoSallenGet(int SallaID)
        {
            var salla = db.tblKlasa.Find(SallaID);
            if(salla==null)
            {
                Session["mesazhi"] = "Salla me kete numer nuk eshte gjetur.";
                return RedirectToAction("index");
            }
            else
            {
                var mbushModelin = new SallatList();
                mbushModelin.KlasaID = salla.KlasaID;
                mbushModelin.Numri = salla.Numri.ToString();
                return PartialView("_ModifikoSallenGet",mbushModelin);
            }

        }

        public ActionResult ModifikoSallenPost(SallatList modeli)
        {
            try
            {
            var sallaDB = db.tblKlasa.Find(modeli.KlasaID);
            if(sallaDB==null)
            {
                Session["mesazhi"] = "Salla e tille nuk ekziston";
                return RedirectToAction("Index");
            }

            sallaDB.Numri = int.Parse(modeli.Numri);
            db.Entry(sallaDB).State = EntityState.Modified;
            db.SaveChanges();

            Session["mesazhi"] = "Salla u modifikua me sukses.";

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