using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MVCAssessment2.Models;
using MVCAssessment2.ViewModels;
using System.Data;

namespace MVCAssessment2.Controllers
{
    [Authorize]
    public class ApplicantController : Controller
    {
        //public Applicant a { get; set; }
        private UserManager<IdentityUser> userManager { get; }

        //public CreateApplicantViewModel b { get; set; }

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

        // GET Edit for when an Applicant wants to edit there application
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);


            var userApplicant = from applicant in _db.applicant
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
                                    Email = user.Email
                                };

            EditApplicantViewModel applicantDetails = new EditApplicantViewModel();

            applicantDetails.PopulateDropDownList(_db);

            foreach (var editApplicant in userApplicant)
            {
                applicantDetails.applicantID = editApplicant.applicantID;
                applicantDetails.firstName = editApplicant.firstName;
                applicantDetails.lastName = editApplicant.lastName;
                applicantDetails.dateOfBirth = editApplicant.dateOfBirth;
                applicantDetails.gpa = editApplicant.gpa;
                applicantDetails.courseID = editApplicant.courseID;
                applicantDetails.uniID = editApplicant.uniID;
                applicantDetails.Id = editApplicant.Id;
                applicantDetails.Email = editApplicant.Email;
            }

            return View(applicantDetails);
        }

        // POST Edit for when an Applicant wants to edit there application
        [HttpPost]
        public async Task<IActionResult> Edit(EditApplicantViewModel applicant)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            applicant.Id = user.Id;

            Console.WriteLine("Logging applicants userID: " + applicant.Id);
            
            _db.applicant.Update(applicant);
            //_db.Entry(applicant).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
                           Email = v4.Email

                       };

            //Console.WriteLine("test");
            List<Combined> cList = new List<Combined>();

            foreach (var a in aArr)
            {
                Applicant a1 = new Applicant { applicantID = a.applicantID, firstName = a.firstName, lastName = a.lastName, dateOfBirth = a.dateOfBirth, gpa = a.gpa, coverLetter = a.coverLetter };
                Courses r1 = new Courses { courseID = a.courseID, courseName = a.courseName };
                Universities u1 = new Universities { uniID = a.uniID, universityName = a.universityName };
                AspNetUsers n1 = new AspNetUsers { Id = a.Id, Email = a.Email };
                Combined c = new Combined { applicant = a1, courses = r1, universities = u1, aspNetUsers = n1 };

                cList.Add(c);
            }
            Console.WriteLine("test");
            return View(cList);
        }

    }
}
