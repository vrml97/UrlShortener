using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class UrlMapping
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ShortCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(2000)]
        public string OriginalUrl { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public int ClickCount { get; set; } = 0;
        
        public string? Description { get; set; }
    }
}
