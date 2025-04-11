using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class YemekSepetiDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=YemekSepetiDB;Trusted_Connection=true;Enlist=false;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<OrderBridgeStatus>()
            //    .HasOne(osb => osb.Order)
            //    .WithMany(o => o.StatusHistory)
            //    .HasForeignKey(osb => osb.OrderId);

            //modelBuilder.Entity<OrderBridgeStatus>()
            //    .HasOne(osb => osb.Status)
            //    .WithMany()
            //    .HasForeignKey(osb => osb.OrderStatusId);

            modelBuilder.Entity<Restaurant>()
                .HasQueryFilter(r => !r.IsDeleted);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasQueryFilter(r => !r.IsDeleted);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasQueryFilter(r => !r.IsDeleted);
            base.OnModelCreating(modelBuilder);
        }



        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantUser> RestaurantUsers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderBridgeStatus> OrderBridgeStatuses { get; set; }

    }
}
