using Microsoft.EntityFrameworkCore;

namespace PedidosDeCompra.Models
{
    public class PedidosDeCompraContext : DbContext
    {
          public PedidosDeCompraContext(DbContextOptions<PedidosDeCompraContext> options) : base(options)
        {

        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
    }
}