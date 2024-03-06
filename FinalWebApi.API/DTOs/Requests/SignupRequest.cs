using System.ComponentModel.DataAnnotations;

namespace FinalWebApi.API.DTOs.Requests
{
    public class SignupRequest
    {
        [Required]
        public string email { get; set; }
        
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string confirmPassword { get; set; }
    }
}