using System.ComponentModel.DataAnnotations;

namespace FinalWebApi.API.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
}