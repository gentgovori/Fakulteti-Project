using Fakulteti.Models.dbKonektimi;
using Fakulteti.Models.DepartamentiList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fakulteti.Controllers
{
    public class DepartamentiController : Controller
    {
        FakultetiEntities1 db;
        public DepartamentiController()
        {
            db = new FakultetiEntities1();
        }
        // GET: Departamenti
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
            var departamenti = db.tblDepartamenti.ToList();
            List<DepartamentLista> lista = new List<DepartamentLista>();
            foreach (var item in departamenti)
            {
                lista.Add(new DepartamentLista
                {
                    DepartamentiID = item.DepartamentiID,
                    Departamenti = item.Departamenti
                });
            }

            return View(lista);
        }

        public ActionResult RegjistroDepartamentinGet()
        {
            return PartialView("_RegjistroDepartamentinGet");
        }


        public ActionResult RegjistroDepartamentinPost(RegjistroDepartamentin modeli)
        {
            //insertimi ne database
            try
            {
                if (ModelState.IsValid)
                {
                    var departamenti = new tblDepartamenti();

                    departamenti.Departamenti = modeli.Departamenti;
                    db.tblDepartamenti.Add(departamenti);
                    db.SaveChanges();

                    Session["Mesazhi"] = "Departamenti u regjistrua me sukses.";
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

        public ActionResult ModifikoDepartamentinGet(int DepartamentiID)
        {
            var departamenti = db.tblDepartamenti.Find(DepartamentiID);
            if (departamenti == null)
            {
                Session["mesazhi"] = "Departamenti me kete numer nuk eshte gjetur.";
                return RedirectToAction("index");
            }
            else
            {
                var mbushModelin = new DepartamentLista();
                mbushModelin.DepartamentiID = departamenti.DepartamentiID;
                mbushModelin.Departamenti = departamenti.Departamenti;
                return PartialView("_ModifikoDepartamentinGet", mbushModelin);
            }

        }

        public ActionResult ModifikoDepartamentinPost(DepartamentLista modeli)
        {
            try
            {
                var departamentiDB = db.tblDepartamenti.Find(modeli.DepartamentiID);
                if (departamentiDB == null)
                {
                    Session["mesazhi"] = "Departamenti i tille nuk ekziston";
                    return RedirectToAction("Index");
                }

                departamentiDB.Departamenti = modeli.Departamenti;
                db.Entry(departamentiDB).State = EntityState.Modified;
                db.SaveChanges();

                Session["mesazhi"] = "Departamenti u modifikua me sukses.";

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