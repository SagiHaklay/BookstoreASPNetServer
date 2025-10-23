using BookstoreASPNetServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookstoreASPNetServer.Data
{
    public class BookstoreContext : IdentityDbContext<AppUser>
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options)
        {
            
        }
        public DbSet<BookModel> Books { get; set; }
    }
}
