using System.ComponentModel.DataAnnotations;

namespace UrlShortener.ViewModels
{
    public class CreateUrlMappingViewModel
    {
        [StringLength(50)]
        [Display(Name = "Short Code")]
        public string? ShortCode { get; set; }

        [Required]
        [Url]
        [StringLength(2000)]
        [Display(Name = "Original URL")]
        public string OriginalUrl { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
