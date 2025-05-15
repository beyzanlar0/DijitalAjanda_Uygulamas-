using Microsoft.EntityFrameworkCore;

namespace ForMig
{
    public class DataBase : DbContext
    {
        public DbSet<Task> Taskss { get; set; }

        public DbSet<User> Userss { get; set; }
        public DbSet<Friend> Friendss { get; set; }
        public DbSet<Messages> Messagess { get; set; }


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
