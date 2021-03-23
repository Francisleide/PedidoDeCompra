using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosDeCompra.Models
{
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public double Valor { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeDisponivel { get; set; }
        [ForeignKey("Pedido")]
        public int? PedidoId { get; set; }
        public virtual Pedido Pedido { get; set; }
    }
}