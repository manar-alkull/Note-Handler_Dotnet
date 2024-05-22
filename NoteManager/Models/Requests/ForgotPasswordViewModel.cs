using System.ComponentModel.DataAnnotations;

namespace NoteManager.Models.Requests
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
