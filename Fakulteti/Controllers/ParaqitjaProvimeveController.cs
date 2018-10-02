using Fakulteti.Models.dbKonektimi;
using Fakulteti.Models.ParaqitjaProvimeveModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fakulteti.Controllers
{
    public class ParaqitjaProvimeveController : Controller
    {
        FakultetiEntities1 db;
        public ParaqitjaProvimeveController()
        {
            db = new FakultetiEntities1();
        }


        // GET: ParaqitjaProvimeve
        public ActionResult Index()
        {
            if (Session["PerdoruesiID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int? PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
            int? RoliID = int.Parse(Session["RoliID"].ToString());
            if (RoliID != 4)
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
                Session["Mesazhi"] = null;
            }

            var paraqitjaProvimeve = (from pr in db.tblParaqitjaProvimeve.Where(x => x.PerdoruesiID == PerdoruesiID)
                                      join a in db.tblAfati.Where(x=>x.Aktiv==true) on pr.AfatiID equals a.AfatiID
                                      join v in db.tblViti on pr.VitiID equals v.VitiID
                                      join pl in db.tblProfesoriLenda on pr.ProfesoriLendaID equals pl.ProfesoriLendaID
                                      join profesori in db.tblPerdoruesi.Where(x => x.RoliID == 3) on pl.ProfesoriID equals profesori.PerdoruesiID
                                      join l in db.tblLenda on pl.LendaID equals l.LendaID
                                      select new
                                      {
                                          ParaqitjaProvimeveID = pr.ParaqitjaProvimeveID,
                                          Lenda = l.Emri,
                                          Profesori = profesori.Emri + " " + profesori.Mbiemri,
                                          Afati = a.Afati,
                                          Viti = v.Viti
                                      }
                                      ).ToList();
            List<ListaProvimeve> lista = new List<ListaProvimeve>();
            foreach(var item in paraqitjaProvimeve)
            {
                lista.Add(new ListaProvimeve()
                {
                    ParaqitjaProvimeveID = item.ParaqitjaProvimeveID,
                    Lenda = item.Lenda,
                    Profesori = item.Profesori,
                    Afati = item.Afati,
                    Viti = item.Viti
                });
            }

            return View(lista);
        }

        public ActionResult ParaqiteProviminGet()
        {
            int PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
            var afati = (from a in db.tblAfati.Where(x => x.Aktiv == true)
                         select new
                         {
                             id = a.AfatiID,
                             pershkrimi = a.Afati
                         }).ToList();
            ViewBag.Afati = new SelectList(afati, "id", "pershkrimi");

            var profesoriLenda = (from pl in db.tblProfesoriLenda
                                  join sl in db.tblStudentiLenda.Where(x => x.StudentiID == PerdoruesiID) on pl.ProfesoriLendaID equals sl.ProfesoriLendaID
                                  join p in db.tblPerdoruesi.Where(x => x.RoliID == 3) on pl.ProfesoriID equals p.PerdoruesiID
                                  join l in db.tblLenda on pl.LendaID equals l.LendaID
                         select new
                         {
                             id = pl.ProfesoriLendaID,
                             pershkrimi = l.Emri+ " - "+p.Emri+" "+p.Mbiemri
                         }).ToList();
            ViewBag.Lenda = new SelectList(profesoriLenda, "id", "pershkrimi");

            
            var viti = db.tblViti.Where(x=>x.Viti==DateTime.Now.Year).ToList();
            ViewBag.Viti = new SelectList(viti, "VitiID", "Viti");


            return PartialView("_ParaqiteProviminGet");
        }

        public ActionResult ParaqiteProviminPost(ParaqitjaProvimeve modeli)
        {
            //insertimi ne database

            try
            {
                if (ModelState.IsValid)
                {
                    int PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
                    var paraqitjaProvimeve = new tblParaqitjaProvimeve();
                    paraqitjaProvimeve.PerdoruesiID = PerdoruesiID;
                    paraqitjaProvimeve.ProfesoriLendaID = modeli.ProfesoriLendaID;
                    paraqitjaProvimeve.AfatiID = modeli.AfatiID;
                    paraqitjaProvimeve.VitiID = modeli.VitiID;
                    db.tblParaqitjaProvimeve.Add(paraqitjaProvimeve);
                    db.SaveChanges();

                    Session["Mesazhi"] = "Provimi eshte paraqitur me sukses.";
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

        public ActionResult FshijeLendenParaqitur(int ParaqitjaProvimeveID)
        {
            var sl = db.tblParaqitjaProvimeve.Find(ParaqitjaProvimeveID);
            if (sl != null)
            {
                db.tblParaqitjaProvimeve.Remove(sl);
                db.SaveChanges();
                Session["Mesazhi"] = "Lenda u fshi me sukses!";
                return RedirectToAction("Index");
            }
            else
            {
                Session["Mesazhi"] = "Lenda nuk u gjete!";
                return RedirectToAction("Index");
            }
        }
    }
}