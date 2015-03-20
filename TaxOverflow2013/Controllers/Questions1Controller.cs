using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaxOverflow2013.Models;

namespace TaxOverflow2013.Controllers
{
    public class Questions1Controller : Controller
    {
        private TestTODBEntities db = new TestTODBEntities();

        // GET: Questions1
        public ActionResult Index()
        {
            var questions1 = db.Questions1.Include(q => q.Categories1).Include(q => q.User);
            return View(questions1.ToList());
        }

        // GET: Questions1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions1 questions1 = db.Questions1.Find(id);
            if (questions1 == null)
            {
                return HttpNotFound();
            }
            return View(questions1);
        }

        // GET: Questions1/Create
        public ActionResult Create()
        {
            ViewBag.Category_CategoryID = new SelectList(db.Categories1, "CategoryID", "Category");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        // POST: Questions1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuestionID,Question,Score,QuestionDTS,UserID,Category_CategoryID")] Questions1 questions1)
        {
            if (ModelState.IsValid)
            {
                db.Questions1.Add(questions1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_CategoryID = new SelectList(db.Categories1, "CategoryID", "Category", questions1.Category_CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", questions1.UserID);
            return View(questions1);
        }

        // GET: Questions1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions1 questions1 = db.Questions1.Find(id);
            if (questions1 == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_CategoryID = new SelectList(db.Categories1, "CategoryID", "Category", questions1.Category_CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", questions1.UserID);
            return View(questions1);
        }

        // POST: Questions1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionID,Question,Score,QuestionDTS,UserID,Category_CategoryID")] Questions1 questions1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questions1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_CategoryID = new SelectList(db.Categories1, "CategoryID", "Category", questions1.Category_CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", questions1.UserID);
            return View(questions1);
        }

        // GET: Questions1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions1 questions1 = db.Questions1.Find(id);
            if (questions1 == null)
            {
                return HttpNotFound();
            }
            return View(questions1);
        }

        // POST: Questions1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Questions1 questions1 = db.Questions1.Find(id);
            db.Questions1.Remove(questions1);
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
