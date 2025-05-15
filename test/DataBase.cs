using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MauiApp1
{
    public class DataBase : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friends { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)   
        {
            optionsBuilder.UseSqlServer(@"Data Source=.;Initial Catalog=DijitalAjandaDB;Integrated Security=true;TrustServerCertificate=true");
           
        } 
    }
}
