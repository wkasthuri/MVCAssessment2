using System.ComponentModel.DataAnnotations;

namespace MVCAssessment2.Models
{
    public class Applicant
    {
        [Key]
        public int? applicantID { get; set; }

        [Display(Name = "First Name")]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime dateOfBirth { get; set; }

        [Display(Name = "GPA")]
        public double gpa { get; set; }

        public int uniID { get; set; }

        public int courseID { get; set; }

        public string? coverLetter { get; set; }

        public string? Id { get; set; }
    }
}
