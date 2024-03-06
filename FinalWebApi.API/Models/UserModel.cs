using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalWebApi.API.Models
{
    [Table("users")]
    public class UserModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id {get;set;}

        [Required]
        public string username { get;set;}

        [Required]
        public string email { get;set;}

        [Required]
        public string password_hash { get;set;}
    }
}