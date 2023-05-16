using System.ComponentModel.DataAnnotations;

namespace MVCAssessment2.Models
{
    public class Courses
    {
        [Key]
        public int courseID { get; set; }

        [Display(Name = "Course")]
        public string courseName { get; set; }
    }
}
