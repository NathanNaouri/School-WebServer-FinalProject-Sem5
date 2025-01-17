﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FinalProject_1933112.Models;

namespace FinalProject_1933112.Controllers
{
    public class TenantsController : Controller
    {
        private PropertyManagementDBEntities3 db = new PropertyManagementDBEntities3();

        // GET: Tenants
        public ActionResult Index()
        {
            var tenants = db.Tenants.Include(t => t.Manager);
            //return View(tenants.ToList());
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Index(Tenant model)
        {
            using (PropertyManagementDBEntities3 db = new PropertyManagementDBEntities3())
            {
                bool isValidUser = db.Tenants.Any(user => user.firstName.ToLower() ==
                model.firstName.ToLower() && user.password == model.password);
                if (isValidUser)
                {
                    FormsAuthentication.SetAuthCookie(model.firstName, false);

                    var fname = model.firstName;

                    var test = db.Tenants.FirstOrDefault(p => p.firstName == fname);
                    var test2 = test.tenantID;

                    string x = "Details/" + test2.ToString();
                    return RedirectToAction(x, "Tenants");
                }
                ModelState.AddModelError("", "Invalid username or password !");
                return View();
            }
        }




        public ActionResult ListTenants()
        {
            var tenants = db.Tenants.Include(m => m.Manager);
            return View(tenants.ToList());

        }

        // GET: Tenants/Details/5
        public ActionResult Details(int? id)
        {



            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }


            return View(tenant);
        }

        // GET: Tenants/Create
        public ActionResult Create()
        {
           
           
            ViewBag.managerID = new SelectList(db.Managers, "ManagerID", "FirstName");
            return View();
           
        }

        // POST: Tenants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tenantID, firstName,lastName,password, managerID")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
               
                db.Tenants.Add(tenant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.managerID = new SelectList(db.Managers, "ManagerID", "FirstName", tenant.managerID);
            return View(tenant);
        }

        // GET: Tenants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            ViewBag.managerID = new SelectList(db.Managers, "ManagerID", "FirstName", tenant.managerID);
            return View(tenant);
        }

        // POST: Tenants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tenantID,firstName,lastName,password,managerID")] Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tenant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.managerID = new SelectList(db.Managers, "ManagerID", "FirstName", tenant.managerID);
            return View(tenant);
        }

        // GET: Tenants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        // POST: Tenants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tenant tenant = db.Tenants.Find(id);
            db.Tenants.Remove(tenant);
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
