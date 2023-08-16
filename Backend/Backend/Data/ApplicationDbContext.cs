using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Hotel>()
                .HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Hotel>()
                .HasMany(h => h.Comments)
                .WithOne(c => c.Hotel)
                .HasForeignKey(c => c.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Hotel>()
                .HasMany(h => h.Reservations)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserEmail)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserEmail)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Room>()
                .HasMany(r => r.Reservations)
                .WithOne(rv => rv.Room)
                .HasForeignKey(rv => rv.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Reservation>()
                .HasOne(rv => rv.Hotel)
                .WithMany(h => h.Reservations)
                .HasForeignKey(rv => rv.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Reservation>()
                .HasOne(rv => rv.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(rv => rv.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}