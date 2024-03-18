using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Security.Policy;

namespace Athenaeum_API.Models
{
    [ExcludeFromCodeCoverage]
    [Table("LOGINS")]
    public class User
    {
        [Key]
        public int LOG_KEY { get; set; } 
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public DateTime? LAST_LOGIN { get; set; }

    }
}
