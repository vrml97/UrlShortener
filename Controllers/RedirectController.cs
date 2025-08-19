using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;

namespace UrlShortener.Controllers
{
    public class RedirectController : Controller
    {
        private readonly UrlShortenerContext _context;

        public RedirectController(UrlShortenerContext context)
        {
            _context = context;
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> Index(string shortCode)
        {
            if (string.IsNullOrEmpty(shortCode))
            {
                return NotFound();
            }

            var urlMapping = await _context.UrlMappings
                .FirstOrDefaultAsync(u => u.ShortCode == shortCode);

            if (urlMapping == null)
            {
                return NotFound("Short URL not found");
            }

            // Increment click count
            urlMapping.ClickCount++;
            await _context.SaveChangesAsync();

            // Redirect to the original URL
            return Redirect(urlMapping.OriginalUrl);
        }
    }
}
