using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventoryApp.Models
{
    class PopularItem
    {
        public int id { get; set; }
        public string code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        //public string ImagePath { get; set; }
        public double TotalInPedidos { get; set; }
    }
}
