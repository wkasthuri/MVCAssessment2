using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCAssessment2.ViewModels
{
    public class FillViewModel
    {
        //public string firstName { get; set; }
        //public string lastName { get; set; }
        //public DateTime dateOfBirth { get; set; }

        public string email { get; set; }

        //public int gpa { get; set; }

        // Course dropdown
        public string selectedCourse { get; set; }
        public List<SelectListItem> courseSelectList { get; set; }

        // Uni dropdown
        public string selectedUniversity { get; set; }
        public List<SelectListItem> uniSelectList { get; set; }
    }
}