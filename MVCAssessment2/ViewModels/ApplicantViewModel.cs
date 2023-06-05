using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAssessment2.Models;
using System.ComponentModel.DataAnnotations;

namespace MVCAssessment2.ViewModels
{
    public class ApplicantViewModel : Applicant
    {
        // Course dropdown
        public List<SelectListItem>? courseSelectList { get; set; }

        // Uni dropdown
        public List<SelectListItem>? uniSelectList { get; set; }

        [Required]
        public string PhoneNumber { get; set; }


        public ApplicantViewModel PopulateDropDownList(CSIROContext _db)
        {
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

            // Init the course dropdown list
            this.courseSelectList = new List<SelectListItem>();
            // Init the university dropdown list
            this.uniSelectList = new List<SelectListItem>();

            // Loop through the course and add them to the viewmodel
            foreach (var course in courses)
            {
                this.courseSelectList.Add(new SelectListItem { Text = course.courseName, Value = course.courseID.ToString() });
            }

            // Loop through the universitys results and add them to the viewmodel
            foreach (var uni in universities)
            {
                this.uniSelectList.Add(new SelectListItem { Text = uni.uniName, Value = uni.uniID.ToString() });
            }

            return this;
        }
    }
}
