using Microsoft.EntityFrameworkCore;
using System.Configuration;
using bancoDeImagens.Models;

namespace bancoDeImagens.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) 
        { }
        public DbSet<Images>? Images { get; set; }
    }
}
