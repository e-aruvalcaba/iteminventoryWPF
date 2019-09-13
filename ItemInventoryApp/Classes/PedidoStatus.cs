using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventoryApp.Classes
{
    class PedidoStatus
    {
        public int NotRegistered { get; } = 0;
        public int Registered { get; } = 1;
        public int Completed { get; } = 2;
        public int Canceled { get; } = 3;
    }
}
