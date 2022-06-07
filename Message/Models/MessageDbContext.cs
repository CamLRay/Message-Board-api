using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Message.Models
{
    public class MessageContext : IdentityDbContext<ApplicationUser>
    {
      public DbSet<Post> Posts { get; set; }
      public DbSet<Thread> Threads { get; set; }
      
      public MessageContext(DbContextOptions<MessageContext> options) : base(options) {  }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
        optionsBuilder.UseLazyLoadingProxies();
      }
    }
}