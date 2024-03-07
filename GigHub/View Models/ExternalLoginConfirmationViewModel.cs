using System.ComponentModel.DataAnnotations;

namespace GigHub.View_Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}