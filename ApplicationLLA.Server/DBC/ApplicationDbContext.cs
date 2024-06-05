using ApplicationLLA.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLLA.Server.DBC
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Reservation>(x => x.HasKey(p => p.ReservationId));
            builder.Entity<Reservation>()
                 .HasOne(r => r.Post)
                 .WithMany(p => p.Reservations)
                 .HasForeignKey(r => r.PostId);
                //.OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Reservation>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CustomerId);
            //.OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId);


            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                },
                new IdentityRole
                {
                    Name = "Worker",
                    NormalizedName ="WORKER"
                 },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
