using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventoryApp.Models
{
    class DatabaseModel
    {
        public List<Item> Items { get; set; }
        public List<Pedido> Pedidos { get; set; }
        public Pedido TempPedido { get; set; }
        public int LastItemID { get; set; } = 1;
        public int LastPedidoID { get; set; } = 1;
        public bool EditOn { get; set; }
        public int Theme { get; set; } = 0;
    }
}
