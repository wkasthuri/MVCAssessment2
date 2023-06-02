using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAssessment2.Models;

namespace MVCAssessment2.ViewModels
{
    public class EditApplicantViewModel : Applicant
    {
        public List<SelectListItem>? courseSelectList { get; set; }
        public List<SelectListItem>? uniSelectList { get; set; }
        // public string Id { get; set; }
        public string Email { get; set; }
    }
}
