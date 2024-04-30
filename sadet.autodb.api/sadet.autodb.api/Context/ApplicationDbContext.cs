using Microsoft.EntityFrameworkCore;
using sadet.autodb.api.Models;

namespace sadet.autodb.api.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Game> Games { get; set; }
    
    public DbSet<Data> Data { get; set; }
}