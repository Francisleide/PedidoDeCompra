using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PedidosDeCompra.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }
        [Required]
        public string Nome { get; set; }
        [DisplayName("Endereço")]
        public string Endereco { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }

    }
}