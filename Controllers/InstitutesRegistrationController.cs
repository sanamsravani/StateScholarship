using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class InstitutesRegistrationController : Controller
    {
        private InstitutionDBEntities db = new InstitutionDBEntities();

       
        

        // GET: InstitutesRegistration/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,password,Name,Address,Course,Scholarship,Category")] Institute institute)
        {
            var checkId = db.Institutes.Where(x => x.Id == institute.Id).FirstOrDefault();
            {
                if (checkId == null)
                {
                    if (ModelState.IsValid)
                    {
                        db.Institutes.Add(institute);
                        db.SaveChanges();
                        ModelState.Clear();
                        ViewBag.SuccessMessage = "New Institution is Added Successfully";
                    }
                }
                else
                {
                    ViewBag.Message = "Institute Id already exists";
                    return View();
                }

                return View(institute);
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Institute institute)
        {
            var credentialCheck = db.Institutes.Where(x => x.Id.Equals(institute.Id)).FirstOrDefault();
            var Password =db.Institutes.Where(x=>x.password.Equals(institute.password)).FirstOrDefault();

            if (credentialCheck != null && Password !=null)
            {
                Session["Name"] = credentialCheck.Name.ToString();
                Session["Id"] = credentialCheck.Id.ToString();

                return RedirectToAction("Welcome");
            }
            if (credentialCheck == null && Password != null)
            {
                ViewBag.Message = "Id is not present";
            }
            if (Password == null && credentialCheck != null)
            {
                ViewBag.Message = "Password not matching";
            }


            return View(institute);
        }
        public ActionResult Welcome()
        {
            return View();
        }
        public ActionResult UpdateDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateDetails(Institute institute)
        {
            var searchRecord = db.Institutes.Where(x => x.Id == institute.Id).FirstOrDefault();
            searchRecord.Name = institute.Name;
            searchRecord.Address = institute.Address;
            searchRecord.password = institute.password;
            searchRecord.Course = institute.Course;
            searchRecord.Scholarship = institute.Scholarship;
            searchRecord.Category = institute.Category;
            db.SaveChanges();
            return RedirectToAction("Login");
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
