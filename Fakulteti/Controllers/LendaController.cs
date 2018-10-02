using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fakulteti.Models;
using Fakulteti.Models.Lendet;
using Fakulteti.Models.dbKonektimi;
using System.Data.Entity;
using System.Dynamic;

namespace Fakulteti.Controllers
{
    public class LendaController : Controller
    {
        FakultetiEntities1 db;
        public LendaController()
        {
            db = new FakultetiEntities1();
        }

        // GET: Lenda
        public ActionResult Index()
        {

            int? PerdoruesiID = int.Parse(Session["PerdoruesiID"].ToString());

            if(PerdoruesiID==null)
            {
                return RedirectToAction("Login", "Account");
            }
            int? RoliID = int.Parse(Session["RoliID"].ToString());
            if (RoliID != 2)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.RoliID = RoliID;
            if (Session["Mesazhi"] == null )
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
            //var lendet = db.tblProfesoriLenda.ToList();

            var lendet = (from pl in db.tblProfesoriLenda
                          join l in db.tblLenda on pl.LendaID equals l.LendaID
                          join p in db.tblPerdoruesi.Where(x => x.RoliID == 3) on pl.ProfesoriID equals p.PerdoruesiID
                          select new
                          {
                              ProfesoriLendaID = pl.ProfesoriLendaID,
                              LendaID=l.LendaID,
                              ProfesorID=pl.ProfesoriID,
                              Lenda = l.Emri,
                              Pershkrimi = l.Pershkrimi,
                              Profesori = p.Emri + " " + p.Mbiemri
                          }).ToList();
            List<LendaList> modeli = new List<LendaList>();

            foreach(var item in lendet)
            {
                modeli.Add(new LendaList
                {
                    LendaID = item.LendaID,
                    Emri = item.Lenda,
                    Pershkrimi = item.Pershkrimi,
                    Profesori=item.Profesori,
                    ProfesoriID=item.ProfesorID,
                    ProfesoriLendaID = item.ProfesoriLendaID
                });
            }

            return View(modeli);
        }

        public ActionResult RegjistroLendenGet()
        {
            var profesoret = (from p in db.tblPerdoruesi.Where(p=>p.RoliID==3)
                           select new
                           {
                               id = p.PerdoruesiID,
                               pershkrimi = p.Emri + " " + p.Mbiemri,
                           }).ToList();
            ViewBag.Profesoret = new SelectList(profesoret, "id", "pershkrimi");
            return PartialView("_RegjistroLendenGet");
        }

        public ActionResult RegjistroLendenPost(RegjistroLenden modeli)
        {
            //insertimi ne database
            try
            {
                if (ModelState.IsValid)
                {
                    var check = db.tblLenda.Any(x => x.Emri == modeli.Emri);
                    if(check)
                    {
                        var LendaID = db.tblLenda.Where(x => x.Emri == modeli.Emri).FirstOrDefault();
                        var ProfesoriID = int.Parse(modeli.PerdoruesiID);
                        var check2 = db.tblProfesoriLenda.Any(x => x.LendaID == LendaID.LendaID && x.ProfesoriID == ProfesoriID);
                        if(check2)
                        {
                            Session["Mesazhi"] = "Lenda me kete profesor, ekziston ne sistem!";
                            return RedirectToAction("Index");
                        }
                        else
                        {

                            var profesoriLenda1 = new tblProfesoriLenda();
                            profesoriLenda1.ProfesoriID = int.Parse(modeli.PerdoruesiID);
                            profesoriLenda1.LendaID = LendaID.LendaID;
                            db.tblProfesoriLenda.Add(profesoriLenda1);
                            db.SaveChanges();



                            Session["Mesazhi"] = "Lenda u regjistrua me sukses.";
                            Session["Check"] = "1";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        var lenda = new tblLenda();
                        lenda.Emri = modeli.Emri;
                        lenda.Pershkrimi = modeli.Pershkrimi;
                        db.tblLenda.Add(lenda);
                        db.SaveChanges();

                        var profesoriLenda = new tblProfesoriLenda();
                        profesoriLenda.ProfesoriID = int.Parse(modeli.PerdoruesiID);
                        profesoriLenda.LendaID = lenda.LendaID;
                        db.tblProfesoriLenda.Add(profesoriLenda);
                        db.SaveChanges();



                        Session["Mesazhi"] = "Lenda u regjistrua me sukses.";
                        Session["Check"] = "1";
                        return RedirectToAction("Index");
                    }
                   
                }
                else
                {
                    Session["Mesazhi"] = "Te dhenat jo-valide.";
                    Session["Check"] = "2";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Session["Mesazhi"] = "Ka ndodhur nje gabim.";
                Session["Check"] = "3";
                return RedirectToAction("Index");
            }

        }

        public ActionResult ModifikoLendenGet(int LendaID, int ProfesoriID, int ProfesoriLendaID)
        {
            Session["L"] = LendaID;
            var lenda = db.tblLenda.Find(LendaID);
            var profesoret = (from p in db.tblPerdoruesi.Where(p=>p.RoliID==3)
                           select new
                           {
                               ProfesoriID = p.PerdoruesiID,
                               EmriMbiemri = p.Emri + " " + p.Mbiemri
                           }).ToList();

            ViewBag.Profesoret = new SelectList(profesoret, "ProfesoriID", "EmriMbiemri");

            if (lenda == null)
            {
                Session["mesazhi"] = "Lenda me kete numer nuk eshte gjetur.";
                return RedirectToAction("index");
            }
            else
            {
                var mbushModelin = new LendaList();
                mbushModelin.LendaID = lenda.LendaID;
                mbushModelin.Emri = lenda.Emri;
                mbushModelin.Pershkrimi = lenda.Pershkrimi;
                mbushModelin.ProfesoriID = ProfesoriID;
                mbushModelin.ProfesoriLendaID = ProfesoriLendaID;

                return PartialView("_ModifikoLendenGet", mbushModelin);
            }
        }

        public ActionResult ModifikoLendenPost(LendaList modeli)
        {
            try
            {
                //var LendaID=int.Parse(Session[])
                var t = db.tblLenda.FirstOrDefault(x => x.Emri == modeli.Emri);
                if(t==null)
                {
                    var lendaDB = db.tblLenda.Find(modeli.LendaID);
                    if (lendaDB == null)
                    {
                        Session["mesazhi"] = "Lenda nuk ekziston";
                        return RedirectToAction("Index");
                    }
                    lendaDB.Emri = modeli.Emri;
                    lendaDB.Pershkrimi = modeli.Pershkrimi;
                    db.Entry(lendaDB).State = EntityState.Modified;
                    db.SaveChanges();

                    var lendaProfesoriDB = db.tblProfesoriLenda.Find(modeli.ProfesoriLendaID);
                    lendaProfesoriDB.ProfesoriID = modeli.ProfesoriID;
                    db.Entry(lendaProfesoriDB).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["mesazhi"] = "Lenda u modifikua me sukses.";

                    return RedirectToAction("Index");
                }
                else
                {
                    var check2 = db.tblProfesoriLenda.Any(x => x.ProfesoriID == modeli.ProfesoriID && x.LendaID == t.LendaID);
                    if (check2)
                    {
                        Session["Mesazhi"] = "Lenda me kete profesor, ekziston ne sistem!";
                        return RedirectToAction("Index");
                    }
                    var lendaDB = db.tblLenda.Find(modeli.LendaID);
                    if (lendaDB == null)
                    {
                        Session["mesazhi"] = "Lenda nuk ekziston";
                        return RedirectToAction("Index");
                    }
                    lendaDB.Emri = modeli.Emri;
                    lendaDB.Pershkrimi = modeli.Pershkrimi;
                    db.Entry(lendaDB).State = EntityState.Modified;
                    db.SaveChanges();

                    var lendaProfesoriDB = db.tblProfesoriLenda.Find(modeli.ProfesoriLendaID);
                    lendaProfesoriDB.ProfesoriID = modeli.ProfesoriID;
                    db.Entry(lendaProfesoriDB).State = EntityState.Modified;
                    db.SaveChanges();
                    Session["mesazhi"] = "Lenda u modifikua me sukses.";

                    return RedirectToAction("Index");
                }
                
                    

               
            }
            catch (Exception e)
            {
                Session["Mesazhi"] = "Ka ndodhur nje gabim. " + e.Message;
               
                return RedirectToAction("Index");
            }
        }
    }
}