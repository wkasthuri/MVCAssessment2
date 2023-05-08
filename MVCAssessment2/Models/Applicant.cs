using System.ComponentModel.DataAnnotations;

namespace MVCAssessment2.Models
{
    public class Applicant
    {
        [Key]
        public int applicantID { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public DateTime dateOfBirth { get; set; }

        public float gpa { get; set; }

        public int uniID { get; set; }

        public int courseID { get; set; }

        public int appID { get; set; }

        public int userID { get; set; }
    }
}
