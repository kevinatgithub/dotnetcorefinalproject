using System.ComponentModel.DataAnnotations;

namespace FinalProject.ApiModels
{
    public class ConfirmEmailModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
