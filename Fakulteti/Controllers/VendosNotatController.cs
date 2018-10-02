using Fakulteti.Models.dbKonektimi;
using Fakulteti.Models.Notat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fakulteti.Controllers
{
    public class VendosNotatController : Controller
    {
        FakultetiEntities1 db;
        public VendosNotatController()
        {
            db = new FakultetiEntities1();
        }

        // GET: VendosNotat
        public ActionResult Index()
        {
            if (Session["PerdoruesiID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int? PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());
            int? RoliID = int.Parse(Session["RoliID"].ToString());
            if (RoliID != 3)
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

            var provimet = (from pr in db.tblParaqitjaProvimeve
                            join studenti in db.tblPerdoruesi.Where(x => x.RoliID == 4) on pr.PerdoruesiID equals studenti.PerdoruesiID
                            join pl in db.tblProfesoriLenda.Where(x => x.ProfesoriID == PerdoruesiID) on pr.ProfesoriLendaID equals pl.ProfesoriLendaID
                            join profesori in db.tblPerdoruesi.Where(x => x.RoliID == 3 && x.PerdoruesiID == PerdoruesiID) on pl.ProfesoriID equals profesori.PerdoruesiID
                            join lenda in db.tblLenda on pl.LendaID equals lenda.LendaID
                            join afati in db.tblAfati on pr.AfatiID equals afati.AfatiID
                            join viti in db.tblViti on pr.VitiID equals viti.VitiID
                            select new
                            {
                                ParaqitjaProvimeveID = pr.ParaqitjaProvimeveID,
                                Studenti = studenti.Emri +" " + studenti.Mbiemri + " - " + studenti.PerdoruesiID,
                                Lenda = lenda.Emri,
                                Afati = afati.Afati,
                                Viti = viti.Viti,
                                Nota = pr.Nota,
                              }).ToList();
            List<ParaqitjaProvimeveNota> lista = new List<ParaqitjaProvimeveNota>();
            foreach(var item in provimet)
            {
                lista.Add(new ParaqitjaProvimeveNota()
                {
                    ParaqitjaProvimeveID = item.ParaqitjaProvimeveID,
                    Studenti = item.Studenti,
                    Lenda = item.Lenda,
                    Afati = item.Afati,
                    Viti = item.Viti,
                    Nota = item.Nota == null?"0":item.Nota.ToString(),
                    Statusi = item.Nota == null?"Pa notuar":"Notuar",
                    
                });
            }

            return View(lista);
        }

        public ActionResult VendosNotenGet(int ParaqitjaProvimeveID)
        {
            Session["ParaqitjaProvimeveID"] = ParaqitjaProvimeveID;
            return PartialView("_VendosNotenGet");

        }

        public ActionResult VendosNotenPost(ParaqitjaProvimeveNota modeli)
        {
            int pid = int.Parse(Session["ParaqitjaProvimeveID"].ToString());
            var paraqitjaProvimeve = db.tblParaqitjaProvimeve.Find(pid);
            paraqitjaProvimeve.Nota = int.Parse(modeli.Nota);
            db.Entry(paraqitjaProvimeve).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            Session["Mesazhi"] = "Nota u vendos me sukses.";
            return RedirectToAction("Index");
        }
    }
}