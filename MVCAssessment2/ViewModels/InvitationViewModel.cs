using MVCAssessment2.Models;

namespace MVCAssessment2.ViewModels
{
    public class InvitationViewModel
    {
        public Applicant applicant { get; set; }
        public AspNetUsers aspNetUsers { get; set; }
        public string adminName { get; set; }
    }
}
