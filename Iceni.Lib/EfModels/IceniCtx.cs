using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Iceni.Lib.EfModels;

/// <summary>
///     Context for accessing the IceniDb
/// </summary>
public class IceniCtx : IdentityDbContext<IceniUser, IceniRole, Guid>
{
    /// <summary>
    ///     build ctr
    /// </summary>
    public IceniCtx() : base(new DbContextOptionsBuilder().UseSqlServer("fake connection string for ef migrations").Options)
    {

    }

    /// <summary>
    ///     Overrides OnModelCreating
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Payment>().HasOne(x => x.Pupil).WithMany(x => x.Payments).OnDelete(DeleteBehavior.NoAction);
    }

    /// <summary>
    ///     runtime ctr
    /// </summary>
    /// <param name="options"></param>
    public IceniCtx(DbContextOptions<IceniCtx> options) : base(options)
    {
            
    }

    /// <summary>
    ///     Pupils DbSet
    /// </summary>
    public DbSet<Pupil> Pupils { get; set; } = null!;

    /// <summary>
    ///     Lessons DbSet
    /// </summary>
    public DbSet<Lesson> Lessons { get; set; } = null!;

    /// <summary>
    ///     Payments DbSet
    /// </summary>
    public DbSet<Payment> Payments { get; set; } = null!;
}