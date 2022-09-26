using Atlancer.Models;
using E_Freelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace Atlancer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<Freelancer> Freelancer { get; set; }
        public DbSet<Gigs> Gig { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Bid> Bid { get; set; }
    }
}
