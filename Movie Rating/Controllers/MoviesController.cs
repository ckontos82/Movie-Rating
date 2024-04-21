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

        //For demonstration/educational purposes.
        //This private List will be removed and replaced with db support.
        private static List<Movie> _movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "The Godfather", Year = 1972 },
            new Movie { Id = 2, Title = "Schindler's List", Year = 1993 },
            new Movie { Id = 3, Title = "WALL·E", Year = 2008 },
            new Movie { Id = 4, Title = "The Matrix", Year = 1999 }
        };

        private ILogger<MoviesController> _logger;

        public MoviesController(ILogger<MoviesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getbyid/{id}")]
        public ActionResult<Movie> GetById(int id)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return movie;
        }

        [HttpGet("getmovies")]
        public ActionResult<IEnumerable<Movie>> Get()
        {
            var validationResults = _movies.Select(movie =>
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
                : Ok(_movies);
        }

        [HttpPost("addmovie")]
        public IActionResult Post([FromBody] Movie movie)
        {
            var context = new ValidationContext(movie);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(movie, context, results, true);

            Func<Movie, IActionResult> addMovie = (m) =>
            {
                m.Id = _movies.Any() ? _movies.Max(x => x.Id) + 1 : 1;
                _movies.Add(m);
                return CreatedAtAction(nameof(GetById), new { id = m.Id }, m);
            };

            return isValid ? addMovie(movie) : BadRequest(results);
        }

        [HttpPost("addmovies")]
        public ActionResult<IEnumerable<Movie>> Post([FromBody] IEnumerable<Movie> movies)
        {
            var validationResults = movies.Select(movie =>
            {
                var context = new ValidationContext(movie);
                var results = new List<ValidationResult>();
                Validator.TryValidateObject(movie, context, results, true);
                return new { Movie = movie, Results = results };
            })
                .Where(x => x.Results.Any())
                .ToList();

            if (!validationResults.Any())
                movies.ToList().ForEach(m =>
                {
                    m.Id = _movies.Max(x => x.Id) + 1;
                    _movies.Add(m);
                });            

            return validationResults.Any() ?
                BadRequest(validationResults.SelectMany(x => x.Results).ToList()) :
                Ok(movies);
        }
    }
}
