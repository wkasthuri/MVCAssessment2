using System.ComponentModel.DataAnnotations;

namespace MVCAssessment2.Models
{
    public class Universities
    {
        [Key]
        public int uniID { get; set; }

        [Display(Name = "University")]
        public string universityName { get; set; }
    }
}
