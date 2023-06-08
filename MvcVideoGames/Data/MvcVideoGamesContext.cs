using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcVideoGames.Models;

namespace MvcVideoGames.Data
{
    public class MvcVideoGamesContext : DbContext
    {
        public MvcVideoGamesContext (DbContextOptions<MvcVideoGamesContext> options)
            : base(options)
        {
        }

        public DbSet<MvcVideoGames.Models.VideoGame> VideoGame { get; set; } = default!;

        public DbSet<MvcVideoGames.Models.Review> Review { get; set; }
    }
}
