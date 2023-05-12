using System.ComponentModel.DataAnnotations;

namespace MVCAssessment2.Models
{
    public class Applications
    {
        [Key]
        public int appID { get; set; }

        public string coverLetter { get; set; }

    }
}
