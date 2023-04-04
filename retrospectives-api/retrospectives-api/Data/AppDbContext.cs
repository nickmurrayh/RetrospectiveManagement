using Microsoft.EntityFrameworkCore;
using retrospectives_api.Controllers;
using retrospectives_api.Models;

namespace retrospectives_api.Data;

public class AppDbContext : DbContext
{
    public virtual DbSet<Retrospective> Retrospectives { get; set; }
    public virtual DbSet<FeedbackItem> FeedbackItems { get; set; }

    public AppDbContext()
    {
        
    }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("InMemoryDb");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Retrospective>()
            .HasKey(r => r.Name);

        builder.Entity<Retrospective>()
            .HasMany(r => r.FeedbackItems)
            .WithOne(f => f.Retrospective)
            .HasForeignKey(f => f.RetrospectiveName);
        
        builder.Entity<Retrospective>().Property(p => p.Participants).HasConversion(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
    
}