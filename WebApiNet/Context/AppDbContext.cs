using Microsoft.EntityFrameworkCore;
using WebApiNet.Models;

namespace WebApiNet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { 
        }

        #region DbSet

        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
		public DbSet<Book>? Books { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillBook> BillBooks { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillBook>()
                .HasKey(e => new { e.BookId, e.BillId });
            modelBuilder.Entity<BillBook>()
                .HasOne(b => b.Book)
                .WithMany(bb => bb.BillBooks)
                .HasForeignKey(b => b.BookId);
			modelBuilder.Entity<BillBook>()
				.HasOne(b => b.Bill)
				.WithMany(bb => bb.BillBooks)
				.HasForeignKey(b => b.BillId);

            modelBuilder.Entity<User>()
                .HasIndex(e=>e.Username).IsUnique();
		}
    }
}