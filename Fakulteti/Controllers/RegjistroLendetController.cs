using Fakulteti.Models.dbKonektimi;
using Fakulteti.Models.StudentiLendet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fakulteti.Controllers
{
    public class RegjistroLendetController : Controller
    {
        FakultetiEntities1 db;
        public RegjistroLendetController()
        {
            db = new FakultetiEntities1();
        }

        public ActionResult Index()
        {
            #region
            if (Session["PerdoruesiID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int? PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());

          
            int? RoliID = int.Parse(Session["RoliID"].ToString());
            if (RoliID != 4)
            {
                Session["PerdoruesiID"] = null;
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
            #endregion

            var lendetRegjistruara = (from sl in db.tblStudentiLenda.Where(x => x.StudentiID == PerdoruesiID)
                                      join pl in db.tblProfesoriLenda on sl.ProfesoriLendaID equals pl.ProfesoriLendaID
                                      join l in db.tblLenda on pl.LendaID equals l.LendaID 
                                      join p in db.tblPerdoruesi.Where(x => x.RoliID == 3) on pl.ProfesoriID equals p.PerdoruesiID
                                      select new
                                      {
                                          StudentiLendaID=sl.StudentiLendaID,
                                          ProfesoriLendaID=pl.ProfesoriLendaID,
                                          Lenda=l.Emri,
                                          Profesori=p.Emri+" "+p.Mbiemri
                                      }).ToList();
            List<StudentiLendetLista> lista = new List<StudentiLendetLista>();
            foreach(var item in lendetRegjistruara)
            {
                lista.Add(new StudentiLendetLista()
                {
                    StudentiLendaID=item.StudentiLendaID,
                    ProfesoriLendaID=item.ProfesoriLendaID,
                    Lenda=item.Lenda,
                    Profesori=item.Profesori
                });
            }
            return View(lista);
        }

        public ActionResult RegjistroLendetGet()
        {

            var lendetProfesori1 = (from pl in db.tblProfesoriLenda
                                   join l in db.tblLenda on pl.LendaID equals l.LendaID
                                   join p in db.tblPerdoruesi.Where(x => x.RoliID == 3) on pl.ProfesoriID equals p.PerdoruesiID
                                   select new
                                   {
                                       id = pl.ProfesoriLendaID,
                                       pershkrimi = l.Emri + " - " + p.Emri + " " + p.Mbiemri
                                   }).ToList();
            int studenti = (int)Session["PerdoruesiID"];
            var lendetProfesori2 = (from pl in db.tblProfesoriLenda
                                   join l in db.tblLenda on pl.LendaID equals l.LendaID
                                   join p in db.tblPerdoruesi.Where(x => x.RoliID == 3) on pl.ProfesoriID equals p.PerdoruesiID
                                   join z in db.tblStudentiLenda.Where(x=> x.StudentiID==studenti) on pl.ProfesoriLendaID equals z.ProfesoriLendaID
                                   select new
                                   {
                                       id = pl.ProfesoriLendaID,
                                       pershkrimi = l.Emri + " - " + p.Emri + " " + p.Mbiemri
                                   }).ToList();
            foreach (var item in lendetProfesori2)
            {
                lendetProfesori1.Remove(item);
            }
            //TODO: me kry validimin 
            ViewBag.LendetProfesori = new SelectList(lendetProfesori1, "id", "pershkrimi");



            return PartialView("_RegjistroLendetGet");
        }

        [HttpPost]
        public ActionResult RegjistroLendetStudentiPost(RegjistroLendetStudenti modeli)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var lendetStudenti = new tblStudentiLenda();
                    int StudentiID = int.Parse(Session["PerdoruesiID"].ToString());
                    lendetStudenti.StudentiID = StudentiID;
                    lendetStudenti.ProfesoriLendaID = modeli.ProfesoriLendaID;
                    db.tblStudentiLenda.Add(lendetStudenti);
                    db.SaveChanges();

                    Session["Mesazhi"] = "Lenda u regjistrua me sukses.";
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

        public ActionResult FshijLenden(int StudentiLendaID)
        {

            var sl = db.tblStudentiLenda.Find(StudentiLendaID);
            if (sl!=null)
            {

                db.tblStudentiLenda.Remove(sl);
                db.SaveChanges();
                Session["Mesazhi"] = "Lenda u fshi me sukses!";
                return RedirectToAction("Index");
            }
            else
            {
                Session["Mesazhi"] = "Lenda nuk u gjet!";
                return RedirectToAction("Index");
            }
        }

    }
}