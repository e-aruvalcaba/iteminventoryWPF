using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventoryApp.Models
{
    class DGPedido
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Items { get; set; } = "";
        public string Status { get; set; }
        public double Total { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public DateTime dateTime { get; set; }
    }
}
