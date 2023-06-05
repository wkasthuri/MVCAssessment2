﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using MVCAssessment2.Models;
using MVCAssessment2.ViewModels;

namespace MVCAssessment2.Controllers
{
    [Authorize]
    public class ApplicantController : Controller
    {
        //public Applicant a { get; set; }

        private UserManager<IdentityUser> userManager { get; }

        private readonly CSIROContext _db;

        public ApplicantController(CSIROContext db, UserManager<IdentityUser> _userManager)
        {
            _db = db;
            this.userManager = _userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        // GET Add for when a User submits an application
        [HttpGet]
        public IActionResult Add()
        {
            // Init ViewModel
            ApplicantViewModel fillApplication = new ApplicantViewModel();
            // Populate the University and Courses dropdown lists
            fillApplication.PopulateDropDownList(_db);

            // Pass the view model to the view
            return View(fillApplication);
        }

        // Post Add for when a User submits an application
        [HttpPost]
        public async Task<IActionResult> Add(ApplicantViewModel applicant)
        {
            // Init ViewModel
            ApplicantViewModel fillApplication = new ApplicantViewModel();
            // Populate the University and Courses dropdown lists
            fillApplication.PopulateDropDownList(_db);

            // Get the currently logged in users email
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            Console.WriteLine(user.Id);
            applicant.Id = user.Id;

            // Check that the data being submitted is valid
            if (!ModelState.IsValid) return View(fillApplication);


            _db.applicant.Add(applicant);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        // GET Display for Displaying all applicant to the Administrator
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Display()
        {
            var aArr = from v1 in _db.applicant
                       join v2 in _db.courses on v1.courseID equals v2.courseID
                       join v3 in _db.universities on v1.uniID equals v3.uniID
                       join v4 in _db.aspNetUsers on v1.Id equals v4.Id

                       select new
                       {
                           Id = v4.Id,
                           applicantID = v1.applicantID,
                           courseID = v2.courseID,
                           uniID = v3.uniID,
                           firstName = v1.firstName,
                           lastName = v1.lastName,
                           dateOfBirth = v1.dateOfBirth,
                           gpa = v1.gpa,
                           coverLetter = v1.coverLetter,
                           courseName = v2.courseName,
                           universityName = v3.universityName,
                           Email = v4.Email,
                           PhoneNumber = v4.PhoneNumber

                       };

            //Console.WriteLine("test");
            List<Combined> cList = new List<Combined>();

            foreach (var a in aArr)
            {
                Applicant a1 = new Applicant { applicantID = a.applicantID, firstName = a.firstName, lastName = a.lastName, dateOfBirth = a.dateOfBirth, gpa = a.gpa, coverLetter = a.coverLetter };
                Courses r1 = new Courses { courseID = a.courseID, courseName = a.courseName };
                Universities u1 = new Universities { uniID = a.uniID, universityName = a.universityName };
                AspNetUsers n1 = new AspNetUsers { Id = a.Id, Email = a.Email, PhoneNumber = a.PhoneNumber };
                Combined c = new Combined { applicant = a1, courses = r1, universities = u1, aspNetUsers = n1 };

                cList.Add(c);
            }
            Console.WriteLine("test");
            return View(cList);

        }


        [HttpGet]
        public IActionResult Edit(int applicantID)
        {
            var userApplicant = from applicant in _db.applicant
                                join user in _db.aspNetUsers on applicant.Id equals user.Id

                                select new
                                {
                                    applicantID = applicant.applicantID,
                                    firstName = applicant.firstName,
                                    lastName = applicant.lastName,
                                    dateOfBirth = applicant.dateOfBirth,
                                    gpa = applicant.gpa,
                                    coverLetter = applicant.coverLetter,
                                    courseID = applicant.courseID,
                                    uniID = applicant.uniID,
                                    Id = user.Id,
                                    Email = user.Email,
                                    PhoneNumber = user.PhoneNumber

                                };

            EditApplicantViewModel applicantDetails = new EditApplicantViewModel();

            // Fetch course data from db
            var courses = from course in _db.courses
                          select new
                          {
                              courseID = course.courseID,
                              courseName = course.courseName
                          };

            var universities = from university in _db.universities
                               select new
                               {
                                   uniID = university.uniID,
                                   uniName = university.universityName,
                               };

            // Init fillViewModel for autofill and dropdowns
            var fillApplication = new EditApplicantViewModel();

            // Init the course dropdown list
            fillApplication.courseSelectList = new List<SelectListItem>();
            // Init the university dropdown list
            fillApplication.uniSelectList = new List<SelectListItem>();

            // Loop through the course and add them to the viewmodel
            foreach (var course in courses)
            {
                fillApplication.courseSelectList.Add(new SelectListItem { Text = course.courseName, Value = course.courseID.ToString() });
            }

            // Loop through the universitys results and add them to the viewmodel
            foreach (var uni in universities)
            {
                fillApplication.uniSelectList.Add(new SelectListItem { Text = uni.uniName, Value = uni.uniID.ToString() });
            }
            foreach (var b in userApplicant)
            {
                Applicant b1 = new Applicant { applicantID = b.applicantID, firstName = b.firstName, lastName = b.lastName, dateOfBirth = b.dateOfBirth, gpa = b.gpa, coverLetter = b.coverLetter, uniID = b.uniID, courseID = b.courseID };
                AspNetUsers n1 = new AspNetUsers { Id = b.Id, Email = b.Email, PhoneNumber = b.PhoneNumber };
                fillApplication.applicantID = b1.applicantID;
                fillApplication.firstName = b1.firstName;
                fillApplication.lastName = b1.lastName;
                fillApplication.dateOfBirth = b1.dateOfBirth;
                fillApplication.gpa = b1.gpa;
                fillApplication.coverLetter = b1.coverLetter;
                fillApplication.courseID = b1.courseID;
                fillApplication.uniID = b1.uniID;
                fillApplication.Id = n1.Id;
                fillApplication.Email = n1.Email;
                fillApplication.PhoneNumber = n1.PhoneNumber;

            }

            return View(fillApplication);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(EditApplicantViewModel applicant)
        {
            // Get the currently logged in users email
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            user.PhoneNumber = applicant.PhoneNumber;
            await userManager.UpdateAsync(user);
            //Console.WriteLine(applicant.PhoneNumber);
            applicant.Id = user.Id;
            _db.Entry(applicant).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Display");

        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            // Get the currently logged in users email
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            //Console.WriteLine(user.Id);
            var a = from applicant in _db.applicant
                    where applicant.Id == user.Id
                    select new
                            {
                                applicantID = applicant.applicantID,
                                firstName = applicant.firstName,
                                lastName = applicant.lastName,
                                dateOfBirth = applicant.dateOfBirth,
                                gpa = applicant.gpa,
                                courseID = applicant.courseID,
                                uniID = applicant.uniID,
                                Id = user.Id,
                                PhoneNumber = user.PhoneNumber
                            };
            Applicant deleteApplicant = new Applicant();
            
            foreach (var item in a)
            {
                deleteApplicant.applicantID = item.applicantID;
                deleteApplicant.firstName = item.firstName;
                deleteApplicant.lastName = item.lastName;
                deleteApplicant.dateOfBirth = item.dateOfBirth;
                deleteApplicant.gpa = item.gpa;
                deleteApplicant.courseID = item.courseID;
                deleteApplicant.uniID = item.uniID;
                deleteApplicant.Id = item.Id;
            };
            //Console.WriteLine(deleteApplicant);
            
            _db.Entry(deleteApplicant).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

            _db.SaveChanges();
            return RedirectToAction("Display");

        }

        
    }
}
