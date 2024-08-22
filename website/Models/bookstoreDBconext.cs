using Microsoft.EntityFrameworkCore;

using website.Models;
namespace website.Models
{
    public class bookstoreDBconext : DbContext
    {
        public bookstoreDBconext(DbContextOptions<bookstoreDBconext> options) : base(options)
        {



        }
  
        public DbSet<webbookstore> webbookstore { get; set; }

        public DbSet<genres> genres { get; set; }
		public DbSet<orders> orders { get; set; }
	}
}
