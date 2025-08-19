using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.ViewModels;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private readonly UrlShortenerContext _context;

        public UrlsController(UrlShortenerContext context)
        {
            _context = context;
        }

        // GET: api/urls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UrlMappingApiResponse>>> GetUrlMappings()
        {
            var urlMappings = await _context.UrlMappings
                .OrderByDescending(u => u.CreatedDate)
                .ToListAsync();

            var response = urlMappings.Select(u => new UrlMappingApiResponse
            {
                Id = u.Id,
                ShortCode = u.ShortCode,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{u.ShortCode}",
                OriginalUrl = u.OriginalUrl,
                Description = u.Description,
                CreatedDate = u.CreatedDate,
                ClickCount = u.ClickCount
            });

            return Ok(response);
        }

        // GET: api/urls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UrlMappingApiResponse>> GetUrlMapping(int id)
        {
            var urlMapping = await _context.UrlMappings.FindAsync(id);

            if (urlMapping == null)
            {
                return NotFound();
            }

            var response = new UrlMappingApiResponse
            {
                Id = urlMapping.Id,
                ShortCode = urlMapping.ShortCode,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{urlMapping.ShortCode}",
                OriginalUrl = urlMapping.OriginalUrl,
                Description = urlMapping.Description,
                CreatedDate = urlMapping.CreatedDate,
                ClickCount = urlMapping.ClickCount
            };

            return Ok(response);
        }

        // POST: api/urls
        [HttpPost]
        public async Task<ActionResult<UrlMappingApiResponse>> CreateUrlMapping(CreateUrlMappingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Generate short code if not provided
            string shortCode = !string.IsNullOrEmpty(model.ShortCode) 
                ? model.ShortCode 
                : GenerateShortCode();

            // Check if short code already exists
            var existingMapping = await _context.UrlMappings
                .FirstOrDefaultAsync(u => u.ShortCode == shortCode);

            if (existingMapping != null)
            {
                return Conflict(new { message = "Short code already exists", shortCode });
            }

            var urlMapping = new UrlMapping
            {
                ShortCode = shortCode,
                OriginalUrl = model.OriginalUrl,
                Description = model.Description,
                CreatedDate = DateTime.UtcNow
            };

            _context.UrlMappings.Add(urlMapping);
            await _context.SaveChangesAsync();

            var response = new UrlMappingApiResponse
            {
                Id = urlMapping.Id,
                ShortCode = urlMapping.ShortCode,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{urlMapping.ShortCode}",
                OriginalUrl = urlMapping.OriginalUrl,
                Description = urlMapping.Description,
                CreatedDate = urlMapping.CreatedDate,
                ClickCount = urlMapping.ClickCount
            };

            return CreatedAtAction("GetUrlMapping", new { id = urlMapping.Id }, response);
        }

        // PUT: api/urls/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUrlMapping(int id, CreateUrlMappingViewModel model)
        {
            var urlMapping = await _context.UrlMappings.FindAsync(id);
            if (urlMapping == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if short code already exists for another mapping
            var existingMapping = await _context.UrlMappings
                .FirstOrDefaultAsync(u => u.ShortCode == model.ShortCode && u.Id != id);

            if (existingMapping != null)
            {
                return Conflict(new { message = "Short code already exists", shortCode = model.ShortCode });
            }

            urlMapping.ShortCode = model.ShortCode ?? urlMapping.ShortCode;
            urlMapping.OriginalUrl = model.OriginalUrl;
            urlMapping.Description = model.Description;

            _context.Update(urlMapping);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/urls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrlMapping(int id)
        {
            var urlMapping = await _context.UrlMappings.FindAsync(id);
            if (urlMapping == null)
            {
                return NotFound();
            }

            _context.UrlMappings.Remove(urlMapping);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/urls/by-code/{shortCode}
        [HttpGet("by-code/{shortCode}")]
        public async Task<ActionResult<UrlMappingApiResponse>> GetUrlMappingByCode(string shortCode)
        {
            var urlMapping = await _context.UrlMappings
                .FirstOrDefaultAsync(u => u.ShortCode == shortCode);

            if (urlMapping == null)
            {
                return NotFound();
            }

            var response = new UrlMappingApiResponse
            {
                Id = urlMapping.Id,
                ShortCode = urlMapping.ShortCode,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{urlMapping.ShortCode}",
                OriginalUrl = urlMapping.OriginalUrl,
                Description = urlMapping.Description,
                CreatedDate = urlMapping.CreatedDate,
                ClickCount = urlMapping.ClickCount
            };

            return Ok(response);
        }

        private string GenerateShortCode()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new char[6];
            
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            
            return new string(result);
        }
    }

    public class UrlMappingApiResponse
    {
        public int Id { get; set; }
        public string ShortCode { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public string OriginalUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ClickCount { get; set; }
    }
}
