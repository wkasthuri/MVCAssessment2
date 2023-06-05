using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAssessment2.Models;
using System.ComponentModel.DataAnnotations;

namespace MVCAssessment2.ViewModels
{
    public class EditApplicantViewModel: Applicant
    {
        // Course dropdown
        public List<SelectListItem>? courseSelectList { get; set; }

        // Uni dropdown
        public List<SelectListItem>? uniSelectList { get; set; }
       
        public string Email { get; set; }
    }
}
