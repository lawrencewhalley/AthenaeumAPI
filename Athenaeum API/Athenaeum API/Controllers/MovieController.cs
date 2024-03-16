using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Athenaeum_API.Models;
using Athenaeum_API.Data;
using System.Drawing.Text;
using System.Net;
using System.IO;
using System.Net;
using System.Net.Http;

namespace Athenaeum_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly APIContext context;

        public MovieController(APIContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public JsonResult AddMovie(Movie movie)
        {

            var FoundMovie = context.Movies.Any(x => x == movie);

            if (FoundMovie != false)
            {
                return new JsonResult(new { message = "Movie already Exists" }) { StatusCode = 500 };
            }

            context.Movies.Add(movie);

            context.SaveChanges();

            return new JsonResult(Ok(movie));

        }
        [HttpPost]
        public JsonResult DeleteMovie(int Movie_Key)
        {
            var MovieExists = context.Movies.Any(x => x.MOVIE_KEY == Movie_Key);

            if (MovieExists == false)
            {
                return new JsonResult(new { message = "Movie Does not Exist" }) { StatusCode = 500 };
            }

            var FoundMovie = context.Movies.First(x => x.MOVIE_KEY == Movie_Key);

            context.Movies.Remove(FoundMovie);

            context.SaveChanges();

            return new JsonResult(Ok(true));

        }

        [HttpPost]
        [RequestSizeLimit(20L * 1024 * 1024 * 1024)]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var movieDescription = Request.Form["MovieDesc"];
                var movieName = Request.Form["MovieName"];
                var moviePoster = Request.Form.Files[0];
                var movieFile = Request.Form.Files[1];

                var MoviePosterFolderPath = "V:\\video-project\\public\\MoviePosters";
                var MovieFilesFolderPath = "V:\\video-project\\public\\MovieFiles";

                if (!Directory.Exists(MoviePosterFolderPath))
                {
                    Directory.CreateDirectory(MoviePosterFolderPath);
                }

                if (!Directory.Exists(MovieFilesFolderPath))
                {
                    Directory.CreateDirectory(MovieFilesFolderPath);
                }

                if (moviePoster.Length > 0)
                {
                    var MoviePosterPath = Path.Combine(MoviePosterFolderPath, moviePoster.FileName);
                    using (var stream = new FileStream(MoviePosterPath, FileMode.Create))
                    {
                        await moviePoster.CopyToAsync(stream);
                    }
                }

                if (movieFile.Length > 0)
                {
                    var MovieFilePath = Path.Combine(MovieFilesFolderPath, movieFile.FileName);
                    using (var stream = new FileStream(MovieFilePath, FileMode.Create))
                    {
                        await movieFile.CopyToAsync(stream);
                    }
                }

                var Movie = new Movie
                {
                    MOVIE_NAME = movieName,
                    MOVIE_DESC = movieDescription,
                    MOVIE_POSTER_PATH = Path.Combine(MoviePosterFolderPath, moviePoster.FileName),
                    MOVIE_FILE_PATH = Path.Combine(MovieFilesFolderPath, movieFile.FileName)
                };

                var MovieExists = context.Movies.Any(x => x.MOVIE_NAME == Movie.MOVIE_NAME &&
                                                          x.MOVIE_DESC == Movie.MOVIE_DESC &&
                                                          x.MOVIE_POSTER_PATH == Movie.MOVIE_POSTER_PATH &&
                                                          x.MOVIE_FILE_PATH == Movie.MOVIE_FILE_PATH);

                if (MovieExists)
                {
                    return BadRequest(new { message = "movie already exists" });
                }

                context.Movies.Add(Movie);

                context.SaveChanges();

                return Ok(new { message = "Files uploaded successfully and Movie Record added to the Database" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

    }
}
