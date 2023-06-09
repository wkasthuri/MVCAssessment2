using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MVCAssessment2.Models
{
    public class Applicant
    {
        [Key]
        public int applicantID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateOfBirth { get; set; }

        [Required]
        [Display(Name = "GPA")]
        [DisplayFormat(NullDisplayText = "")]
        public double gpa { get; set; }

        [Required]
        [Display(Name = "University Name")]
        public int uniID { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        public int courseID { get; set; }

        [Display(Name = "Cover Letter (Optional)")]
        public string? coverLetter { get; set; }

        public string? Id { get; set; }
    }
}
