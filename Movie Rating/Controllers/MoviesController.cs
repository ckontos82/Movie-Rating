using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IEnumerable<Movie> Get()
        {
            return new[]
            {
                new Movie {Id = 1, Title = "The Godfather", Year = 1972},
                new Movie {Id = 2, Title = "Schindler's List", Year = 1993},
                new Movie {Id = 3, Title = "WALL·E", Year = 2008}
            };
        }
    }
}
