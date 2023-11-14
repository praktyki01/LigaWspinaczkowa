using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LigaWspinaczkowa.Models;

namespace LigaWspinaczkowa.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LigaWspinaczkowa.Models.Stage>? Stage { get; set; }
        public DbSet<LigaWspinaczkowa.Models.UserStage>? UserStage { get; set; }
    }
}