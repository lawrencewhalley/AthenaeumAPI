using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Athenaeum_API.Models
{
    [ExcludeFromCodeCoverage]
    [Table("CONFIG")]
    public class Config
    {
        [Key]
        public int CONFIG_KEY { get; set; }
        public string CONFIG_NAME { get; set; }
        public string CONFIG_VALUE { get; set; }
    }
}
