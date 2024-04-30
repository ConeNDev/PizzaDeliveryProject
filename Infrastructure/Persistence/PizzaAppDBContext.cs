using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaApp.Domain.Entities;
using System.Reflection.Emit;

namespace PizzaApp.Infrastructure.Persistence
{
	public class PizzaAppDBContext : IdentityDbContext<ApplicationUser>
	{
        public PizzaAppDBContext(DbContextOptions<PizzaAppDBContext> options):base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Orders)
                      .WithOne(o => o.Cart)
                      .HasForeignKey(o => o.CartId); 
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
        public DbSet<Order> Orders { get; set; }
		public DbSet<Cart> Carts { get; set; }

	}
}
