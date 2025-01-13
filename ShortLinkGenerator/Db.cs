using Microsoft.EntityFrameworkCore;

namespace ShortLinkGenerator;

public class Db: DbContext
{
    public DbSet<UrlData> UrlsDataTable { get; set; }
    
    public Db(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }
}