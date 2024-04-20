using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Movie_Rating.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private ILogger<MoviesController> _logger;

        public MoviesController(ILogger<MoviesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getmovies")]
        public ActionResult<IEnumerable<Movie>> Get()
        {
            var movies = new List<Movie>
            {
                new Movie {Id = 1, Title = "The Godfather", Year = 1972},
                new Movie {Id = 2, Title = "Schindler's List", Year = 1993},
                new Movie {Id = 3, Title = "WALL·E", Year = 2008},
                new Movie {Id = 4, Title = "The Matrix", Year = 1999},
                new Movie {Id = 5, Title = "A bad movie", Year = 360 },
                new Movie {Id = 6, Title = "Another bad movie", Year = 12}
            };

            var invalidMovieResults = movies.Select(movie =>
            {
                var context = new ValidationContext(movie);
                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(movie, context, results, true);
                return isValid ? null : results;
            }).Where(results => results != null).ToList();

            if (invalidMovieResults.Any())
            {
                return BadRequest(invalidMovieResults.SelectMany(x => x).ToList());
            }
            else
            return Ok(movies);
        }


    }
}
