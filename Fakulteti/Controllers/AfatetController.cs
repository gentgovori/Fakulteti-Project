using Fakulteti.Models.Afatet;
using Fakulteti.Models.dbKonektimi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fakulteti.Controllers
{
    public class AfatetController : Controller
    {
        FakultetiEntities1 db;
        public AfatetController()
        {
            db = new FakultetiEntities1();
        }

        // GET: Afatet
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

            var afatet = db.tblAfati.ToList();
            List<AfatiLista> modeli = new List<AfatiLista>();
            foreach (var item in afatet)
            {
                modeli.Add(new AfatiLista
                {
                    AfatiID = item.AfatiID.ToString(),
                    Afati = item.Afati,
                    DataFillimit = item.DataFillimit.ToString("dd-MM-yyyy"),
                    DataMbarimit = item.DataMbarimit.Date.ToString("dd-MM-yyyy"),
                    Aktiv = item.Aktiv,
                    NumriProvimeve = item.NumriProvimeve.ToString()
                });
            }

            return View(modeli);
        }

        public ActionResult RegjistroAfatinGet()
        {
            return PartialView("_RegjistroAfatinGet");
        }

        public ActionResult RegjistroAfatinPost(AfatiLista modeli)
        {
            //insertimi ne database
            try
            {
                if (ModelState.IsValid)
                {
                    var afati = new tblAfati();
                    afati.Afati = modeli.Afati;
                    afati.DataFillimit = DateTime.Parse(modeli.DataFillimit);
                    afati.DataMbarimit = DateTime.Parse(modeli.DataMbarimit);
                    afati.NumriProvimeve = int.Parse(modeli.NumriProvimeve);
                    afati.Aktiv = true;

                    db.tblAfati.Add(afati);
                    db.SaveChanges();

                    Session["Mesazhi"] = "Afati u regjistrua me sukses.";
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
            return null;
        }

        public ActionResult ModifikoAfatinGet(int AfatiID)
        {
            var afati = db.tblAfati.Find(AfatiID);
            if (afati == null)
            {
                Session["mesazhi"] = "Afati me kete numer nuk eshte gjetur.";
                return RedirectToAction("index");
            }
            else
            {
                var mbushModelin = new AfatiLista();
                mbushModelin.AfatiID = afati.AfatiID.ToString();
                mbushModelin.Afati = afati.Afati;
                mbushModelin.DataFillimit = afati.DataFillimit.ToString("dd-MM-yyyy");
                mbushModelin.DataMbarimit = afati.DataMbarimit.ToString("dd-MM-yyyy");
                mbushModelin.NumriProvimeve = afati.NumriProvimeve.ToString();
                mbushModelin.Aktiv = afati.Aktiv;

                return PartialView("_ModifikoAfatinGet", mbushModelin);
            }
        }

        public ActionResult ModifikoAfatinPost(AfatiLista modeli)
        {
            try
            {
                var afatiDB = db.tblAfati.Find(int.Parse(modeli.AfatiID));
                if (afatiDB == null)
                {
                    Session["mesazhi"] = "Afati nuk ekziston";
                    return RedirectToAction("Index");
                }
                afatiDB.Afati = modeli.Afati;
                afatiDB.DataFillimit = DateTime.Parse(modeli.DataFillimit);
                afatiDB.DataMbarimit = DateTime.Parse(modeli.DataMbarimit);
                afatiDB.NumriProvimeve = int.Parse(modeli.NumriProvimeve);
                afatiDB.Aktiv = modeli.Aktiv;
                db.Entry(afatiDB).State = EntityState.Modified;
                db.SaveChanges();
                Session["mesazhi"] = "Afati u modifikua me sukses.";

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