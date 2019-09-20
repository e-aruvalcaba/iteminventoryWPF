using ItemInventoryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemInventoryApp.Classes
{
    class Comparador : IComparer<PopularItem>
    {

        int IComparer<PopularItem>.Compare(PopularItem x, PopularItem y)
        {
            if (x.TotalInPedidos == 0 || y.TotalInPedidos == 0)
            {
                return 0;
            }

            // CompareTo() method 
            return x.TotalInPedidos.CompareTo(y.TotalInPedidos);
        }
    }
}
