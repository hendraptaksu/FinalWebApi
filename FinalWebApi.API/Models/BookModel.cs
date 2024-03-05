using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalWebApi.API.Models
{
    [Table("books")]
    public class BookModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id {get;set;}

        [Required]
        public string title { get;set;}

        [Required]
        public string author { get;set;}

        [Required]
        public int publication_year { get;set;}

        [Required]
        public int page_count { get;set;}

    }
}