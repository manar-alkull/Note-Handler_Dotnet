using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoteManager.Models;

namespace NoteManager.Models
{
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<NoteManager.Models.Note> Notes { get; set; } = default!;
        public DbSet<NoteManager.Models.PublicLink> PublicLink { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>().HasQueryFilter(r => !r.IsDeleted);
            base.OnModelCreating(modelBuilder);

        }
    }
}
