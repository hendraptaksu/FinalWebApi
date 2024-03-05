using System.ComponentModel.DataAnnotations;

namespace FinalWebApi.API.DTOs.Requests
{
    public class UpdateBookRequest
    {
        [Required]
        [MinLength(3)]
        public string title { get; set; }

        [Required]
        public string author { get; set; }

        [Required]
        public int publication_year { get; set; }

        [Required]
        public int page_count { get; set; }
    }
}