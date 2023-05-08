using CardsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CardsAPI.Data
{
    public class CardsDbContext:DbContext
    {
        public CardsDbContext(DbContextOptions<CardsDbContext> options) : base(options) 
        {
        }
        public DbSet<Card> Cards { get; set; }
    }
}
