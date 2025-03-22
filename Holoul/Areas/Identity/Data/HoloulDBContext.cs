using Holoul.Areas.Identity.Data;
using Holoul.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Holoul.Areas.Identity.Data;

public class EDbContext : IdentityDbContext<HoloulUser, IdentityRole, string>
{
    public EDbContext(DbContextOptions<EDbContext> options)
        : base(options)
    {
    }

    public DbSet<AISetting> AISettings { get; set; }
    public DbSet<AISolutionStep> AISolutionSteps { get; set; }
    public DbSet<Category> Categorys { get; set; }
    public DbSet<Problem> Problems { get; set; }
    public DbSet<Feedback> FeedBacks { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Feedback>()
            .HasKey(f => f.id);

        builder.Entity<Feedback>()
            .Property(f => f.id)
            .HasMaxLength(36);


        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<HoloulUser>
{
    public void Configure(EntityTypeBuilder<HoloulUser> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(50);
        builder.Property(x => x.LastName).HasMaxLength(50);
    }
}