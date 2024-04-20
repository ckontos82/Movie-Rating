using System.ComponentModel.DataAnnotations;

namespace Movie_Rating
{
    public class Movie
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Range(1900,2024)]
        public int? Year { get; set; }
    }
}
