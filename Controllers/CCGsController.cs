using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cascadingdropdownlist.DAL;
using Cascadingdropdownlist.Models;

namespace Cascadingdropdownlist.Controllers
{
    public class CCGsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: CCGs
        public ActionResult Index()
        {
            return View(db.CCGs.ToList());
        }

        // GET: CCGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CCG cCG = db.CCGs.Find(id);
            if (cCG == null)
            {
                return HttpNotFound();
            }
            return View(cCG);
        }

        // GET: CCGs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CCGs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CCGID,Title")] CCG cCG)
        {
            if (ModelState.IsValid)
            {
                db.CCGs.Add(cCG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cCG);
        }

        // GET: CCGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CCG cCG = db.CCGs.Find(id);
            if (cCG == null)
            {
                return HttpNotFound();
            }
            return View(cCG);
        }

        // POST: CCGs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CCGID,Title")] CCG cCG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cCG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cCG);
        }

        // GET: CCGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CCG cCG = db.CCGs.Find(id);
            if (cCG == null)
            {
                return HttpNotFound();
            }
            return View(cCG);
        }

        // POST: CCGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CCG cCG = db.CCGs.Find(id);
            db.CCGs.Remove(cCG);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
