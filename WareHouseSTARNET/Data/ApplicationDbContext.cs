using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WareHouseSTARNET.Models;

namespace WareHouseSTARNET.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Material> Materials { get; set; }
        public DbSet<TypeOfMaterial> TypeOfmaterials { get; set; }
        public DbSet<WrittenOffMaterial> WrittenOffMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WrittenOffMaterial>()
                .HasOne(w => w.ApplicationUser)
                .WithMany()
                .HasForeignKey(w => w.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
