using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoT_Newspaper.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IoT_Newspaper.Models;

namespace IoT_Newspaper.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Section> Section { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<News>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<News>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<News>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<News>()
                .Property(e => e.Categories)
                .IsUnicode(false);

            modelBuilder.Entity<News>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Section>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Section>()
                .HasMany(e => e.News)
                .WithOne(e => e.Section)
                .HasForeignKey(e => e.Section_Id);
        }
    }
}