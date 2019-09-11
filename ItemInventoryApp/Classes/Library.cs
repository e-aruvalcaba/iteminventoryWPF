using ItemInventoryApp.DAL;
using ItemInventoryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ItemInventoryApp.Classes
{
    class Library
    {
        public DatabaseModel DatbaseInstance;
        public DBHandler DBHandler;
        public UIRuntime UIRuntime;
        public Validations ValidationsHandler;
        public List<DataGrid> DataGridList;
        public List<TextBox> TextBoxList;
    }
}
