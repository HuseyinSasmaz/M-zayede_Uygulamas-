using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Müzayede_Uygulaması.Models;

namespace Müzayede_Uygulaması.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ÜrünListeleme> ÜrünListelemes { get; set; }    
        public DbSet<Teklif> Teklifs { get; set; } 
        public DbSet<Yorum> Yorums { get; set; }
    }
}
