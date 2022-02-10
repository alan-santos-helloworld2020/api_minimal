using Microsoft.EntityFrameworkCore;
using SistemaOnload.models;

namespace SistemaOnload.ContextDb;
class ClienteDb : DbContext
{
    public ClienteDb(DbContextOptions options) : base(options) { }
    public DbSet<Cliente> Clientes { get; set; }
}