using ItemInventoryDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntemInventoryDomain.Implementation
{
    public interface IPedido
    {
        IEnumerable<Pedido> SelectAll();
        Pedido SelectById(string id);
        void Insert(Pedido item);
        void Update(Pedido item);
        void Delete(string id);
        void Save();
    }
}
