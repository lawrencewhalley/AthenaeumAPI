using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Athenaeum_API.Models
{
    [ExcludeFromCodeCoverage]
    [Table("LOGINS")]
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }



    }
}
