using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcVideoGames.Data;
using MvcVideoGames.Models;

namespace MvcVideoGames.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly MvcVideoGamesContext _context;

        public ReviewsController(MvcVideoGamesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Review.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int videoGameId, [Bind("UserName,Comment,ReviewDate,VideoGameId")] Review review)
        {
            if (ModelState.IsValid)
            {
                review.VideoGameId = videoGameId; 
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "VideoGames", new { id = review.VideoGameId }); 
            }

            return View(review);
        }


    }
}
