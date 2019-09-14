using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using ItemInventoryApp.Models;
using Newtonsoft.Json;
using System.Windows.Controls;
using ItemInventoryApp.Classes;

namespace ItemInventoryApp.DAL
{
    class DBHandler
    {
        private DatabaseModel DBInstance;
        private static string fileName = @"C:\InventoryApp\_InventoryDB.json";

        public DBHandler()
        {
            DBInstance = new DatabaseModel();
            DBInstance.Items = new List<Item>();
            DBInstance.Pedidos = new List<Pedido>();
            DBInstance.TempPedido = new Pedido();
        }
        #region Database Handle
        /*
          * // SUMMARY
          * // Retrieve and initialize the data from the JSON File used as DataBase 
          * // Return: DatabaseModel object
        */
        public DatabaseModel InitializeDB()
        {
            string content = "";

            //Validar directorio existente
            if (!Directory.Exists(@"C:\InventoryApp\"))
            {
                Directory.CreateDirectory(@"C:\InventoryApp\");
            }

            //Si el archivo no existe se crea
            if (!File.Exists(fileName))
            {

                var data = JsonConvert.SerializeObject(DBInstance);
                using (FileStream fs = File.Create(fileName))
                {
                    Byte[] title = new UTF8Encoding(true).GetBytes(data);
                    fs.Write(title, 0, title.Length);
                }

                //DBInstance.Items = new List<Item>();
                //DBInstance.Pedidos = new List<Pedido>();
                //DBInstance.TempPedido = new Pedido();
                return DBInstance;
            }
            else //Si el archivo existe se lee por completo el contenido del JSON y se deserializa, para luego retornarse.
            {
                using (StreamReader sr = File.OpenText(fileName))
                {
                    using (StreamReader r = new StreamReader(fileName))
                    {
                        content = r.ReadToEnd();
                    }
                }

                var json = JsonConvert.DeserializeObject<DatabaseModel>(content);
                validatePedidoAtInit(json);
                return json;
            }
        }
        /*
          * // SUMMARY
          * // Retrieve the data from the JSON File used as DataBase to update the object with more recent data
          * // Return: DatabaseModel object
        */
        public DatabaseModel UpdateDBObject()
        {
            //Declare file name
            string fileName = @"C:\InventoryApp\_InventoryDB.json";
            string content = "";

            using (StreamReader sr = File.OpenText(fileName))
            {
                using (StreamReader r = new StreamReader(fileName))
                {
                    content = r.ReadToEnd();
                }
            }

            var json = JsonConvert.DeserializeObject<DatabaseModel>(content);

            return json;
        }
        /*
          * // SUMMARY
          * // Saves the changes on the JSON File writing the new values
          * // Return: bool (True/False)
        */
        private bool SaveDB(DatabaseModel db)
        {

            bool success = false;

            try
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(db));
                success = true;
                return success;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving data on json database file. Error: " + ex.Message);
                return success;
            }
        }
        /*
          * // SUMMARY
          * // Simulated IDENTITY ID management for pedidos id and item id
          * // Return: int new ID
        */
        public int GenerateID(string action, DatabaseModel DB)
        {
            int ret = 0;
            action = action.ToLower();
            switch (action)
            {
                case "item":
                    ret = DB.LastItemID;
                    DB.LastItemID++;
                    break;
                case "pedido":
                    ret = DB.LastPedidoID;
                    //DB.LastPedidoID++;
                    break;
            }
            SaveDB(DB);
            return ret;
        }

        #endregion

        #region Searchs for item
        /*
          * // SUMMARY
          * // Return all items on the JSON Database
          * // Return: List of Items
        */
        public List<Item> GetAllItems()
        {
            DBInstance = UpdateDBObject();
            return DBInstance.Items;
        }
        /*
          * // SUMMARY
          * // Searchs for a specific item with the ID received
          * // Return: DatabaseModel object
        */
        public Item SearchItembyID(int id)
        {
            Item item = new Item();
            //UpdateDBObject the databaseobject to get the most recent data
            DBInstance = UpdateDBObject();
            try
            {
                item = DBInstance.Items.Find(x => x.id == id);
                return item;
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo encontrar el item con el id especificado");
                return item;
            }
        }
        /*
          * // SUMMARY
          * // Searchs for a specific item wheres the Text coincidence from variable Name received
          * // Return: DatabaseModel object
        */
        public List<Item> SearchItembyName(string Name)
        {
            List<Item> item = new List<Item>();
            //UpdateDBObject the databaseobject to get the most recent data
            DBInstance = UpdateDBObject();
            try
            {
                //item = DBInstance.Items.Find(x => x.id == id);
                var sitem = DBInstance.Items.Where(it => it.Name.ToLower().StartsWith(Name.ToLower())).ToList();
                item = sitem;
                return item;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la busqueda : " + ex.Message);
                return item;
            }
        }
        /*
          * // SUMMARY
          * // Manage the search functions and calls the searchbyID or searchbyName depending on criteria received
          * // Return: DatabaseModel object
        */
        public List<Item> SearchByCriteria(string criteria, string data)
        {
            List<Item> List = new List<Item>();
            //UpdateDBObject the databaseobject to get the most recent data
            DBInstance = UpdateDBObject();

            if (string.IsNullOrEmpty(data))
            {
                return List = GetAllItems();
            }

            switch (criteria)
            {
                case "ID":
                    try
                    {
                        List.Add(SearchItembyID(Convert.ToInt32(data)));
                        //List.Add(SearchItembyID(Convert.ToInt32(data)));
                        List = List[0] == null ? new List<Item>() : List;

                        return List;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Error al buscar por ID, intente ingresando solo numeros de 0 a 9.");
                    }
                    break;
                case "Nombre":
                    try
                    {
                        List = SearchItembyName(data);
                        return List;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al buscar por ID error: " + ex.Message);
                    }
                    break;
            }

            return List;
        }
        #endregion

        #region JSON CRUD FOR ITEMS
        /*
          * // SUMMARY
          * // Insert data of a new item on JSON FILE writing the current data and updates the DBInstance with the new data
          * // Return: bool (True/False)
        */
        public bool CreateItem(Item Newitem, List<DataGrid> dgList, UIRuntime runtime)
        {
            bool success = false;

            //UpdateDBObject the databaseobject to get the most recent data
            DBInstance = UpdateDBObject();

            try
            {
                DBInstance.Items.Add(Newitem);
                success = SaveDB(DBInstance);
                runtime.PopulateAllDataGrids(dgList, DBInstance);
                return success;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return success;
            }

        }
        /*
          * // SUMMARY
          * // Edit or Delete Data from JSON FILE and updates the DBInstance and JSON File with the new data
          * // Return: bool (True/False)
        */
        public bool edit_delete_item(Item item, string action, List<DataGrid> dgList, UIRuntime runtime)
        {
            bool success = false;

            //UpdateDBObject the databaseobject to get the most recent data
            DBInstance = UpdateDBObject();

            try
            {
                var pastItem = DBInstance.Items.FindIndex(x => x.id == item.id);

                if (action == "edit")
                {
                    DBInstance.Items[pastItem] = item;
                    success = SaveDB(DBInstance);

                }
                else if (action == "delete")
                {
                    DBInstance.Items.RemoveAt(pastItem);
                    success = SaveDB(DBInstance);
                }
                runtime.PopulateAllDataGrids(dgList, DBInstance);
                return success;
            }
            catch (Exception es)
            {
                MessageBox.Show("Error mientras se editaba el item. Error: " + es.Message);
                return success;
            }
        }
        #endregion

        #region Pedido Functionality
        /*
           * // SUMMARY
           * // Manage the addition of an item to a current on memory pedido.
           * // Return: Void
       */
        public void addItemtoPedido(int ItemID, UIRuntime runtime)
        {
            Item item = new Item();
            //UpdateDBObject the databaseobject to get the most recent data
            DBInstance = UpdateDBObject();
            //se busca el producto que se agregara al pedido
            item = SearchItembyID(Convert.ToInt32(ItemID));
            //Hay datos en el objeto en memoria??? Si no existen datos se inicializa el nuevo pedido...
            if (DBInstance.TempPedido.id.Equals(0))
            {
                GeneratePedidoOnMemory(item, DBInstance);

                SaveDB(DBInstance);
            }
            else // Si existe un pedido en memoria comenzamos las validaciones del producto en la lista
            {
                //Si existe en la lista cargada en memoria entonces se actualiza, si no existe se agrega a la lista en memoria
                if (DBInstance.TempPedido.Items.Find(x => x.id == item.id) != null) //Ya existe el producto
                {
                    incrementItemQty(DBInstance.TempPedido, DBInstance.TempPedido.Items[DBInstance.TempPedido.Items.FindIndex(x => x.id == item.id)]);
                    DBInstance.TempPedido.Total += item.Price;
                }
                else // No existe el producto en la lista lo agregamos
                {
                    DBInstance.TempPedido.Items.Add(item);
                    DBInstance.TempPedido.ItemsQuantity.Add(new ItemQty
                    {
                        Id = item.id,
                        Price = item.Price,
                        Qty = 0
                    });
                    incrementItemQty(DBInstance.TempPedido, DBInstance.TempPedido.Items[DBInstance.TempPedido.Items.FindIndex(x => x.id == item.id)]);
                    DBInstance.TempPedido.Total += item.Price;
                }
                SaveDB(DBInstance);
            }
            runtime.updateTotals(DBInstance.TempPedido.Total);
            DrawPedido(runtime, DBInstance.TempPedido);
        }

        /*
            * // SUMMARY
            * // Generates a new pedido to store the data on memory of the pedido in progress it memory is used until the user clicks Aceptar pedido or restart the app.
            * // Return: Pedido object
        */
        public Pedido GeneratePedidoOnMemory(Item item, DatabaseModel db)
        {
            Pedido newPedido = new Pedido();
            newPedido.Name = "Sin Nombre";
            newPedido.id = new DBHandler().GenerateID("pedido", db);
            //newPedido.ItemsQuantity = 1;
            newPedido.Status = new PedidoStatus().NotRegistered;
            newPedido.Items.Add(item);
            newPedido.ItemsQuantity.Add(new ItemQty
            {
                Id = item.id,
                Price = item.Price,
                Qty = 0
            });
            newPedido.Total = item.Price;
            newPedido.Date = DateTime.Now.ToLongDateString();
            newPedido.Time = DateTime.Now.ToLongTimeString();
            newPedido.dateTime = DateTime.Now;
            incrementItemQty(newPedido, item);

            return db.TempPedido = newPedido;
        }
        /*
            * // SUMMARY
            * // Draw the visual structure of a pedido on it space on screen from a list of pedidos.
            * // Return: Void
        */
        private void DrawPedido(UIRuntime runtime, Pedido MPedido)
        {
            //var el = (Window)Application.Current.MainWindow;
            //var element = (DockPanel)el.FindName("PanelPedidos");
            var element = (DockPanel)new UIHelper().FindChildByName(Application.Current.MainWindow, "dockpanel", "PanelPedidos");
            element.Children.Clear();

            for (int i = 0; i < MPedido.Items.Count; i++)
            {
                var a = MPedido.ItemsQuantity.Where(x => x.Id.Equals(MPedido.Items[i].id)).FirstOrDefault();

                element.Children.Add(runtime.CreatePedidoPanels(MPedido.Items[i], i + 1, MPedido.ItemsQuantity.Where(x => x.Id.Equals(MPedido.Items[i].id)).FirstOrDefault()));
            }
        }
        /*
            * // SUMMARY
            * // Increment the qty of 1 item on the pedido item list.
            * // Return: Void
        */
        private void incrementItemQty(Pedido pedido, Item itemTarget)
        {
            var index = pedido.Items.FindIndex(x => x.id == itemTarget.id);

            if (!index.Equals(null) && pedido.ItemsQuantity[index].Id.Equals(itemTarget.id))
            {
                pedido.ItemsQuantity[index].Qty++;
                SaveDB(DBInstance);
            }
        }
        /*
            * // SUMMARY
            * // Decrement the qty of 1 item on the pedido item list.
            * // Return: Void
        */
        private void decrementItemQty(Pedido pedido, Item itemTarget, UIRuntime runtime)
        {
            var index = pedido.Items.FindIndex(x => x.id == itemTarget.id);

            if (!index.Equals(null) && pedido.ItemsQuantity[index].Id.Equals(itemTarget.id))
            {
                if (pedido.ItemsQuantity[index].Qty <= 1)
                {
                    //delete pedido
                    removetemfromPedido(itemTarget.id, runtime);
                }
                else
                {
                    pedido.ItemsQuantity[index].Qty--;
                    pedido.Total -= itemTarget.Price;
                }
                SaveDB(DBInstance);
                runtime.updateTotals(DBInstance.TempPedido.Total);
            }
        }
        /*
            * // SUMMARY
            * // Call private function decrementItemQty.
            * // Return: Void
        */
        public void decrementQty(int id, UIRuntime runtime)
        {
            DBInstance = UpdateDBObject();
            decrementItemQty(DBInstance.TempPedido, DBInstance.Items.Find(x => x.id.Equals(id)), runtime);
        }
        /*
            * // SUMMARY
            * // Call private function incrementItemQty.
            * // Return: Void
        */
        public void incrementqty(int id)
        {
            DBInstance = UpdateDBObject();
            incrementItemQty(DBInstance.TempPedido, DBInstance.Items.Find(x => x.id.Equals(id)));
            DBInstance.TempPedido.Total += DBInstance.Items.Find(x => x.id.Equals(id)).Price;
            SaveDB(DBInstance);
            new UIRuntime().updateTotals(DBInstance.TempPedido.Total);
        }
        /*
            * // SUMMARY
            * // Call private function incrementItemQty.
            * // Return: Void
        */
        public void CallDrawPedido(UIRuntime runtime)
        {
            DBInstance = UpdateDBObject();
            DrawPedido(runtime, DBInstance.TempPedido);
        }
        /*
            * // SUMMARY
            * // Insert a confirmed pedido on JSON DB
            * // Return: bool (true/false)
        */
        public bool CreatePedido(string name)
        {
            bool success = false;
            DBInstance = UpdateDBObject();

            if (DBInstance.TempPedido.id.Equals(0) || DBInstance.TempPedido.Items.Count.Equals(0))
            {
                MessageBox.Show("No se puede guardar un pedido vacio, por favor agregue items al pedido.");
                return false;
            }

            try
            {
                DBInstance.TempPedido.Name = name;
                DBInstance.TempPedido.Status = new PedidoStatus().Registered;
                DBInstance.Pedidos.Add(DBInstance.TempPedido);
                DBInstance.TempPedido = new Pedido();
                var element = (DockPanel)new UIHelper().FindChildByName(Application.Current.MainWindow, "dockpanel", "PanelPedidos");
                element.Children.Clear();
                new UIRuntime().updateTotals(DBInstance.TempPedido.Total);
                DBInstance.LastPedidoID++;
                SaveDB(DBInstance);
                success = true;
                MessageBox.Show("Se ha creado el pedido exitosamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha habido un error creando el pedido: " + ex.Message);
            }

            return success;
        }
        #endregion

        #region Dynamic Buttons Functionality

        /*
           * // SUMMARY
           * // Manage the delete of an item on current memory pedido.
           * // Return: Void
       */
        public void removetemfromPedido(int ItemID, UIRuntime runtime)
        {
            Item item = new Item();
            //UpdateDBObject the databaseobject to get the most recent data
            DBInstance = UpdateDBObject();
            //se busca el producto que se agregara al pedido
            item = SearchItembyID(Convert.ToInt32(ItemID));
            //Restamos el costo total del item del total del pedido
            int indexqty = DBInstance.TempPedido.ItemsQuantity.FindIndex(i => i.Id == item.id);
            DBInstance.TempPedido.Total -= (DBInstance.TempPedido.ItemsQuantity[indexqty].Qty * item.Price);
            //Elimino el item quantity correspondiente
            DBInstance.TempPedido.ItemsQuantity.RemoveAt(indexqty);
            //Se elimina el producto del pedido y tambien su quantity correspondiente
            DBInstance.TempPedido.Items.RemoveAt(DBInstance.TempPedido.Items.FindIndex(x => x.id == item.id));

            runtime.updateTotals(DBInstance.TempPedido.Total);
            SaveDB(DBInstance);
            DrawPedido(runtime, DBInstance.TempPedido);
        }

        /*
            * // SUMMARY
            * // Validate that when the software init temp pedido = new pedido.
            * // Return: Void
        */
        public void validatePedidoAtInit(DatabaseModel instance)
        {
            if (!instance.TempPedido.id.Equals(0))
            {
                instance.TempPedido = new Pedido();
                SaveDB(instance);
            }
        }
        #endregion


    } //End of way
}
