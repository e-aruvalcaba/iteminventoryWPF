﻿using System;
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
    }
}
