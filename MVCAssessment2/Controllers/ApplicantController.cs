using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

            user.PhoneNumber = applicant.PhoneNumber;
            await userManager.UpdateAsync(user);

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
        public async Task<IActionResult> Edit()
        {
            // Get the currently logged in users email
            var loggedUser = await userManager.FindByNameAsync(User.Identity.Name);

            var userApplicant = from applicant in _db.applicant
                                join user in _db.aspNetUsers on applicant.Id equals user.Id
                                where applicant.Id == loggedUser.Id
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

            // Init fillViewModel for autofill and dropdowns
            EditApplicantViewModel applicantDetails = new EditApplicantViewModel();

            applicantDetails.PopulateDropDownList(_db);

            foreach (var item in userApplicant)
            {
                applicantDetails.applicantID = item.applicantID;
                applicantDetails.firstName = item.firstName;
                applicantDetails.lastName = item.lastName;
                applicantDetails.dateOfBirth = item.dateOfBirth;
                applicantDetails.gpa = item.gpa;
                applicantDetails.coverLetter = item.coverLetter;
                applicantDetails.courseID = item.courseID;
                applicantDetails.uniID = item.uniID;
                applicantDetails.Id = item.Id;
                applicantDetails.Email = item.Email;
                applicantDetails.PhoneNumber = item.PhoneNumber;

            }
            return View(applicantDetails);
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
            return RedirectToAction("Index", "Home");
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
