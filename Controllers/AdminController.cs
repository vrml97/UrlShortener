using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.ViewModels;

namespace UrlShortener.Controllers
{
    public class AdminController : Controller
    {
        private readonly UrlShortenerContext _context;

        public AdminController(UrlShortenerContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var urlMappings = await _context.UrlMappings
                .OrderByDescending(u => u.CreatedDate)
                .ToListAsync();
            
            return View(urlMappings);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUrlMappingViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Generate short code if not provided
                string shortCode = !string.IsNullOrEmpty(model.ShortCode) 
                    ? model.ShortCode 
                    : GenerateShortCode();

                // Check if short code already exists
                var existingMapping = await _context.UrlMappings
                    .FirstOrDefaultAsync(u => u.ShortCode == shortCode);

                if (existingMapping != null)
                {
                    ModelState.AddModelError("ShortCode", "Short code already exists");
                    return View(model);
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

                TempData["SuccessMessage"] = $"Short URL created successfully: {Request.Scheme}://{Request.Host}/{shortCode}";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urlMapping = await _context.UrlMappings.FindAsync(id);
            if (urlMapping == null)
            {
                return NotFound();
            }

            var model = new CreateUrlMappingViewModel
            {
                ShortCode = urlMapping.ShortCode,
                OriginalUrl = urlMapping.OriginalUrl,
                Description = urlMapping.Description
            };

            return View(model);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateUrlMappingViewModel model)
        {
            var urlMapping = await _context.UrlMappings.FindAsync(id);
            if (urlMapping == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Check if short code already exists for another mapping
                var existingMapping = await _context.UrlMappings
                    .FirstOrDefaultAsync(u => u.ShortCode == model.ShortCode && u.Id != id);

                if (existingMapping != null)
                {
                    ModelState.AddModelError("ShortCode", "Short code already exists");
                    return View(model);
                }

                urlMapping.ShortCode = model.ShortCode ?? urlMapping.ShortCode;
                urlMapping.OriginalUrl = model.OriginalUrl;
                urlMapping.Description = model.Description;

                _context.Update(urlMapping);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Short URL updated successfully";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urlMapping = await _context.UrlMappings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urlMapping == null)
            {
                return NotFound();
            }

            return View(urlMapping);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var urlMapping = await _context.UrlMappings.FindAsync(id);
            if (urlMapping != null)
            {
                _context.UrlMappings.Remove(urlMapping);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Short URL deleted successfully";
            }

            return RedirectToAction(nameof(Index));
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
}
