using System.ComponentModel.DataAnnotations;

namespace MVCAssessment2.Models
{
    public class Courses
    {
        [Key]
        public int courseID { get; set; }

        public string courseName { get; set; }
    }
}
