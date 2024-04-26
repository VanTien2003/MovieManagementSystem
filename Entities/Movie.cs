using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagementSystem.Entities
{
    [Table("Movie")]
    public class Movie : BaseEntity
    {
        [Required(ErrorMessage = "MovieDuration is required")]
        [Range(1, int.MaxValue, ErrorMessage = "MovieDuration must be a positive value")]
        public int MovieDuration { get; set; }

        [Required(ErrorMessage = "EndTime is required")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "PremiereDate is required")]
        public DateTime PremiereDate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Director is required")]
        public string Director { get; set; } = "";

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; } = "";

        [Required(ErrorMessage = "HeroImage is required")]
        public string HeroImage { get; set; } = "";

        [Required(ErrorMessage = "Language is required")]
        public string Language { get; set; } = "";

        [ForeignKey("MovieType")]
        [Required(ErrorMessage = "MovieTypeId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "MovieTypeId must be a positive value")]
        public int MovieTypeId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = "";

        [ForeignKey("Rate")]
        [Required(ErrorMessage = "RateId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "RateId must be a positive value")]
        public int RateId { get; set; }

        [Required(ErrorMessage = "Trailer is required")]
        public string Trailer { get; set; } = "";
        public bool IsActive { get; set; } = false;

        public MovieType? MovieType { get; set; }
        public Rate? Rate { get; set; }
        public IEnumerable<Schedule>? Schedules { get; set; }
    }
}
