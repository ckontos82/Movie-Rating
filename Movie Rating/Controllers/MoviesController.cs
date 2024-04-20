using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

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
                new Movie {Id = 4, Title = "The Matrix", Year = 1999}
            };

            var validationResults = movies.Select(movie =>
                {
                    var context = new ValidationContext(movie);
                    var results = new List<ValidationResult>();
                    Validator.TryValidateObject(movie, context, results, true);
                    return new { Movie = movie, Results = results };
                })
                .Where(x => x.Results.Any())
                .ToList();

            return validationResults.Any()
                ? BadRequest(validationResults.SelectMany(x => x.Results).ToList())
                : Ok(movies);
        }
    }
}
