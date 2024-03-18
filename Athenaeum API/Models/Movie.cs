using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Security.Policy;

namespace Athenaeum_API.Models
{
    [ExcludeFromCodeCoverage]
    [Table("MOVIES")]
    public class Movie
    {
        [Key]
        public int MOVIE_KEY { get; set; }
        public string MOVIE_NAME { get; set; }
        public string ?MOVIE_DESC { get; set; }
        public string ?MOVIE_POSTER_PATH { get; set; }
        public string MOVIE_FILE_PATH { get; set; }
        public string MOVIE_FILE_NAME { get; set; }

    }
}
