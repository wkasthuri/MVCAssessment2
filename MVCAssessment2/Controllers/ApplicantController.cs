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

        [HttpGet]
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
                Applicant a1 = new Applicant { applicantID= a.applicantID, firstName = a.firstName, lastName = a.lastName, dateOfBirth = a.dateOfBirth, gpa = a.gpa, coverLetter = a.coverLetter };
                Courses r1 = new Courses { courseID= a.courseID, courseName = a.courseName };
                Universities u1 = new Universities { uniID=a.uniID, universityName = a.universityName };
                AspNetUsers n1 = new AspNetUsers { Id=a.Id, Email = a.Email };
                Combined c = new Combined { applicant = a1, courses = r1, universities = u1, aspNetUsers = n1 };

                cList.Add(c);
            }
            Console.WriteLine("test");
            return View(cList);
           
        }
           

        [HttpGet]
        public IActionResult Edit(string applicantID)
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
                           Email = user.Email

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
                AspNetUsers n1 = new AspNetUsers { Id = b.Id, Email = b.Email };
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
               
            }
                      
            return View(fillApplication);
        }



        [HttpPost]
        public async Task <IActionResult> Edit(EditApplicantViewModel applicant)
        {
            // Get the currently logged in users email
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            //Console.WriteLine(user.Id);
            applicant.Id = user.Id;
            _db.Entry(applicant).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Display");
            
        }

        /*
       //Edit using Combined Model
        [HttpGet]
       public IActionResult Edit(string applicantID)
       {
           var aArr = from v1 in _db.applicant
                      join v2 in _db.courses on v1.courseID equals v2.courseID
                      join v3 in _db.universities on v1.uniID equals v3.uniID
                      join v4 in _db.aspNetUsers on v1.Id equals v4.Id

                      select new
                      {    
                          applicantID = v1.applicantID,
                          firstName = v1.firstName,
                          lastName = v1.lastName,
                          dateOfBirth = v1.dateOfBirth,
                          gpa = v1.gpa,

                          courseID = v2.courseID,
                          courseName = v2.courseName,

                          uniID = v3.uniID,
                          universityName = v3.universityName,

                          Id = v4.Id,
                          Email = v4.Email

                      };


           Combined c = null;
           //List<Combined> cList = new List<Combined>();

           foreach (var a in aArr)
           {
               Applicant a1 = new Applicant { applicantID= a.applicantID, firstName = a.firstName, lastName = a.lastName, dateOfBirth = a.dateOfBirth, gpa = a.gpa };
               Courses r1 = new Courses { courseID= a.courseID, courseName = a.courseName };
               Universities u1 = new Universities { uniID = a.uniID, universityName = a.universityName };
               AspNetUsers n1 = new AspNetUsers { Id = a.Id, Email = a.Email };
               c = new Combined { applicant = a1, courses = r1, universities = u1, aspNetUsers = n1 };

               //cList.Add(c);
           }
           return View(c);
       }
       */

    }
}
