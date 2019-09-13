using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventoryApp.Models
{
    class Pedido
    {
        public int id { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public int ItemsQuantity { get; set; }
        public int Status { get; set; }
        public double Total { get; set; }
        public DateTime Date { get; set; }
    }
}
