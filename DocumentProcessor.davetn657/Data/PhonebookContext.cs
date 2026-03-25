using Microsoft.EntityFrameworkCore;

namespace DocumentProcessor.davetn657.Data;

public class PhonebookContext : DbContext
{
    public DbSet<PhonebookContext> Contacts { get; set; }

    public PhonebookContext(DbContextOptions options) : base(options)
    {

    }
}