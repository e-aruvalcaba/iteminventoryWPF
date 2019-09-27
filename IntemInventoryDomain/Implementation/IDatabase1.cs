using ItemInventoryDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntemInventoryDomain.Implementation
{
    public interface IDatabase
    {
        DatabaseModel Get();
        bool Save(DatabaseModel db);
    }
}
