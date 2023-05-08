using System.ComponentModel.DataAnnotations;

namespace MVCAssessment2.Models
{
    public class Universities
    {
        [Key]
        public int uniID { get; set; }

        public string universityName { get; set; }

        public int uniRank { get; set; }
    }
}
