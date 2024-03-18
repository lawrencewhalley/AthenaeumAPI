using Microsoft.AspNetCore.Mvc;
using Athenaeum_API.Models;
using Athenaeum_API.Data;
using System.Net.Mime;
using System.IO.Compression;

namespace Athenaeum_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly APIContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MovieController(APIContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            _webHostEnvironment = webHostEnvironment;   
        }

        [HttpPost]
        public JsonResult DeleteMovie(int Movie_Key)
        {

            try
            {
                var MovieExists = context.Movies.Any(x => x.MOVIE_KEY == Movie_Key);

                if (MovieExists == false)
                {
                    return new JsonResult(new { message = "Movie Does not Exist" }) { StatusCode = 500 };
                }

                var FoundMovie = context.Movies.First(x => x.MOVIE_KEY == Movie_Key);

                try
                {
                    System.IO.File.Delete(FoundMovie.MOVIE_FILE_PATH);
                    System.IO.File.Delete(FoundMovie.MOVIE_POSTER_PATH);
                }
                catch (Exception ex)
                {
                    return new JsonResult("Failed on deleting the files" + ex);
                }

                context.Movies.Remove(FoundMovie);
                context.SaveChanges();

                return new JsonResult(Ok(true));
            }

            catch (Exception ex)
            {
                return new JsonResult (StatusCode(500, $"Internal server error: {ex}"));
            }

        }

        [HttpPost]
        [RequestSizeLimit(20L * 1024 * 1024 * 1024)]
        public async Task<IActionResult> UploadMovie()
        {
            try
            {
                var movieDescription = Request.Form["MovieDesc"];
                var movieName = Request.Form["MovieName"];
                var moviePoster = Request.Form.Files[0];
                var movieFile = Request.Form.Files[1];

                var MovieFilesPathConfigName = "MovieFilePath";
                var MoviePosterPathConfigName = "MoviePosterPath";

                var MovieFilesFolderPath = context.Config
                    .Where(x => x.CONFIG_NAME == MovieFilesPathConfigName)
                    .Select(x => x.CONFIG_VALUE)
                    .FirstOrDefault();

                var MoviePosterFolderPath = context.Config
                    .Where(x => x.CONFIG_NAME == MoviePosterPathConfigName)
                    .Select(x => x.CONFIG_VALUE)
                    .FirstOrDefault();


                var Movie = new Movie
                {
                    MOVIE_NAME = movieName,
                    MOVIE_DESC = movieDescription,
                    MOVIE_POSTER_PATH = Path.Combine(MoviePosterFolderPath, moviePoster.FileName),
                    MOVIE_FILE_PATH = Path.Combine(MovieFilesFolderPath, movieFile.FileName),
                    MOVIE_FILE_NAME = movieFile.FileName
                };

                var MovieExists = context.Movies.Any(x => x.MOVIE_NAME == Movie.MOVIE_NAME &&
                                                          x.MOVIE_DESC == Movie.MOVIE_DESC &&
                                                          x.MOVIE_POSTER_PATH == Movie.MOVIE_POSTER_PATH &&
                                                          x.MOVIE_FILE_PATH == Movie.MOVIE_FILE_PATH);

                if (MovieExists)
                {
                    return BadRequest(new { message = "movie already exists" });
                }

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

                context.Movies.Add(Movie);

                context.SaveChanges();

                return Ok(new { message = "Files uploaded successfully and Movie Record added to the Database" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
   
        [HttpGet]
        public IActionResult GetMovieData()
        {
            try
            {
                var result = context.Movies.ToList();
                var moviesData = new List<object>();

                foreach (var movie in result)
                {                    
                    
                    var movieData = new
                    {
                        MovieName = movie.MOVIE_NAME,
                        MovieID = movie.MOVIE_FILE_NAME
                    };

                    moviesData.Add(movieData);
                }

                return new JsonResult(moviesData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet]
        public IActionResult GetMoviePoster(string movieName)
        {
            try
            {
                var moviePosterPath = context.Movies
                    .Where(x => x.MOVIE_NAME == movieName)
                    .Select(x => x.MOVIE_POSTER_PATH)
                    .First();

                byte[] imageData;

                using (var fileStream = new FileStream(moviePosterPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        imageData = memoryStream.ToArray();
                    }
                }

                return new JsonResult(imageData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

    }
}
