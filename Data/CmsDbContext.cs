using ArvinTabriz.Models;
using Microsoft.EntityFrameworkCore;

namespace ArvinTabriz.Data;

public sealed class CmsDbContext(DbContextOptions<CmsDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Slide> Slides => Set<Slide>();
    public DbSet<ContentPage> ContentPages => Set<ContentPage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(product => product.Id);
            entity.HasIndex(product => product.Slug).IsUnique();
            entity.Property(product => product.Name).HasMaxLength(200).IsRequired();
            entity.Property(product => product.Slug).HasMaxLength(200);
            entity.Property(product => product.Summary).HasMaxLength(1000);
            entity.Property(product => product.Content).HasMaxLength(10000);
            entity.Property(product => product.ImagePath).HasMaxLength(500);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(project => project.Id);
            entity.HasIndex(project => project.Slug).IsUnique();
            entity.Property(project => project.Name).HasMaxLength(200).IsRequired();
            entity.Property(project => project.Slug).HasMaxLength(200);
            entity.Property(project => project.Summary).HasMaxLength(1000);
            entity.Property(project => project.Content).HasMaxLength(10000);
            entity.Property(project => project.ImagePath).HasMaxLength(500);
        });

        modelBuilder.Entity<Slide>(entity =>
        {
            entity.HasKey(slide => slide.Id);
            entity.Property(slide => slide.Title).HasMaxLength(200).IsRequired();
            entity.Property(slide => slide.Content).HasMaxLength(5000);
            entity.Property(slide => slide.ImagePath).HasMaxLength(500);
            entity.Property(slide => slide.LinkUrl).HasMaxLength(500);
        });

        modelBuilder.Entity<ContentPage>(entity =>
        {
            entity.HasKey(page => page.Id);
            entity.HasIndex(page => page.Slug).IsUnique();
            entity.Property(page => page.Title).HasMaxLength(200).IsRequired();
            entity.Property(page => page.Slug).HasMaxLength(200).IsRequired();
            entity.Property(page => page.Summary).HasMaxLength(1000);
            entity.Property(page => page.Content).HasMaxLength(20000);
            entity.Property(page => page.ImagePath).HasMaxLength(500);
        });
    }
}
