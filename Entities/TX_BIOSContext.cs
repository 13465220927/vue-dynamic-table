using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Logistyx.Bios.WebApp.Entities
{
    public partial class TX_BIOSContext : DbContext
    {
        public TX_BIOSContext()
        {
        }

        public TX_BIOSContext(DbContextOptions<TX_BIOSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Resource> Resource { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(e => new { e.Key, e.LanguageCode })
                    .ForSqlServerIsClustered(false);

                entity.ToTable("RESOURCE");

                entity.Property(e => e.Key)
                    .HasColumnName("KEY")
                    .HasMaxLength(200);

                entity.Property(e => e.LanguageCode)
                    .IsRequired()
                    .HasColumnName("LANGUAGE_CODE")
                    .HasMaxLength(10);

                entity.Property(e => e.Translation).HasColumnName("TRANSLATION");
            });
        }
    }
}
