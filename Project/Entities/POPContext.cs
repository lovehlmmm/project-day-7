using System.Configuration;

namespace Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class POPContext : DbContext
    {
        public POPContext()
            : base("name=POPDbConnectionStringServer")
        {

        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<CreditCard> CreditCards { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<CreditCard>()
                .Property(e => e.CreditNumber)
                .IsUnicode(false);

            modelBuilder.Entity<CreditCard>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasIndex(e => e.Username)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.MaterialPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Material>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.FolderImage)
                .IsUnicode(false);

            modelBuilder.Entity<Size>()
                .Property(e => e.SizeDetails)
                .IsUnicode(false);

            modelBuilder.Entity<Size>()
                .Property(e => e.SizePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Size>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Status)
                .IsUnicode(false);
        }
    }
}
