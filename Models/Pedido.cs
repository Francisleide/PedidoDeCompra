using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidosDeCompra.Models
{
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; }
        [DisplayName("Valor total")]
        public double ValorTotal { get; set; }
        public string DataPedido { get; set; }
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }

        public Pedido(){
            ValorTotal = 0;
        }
    }
}