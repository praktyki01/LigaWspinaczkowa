using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Liga.Models;

namespace LigaWspinaczkowa.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Liga.Models.Stage>? Stage { get; set; }
        public DbSet<Liga.Models.UserStage>? UserStage { get; set; }
    }
}