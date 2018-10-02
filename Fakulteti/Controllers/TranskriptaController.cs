using Fakulteti.Helpers;
using Fakulteti.Models.dbKonektimi;
using Fakulteti.Models.Notat;
using Fakulteti.Models.RaportetModel;
using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fakulteti.Controllers
{
    public class TranskriptaController : Controller
    {
        FakultetiEntities1 db;
        public TranskriptaController()
        {
            db = new FakultetiEntities1();
        }

        // GET: Salla
        public ActionResult Index()
        {
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

            var provimet = (from pr in db.tblParaqitjaProvimeve.Where(x=>x.Nota>5)
                            join studenti in db.tblPerdoruesi.Where(x => x.RoliID == 4 && x.PerdoruesiID == PerdoruesiID) on pr.PerdoruesiID equals studenti.PerdoruesiID
                            join pl in db.tblProfesoriLenda on pr.ProfesoriLendaID equals pl.ProfesoriLendaID
                            join profesori in db.tblPerdoruesi.Where(x => x.RoliID == 3 ) on pl.ProfesoriID equals profesori.PerdoruesiID
                            join lenda in db.tblLenda on pl.LendaID equals lenda.LendaID
                            join afati in db.tblAfati on pr.AfatiID equals afati.AfatiID
                            join viti in db.tblViti on pr.VitiID equals viti.VitiID
                            select new
                            {
                                ParaqitjaProvimeveID = pr.ParaqitjaProvimeveID,
                                Studenti = studenti.Emri + " " + studenti.Mbiemri + " - " + studenti.PerdoruesiID,
                                Lenda = lenda.Emri,
                                Afati = afati.Afati,
                                Viti = viti.Viti,
                                Nota = pr.Nota,
                                Profesori = profesori.Emri + " " + profesori.Mbiemri
                            }).ToList();
            List<ParaqitjaProvimeveNota> lista = new List<ParaqitjaProvimeveNota>();
            foreach (var item in provimet)
            {
                lista.Add(new ParaqitjaProvimeveNota()
                {
                    ParaqitjaProvimeveID = item.ParaqitjaProvimeveID,
                    Lenda = item.Lenda,
                    Afati = item.Afati,
                    Viti = item.Viti,
                    Nota = item.Nota == null ? "0" : item.Nota.ToString(),
                    Statusi = item.Nota == null ? "Pa notuar" : "Notuar",
                    Profesori = item.Profesori
                });
            }
            Session["lista"] = lista;
            return View(lista);
        }

        public ActionResult ListaRezultateve()
        {
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

            var provimet = (from pr in db.tblParaqitjaProvimeve
                            join studenti in db.tblPerdoruesi.Where(x => x.RoliID == 4 && x.PerdoruesiID == PerdoruesiID) on pr.PerdoruesiID equals studenti.PerdoruesiID
                            join pl in db.tblProfesoriLenda on pr.ProfesoriLendaID equals pl.ProfesoriLendaID
                            join profesori in db.tblPerdoruesi.Where(x => x.RoliID == 3) on pl.ProfesoriID equals profesori.PerdoruesiID
                            join lenda in db.tblLenda on pl.LendaID equals lenda.LendaID
                            join afati in db.tblAfati on pr.AfatiID equals afati.AfatiID
                            join viti in db.tblViti on pr.VitiID equals viti.VitiID
                            select new
                            {
                                ParaqitjaProvimeveID = pr.ParaqitjaProvimeveID,
                                Studenti = studenti.Emri + " " + studenti.Mbiemri + " - " + studenti.PerdoruesiID,
                                Lenda = lenda.Emri,
                                Afati = afati.Afati,
                                Viti = viti.Viti,
                                Nota = pr.Nota,
                                Profesori = profesori.Emri + " " + profesori.Mbiemri
                            }).ToList();
            List<ParaqitjaProvimeveNota> lista = new List<ParaqitjaProvimeveNota>();
            foreach (var item in provimet)
            {
                lista.Add(new ParaqitjaProvimeveNota()
                {
                    ParaqitjaProvimeveID = item.ParaqitjaProvimeveID,
                    Lenda = item.Lenda,
                    Afati = item.Afati,
                    Viti = item.Viti,
                    Nota = item.Nota == null ? "0" : item.Nota.ToString(),
                    Statusi = item.Nota == null ? "Pa notuar" : "Notuar",
                    Profesori = item.Profesori
                });
            }

            return View(lista);
        }

        public ActionResult KartelaStudentit()
        {
            int ID = int.Parse(Session["PerdoruesiID"].ToString());
            var studenti = db.tblPerdoruesi.Where(x => x.PerdoruesiID == ID).FirstOrDefault();
            var notaMesatare = db.tblParaqitjaProvimeve.Where(x => x.PerdoruesiID == ID && x.Nota != null && x.Nota > 5).Average(x => x.Nota);



            List<ParaqitjaProvimeveNota> listaFinale = (List<ParaqitjaProvimeveNota>)Session["lista"];
            var reportPath = Request.MapPath(Request.ApplicationPath) + @"\\Raportet\rptTranskriptaNotave.rdlc";
            List<Statike> ls = new List<Statike>();
            var s = new Statike();
            s.Studenti = "Studenti: " + studenti.Emri + " " + studenti.Mbiemri;
            s.ID = "ID: " + ID.ToString();
            s.Data = "Data: " + DateTime.Now.ToShortDateString();
            s.NotaMesatare = "Nota mesatare: " + notaMesatare.ToString();
            ls.Add(s);
            DataTable tabela = new DataTable();
            DataTable dtStatike = new DataTable();
            using (var reader = ObjectReader.Create(listaFinale, null))
            {
                tabela.Load(reader);
            }
            using (var reader = ObjectReader.Create(ls, null))
            {
                dtStatike.Load(reader);
            }

            var listaMeDataSeta = new List<DataTable>();
            listaMeDataSeta.Add(tabela);
            listaMeDataSeta.Add(dtStatike);
            var reportGenerator = new GjenerimiRaporteve();
            var bajtat = reportGenerator.GjeneroRaportin(GjenerimiRaporteve.LlojiRaportit.rptTranskriptaNotave, reportPath, listaMeDataSeta);
            return File(bajtat, "application/pdf");

        }
    }
}