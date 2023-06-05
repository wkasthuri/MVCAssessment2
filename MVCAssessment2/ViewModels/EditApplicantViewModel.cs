using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAssessment2.Models;

namespace MVCAssessment2.ViewModels
{
    public class EditApplicantViewModel : Applicant
    {
        // Course dropdown
        public List<SelectListItem>? courseSelectList { get; set; }

        // Uni dropdown
        public List<SelectListItem>? uniSelectList { get; set; }
       
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
