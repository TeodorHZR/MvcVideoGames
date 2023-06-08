using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcVideoGames.Data;
using MvcVideoGames.Models;

namespace MvcVideoGames.Controllers
{
    public class VideoGamesController : Controller
    {
        private readonly MvcVideoGamesContext _context;

        public VideoGamesController(MvcVideoGamesContext context)
        {
            _context = context;
        }

        // GET: VideoGames
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var videoGames = from vg in _context.VideoGame
                             select vg;

            if (!String.IsNullOrEmpty(searchString))
            {
                videoGames = videoGames.Where(vg => vg.Title.Contains(searchString)
                                             || vg.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    videoGames = videoGames.OrderByDescending(vg => vg.Title);
                    break;
                case "Date":
                    videoGames = videoGames.OrderBy(vg => vg.ReleaseDate);
                    break;
                case "date_desc":
                    videoGames = videoGames.OrderByDescending(vg => vg.ReleaseDate);
                    break;
                default:
                    videoGames = videoGames.OrderBy(vg => vg.Title);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<VideoGame>.CreateAsync(videoGames.AsNoTracking(), pageNumber ?? 1, pageSize));
        }



        // GET: VideoGames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VideoGame == null)
            {
                return NotFound();
            }

                    var videoGame = await _context.VideoGame
             .Include(v => v.Reviews)
             .FirstOrDefaultAsync(m => m.Id == id);

            if (videoGame == null)
            {
                return NotFound();
            }

            return View(videoGame);
        }

        // GET: VideoGames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VideoGames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Description,Price")] VideoGame videoGame)
        {
            if (ModelState.IsValid)
            {
                _context.Add(videoGame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(videoGame);
        }

        // GET: VideoGames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VideoGame == null)
            {
                return NotFound();
            }

            var videoGame = await _context.VideoGame.FindAsync(id);
            if (videoGame == null)
            {
                return NotFound();
            }
            return View(videoGame);
        }

        // POST: VideoGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Description,Price")] VideoGame videoGame)
        {
            if (id != videoGame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videoGame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoGameExists(videoGame.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(videoGame);
        }

        // GET: VideoGames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VideoGame == null)
            {
                return NotFound();
            }

            var videoGame = await _context.VideoGame
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videoGame == null)
            {
                return NotFound();
            }

            return View(videoGame);
        }

        // POST: VideoGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VideoGame == null)
            {
                return Problem("Entity set 'MvcVideoGamesContext.VideoGame'  is null.");
            }
            var videoGame = await _context.VideoGame.FindAsync(id);
            if (videoGame != null)
            {
                _context.VideoGame.Remove(videoGame);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoGameExists(int id)
        {
          return _context.VideoGame.Any(e => e.Id == id);
        }
    }
}
