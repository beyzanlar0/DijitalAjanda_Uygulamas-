using System;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using Microsoft.Maui.Storage;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MauiApp1
{
    public class DataBase : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Messages> Messages { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)   
        {
            optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=true;TrustServerCertificate=true");
           
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.User1)
                .WithMany(u => u.FriendsInitiated)
                .HasForeignKey(f => f.UserId1)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.User2)
                .WithMany(u => u.FriendsReceived)
                .HasForeignKey(f => f.UserId2)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
