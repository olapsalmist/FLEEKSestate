using EstateApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EsatateApp.Data.DatabaseContexts.ApplicationDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet <Property> Properties {get; set;}
        public DbSet <Contact>  COntacts {get; set;}
    }
}