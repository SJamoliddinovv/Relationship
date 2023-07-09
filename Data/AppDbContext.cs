using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users {get;set;}
    public DbSet<DriverLicense> driverLicenses{get;set;}
    public DbSet<Car>  Cars { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options){ }


    protected override void OnModelCreating(ModelBuilder builder)
    {  
        builder.Entity<User>()
        .HasKey(x => x.Id);

        builder.Entity<User>()
        .Property(x => x.Username)
        .HasMaxLength(128)
        .IsRequired();

        builder.Entity<User>()
        .HasIndex(x => x.Username)
        .IsUnique();

        builder.Entity<User>()
        .HasIndex(x => x.Email)
        .IsUnique();

        builder.Entity<User>()
        .Property(x => x.Email)
        .IsRequired();

        builder.Entity<User>()
        .HasOne(d => d.DriverLicense)
        .WithOne(u => u.Holder)
        .HasForeignKey<DriverLicense>(u => u.Id)
        .HasPrincipalKey<User> (u => u.Id);

        builder.Entity<User>()
        .HasMany(u => u.Cars)
        .WithOne(u => u.Owner)
        .HasForeignKey(u => u.Id)
        .IsRequired();      //one to many relationshipda userni id si car ga joylanvotti

        builder.Entity<Car> ()
        .HasKey(e => e.Id);

        builder.Entity<Car> ()
        .Property(e => e.Brand)
        .IsRequired()
        .HasMaxLength(20);

        builder.Entity<Car> ()
        .Property(e => e.Color)
        .HasMaxLength(20);

        builder.Entity<Car> ()
        .Property(e => e.Model)
        .IsRequired()
        .HasMaxLength(20);


        base.OnModelCreating(builder);
    }

    
}