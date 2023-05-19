using Microsoft.AspNetCore.Mvc;
using MVCAssessment2.Models;

namespace MVCAssessment2.Controllers
{
    public class ApplicantController : Controller
    {
        public Applicant a { get; set; }

        private readonly CSIROContext _db;

        public ApplicantController(CSIROContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Display(AspNetUsers aspNetUsers)
        {
            var aArr = from v1 in _db.applicant
                       join v2 in _db.courses on v1.courseID equals v2.courseID
                       join v3 in _db.universities on v1.uniID equals v3.uniID
                       join v4 in _db.aspNetUsers on v1.Id equals v4.Id

                       select new
                       {
                           firstName1 = v1.firstName,
                           lastName1 = v1.lastName,
                           dateOfBirth1 = v1.dateOfBirth,
                           gpa1 = v1.gpa,
                           courseName1 = v2.courseName,
                           universityName1 = v3.universityName,
                           Email = v4.Email

                       };


            List<Combined> cList = new List<Combined>();

            foreach (var a in aArr)
            {
                Applicant a1 = new Applicant { firstName = a.firstName1, lastName = a.lastName1, dateOfBirth = a.dateOfBirth1, gpa = a.gpa1 };
                Courses r1 = new Courses { courseName = a.courseName1 };
                Universities u1 = new Universities { universityName = a.universityName1 };
                AspNetUsers n1 = new AspNetUsers { Email = a.Email };
                Combined c = new Combined { applicant = a1, courses = r1, universities = u1, aspNetUsers = n1 };

                cList.Add(c);
            }
            return View(cList);
        }
    }
}
