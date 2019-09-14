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
        public string Name { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public List<ItemQty> ItemsQuantity { get; set; } = new List<ItemQty>();
        public int Status { get; set; }
        public double Total { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public DateTime dateTime { get; set; }
    }
}
