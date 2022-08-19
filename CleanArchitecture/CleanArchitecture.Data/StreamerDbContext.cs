using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Data
{
    public class StreamerDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-4K9Q8J3; Initial Catalog=Streamer; Integrated Security = True;")
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information)
            .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Streamer>()
                .HasMany(x => x.Videos)
                .WithOne(x => x.Streamer)
                .HasForeignKey(x => x.StreamerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Video>()
                .HasMany(x => x.Actores)
                .WithMany(x => x.Videos)
                .UsingEntity<VideoActor>(
                    x => x.HasKey(x => new { x.ActorId, x.VideoId })
                );
        }
        public DbSet<Streamer>? Streamers { get; set; }
        public DbSet<Video>? Videos { get; set; }
    }
}
