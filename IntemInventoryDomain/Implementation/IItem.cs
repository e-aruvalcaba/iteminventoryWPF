using ItemInventoryDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntemInventoryDomain.Implementation
{
    public interface IItem
    {
        IEnumerable<Item> SelectAll();
        Item SelectById(string id);
        void Insert(Item item);
        void Update(Item item);
        void Delete(string id);
        void Save();
    }
}
