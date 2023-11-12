using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using telefone_book_api.Models;
using telefone_book_api.Models.Dto;

namespace telefone_book_api.Data;

public class TelephoneBookDbContext : IdentityDbContext<TelephoneBookUser>
{
    public DbSet<Contact> Contacts { get; set; }

    public TelephoneBookDbContext(DbContextOptions<TelephoneBookDbContext> options)
        : base(options) { }

    public TelephoneBookDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-L0KR7172\SQLEXPRESS;Initial Catalog=TelephoneBook;Integrated Security=True");
        }
        base.OnConfiguring(optionsBuilder);
    }
}
