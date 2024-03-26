using System.ComponentModel.DataAnnotations;

namespace Movie_Rating
{
    public class Movie
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int? Year { get; set; }
    }
}
