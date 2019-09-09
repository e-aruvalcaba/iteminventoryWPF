using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventoryApp.Models
{
    class Pedido
    {
        public int Id { get; set; }
        public List<Item> Items { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}
