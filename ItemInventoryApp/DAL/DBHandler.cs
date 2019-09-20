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
        }/*
          * // SUMMARY
          * // Return all pedidos on the JSON Database
          * // Return: List of Items
        */
        public List<Pedido> GetAllPedidos()
        {
            DBInstance = UpdateDBObject();
            return DBInstance.Pedidos;
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
        }        /*
          * // SUMMARY
          * // Searchs for a specific Pedido with the ID received
          * // Return: DatabaseModel object
        */
        public Pedido SearchPedidoByID(int id)
        {
            Pedido item = new Pedido();
            //UpdateDBObject the databaseobject to get the most recent data
            DBInstance = UpdateDBObject();
            try
            {
                item = DBInstance.Pedidos.Find(x => x.id == id);
                return item;
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo encontrar el Pedido con el id especificado");
                return item;
            }
        }
        /*
          * // SUMMARY
          * // Searchs for a specific PedidoS wheres the Text coincidence from variable Name received
          * // Return: DatabaseModel object
        */
        public List<Pedido> SearchPedidobyName(string Name)
        {
            List<Pedido> item = new List<Pedido>();
            //UpdateDBObject the databaseobject to get the most recent data
            DBInstance = UpdateDBObject();
            try
            {
                //item = DBInstance.Items.Find(x => x.id == id);
                var sitem = DBInstance.Pedidos.Where(it => it.Name.ToLower().StartsWith(Name.ToLower())).ToList();
                item = sitem;
                return item;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la busqueda : " + ex.Message);
                return item;
            }
        }      /*
          * // SUMMARY
          * // Searchs for a specific PedidoS wheres the Text coincidence from variable Name received
          * // Return: DatabaseModel object
        */
        public List<Pedido> SearchPedidobyDate(DateTime incio, DateTime final)
        {
            List<Pedido> item = new List<Pedido>();
            //UpdateDBObject the databaseobject to get the most recent data
            DBInstance = UpdateDBObject();
            try
            {
                //item = DBInstance.Items.Find(x => x.id == id);
                var sitem = DBInstance.Pedidos.Where(it => it.dateTime >= incio && it.dateTime <= final).ToList();
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
        public List<Pedido> SearchPedidoByCriteria(string criteria, string data)
        {
            List<Pedido> List = new List<Pedido>();
            DBInstance = UpdateDBObject();

            if (string.IsNullOrEmpty(data))
            {
                return List = GetAllPedidos();
            }

            switch (criteria)
            {
                case "ID":
                    try
                    {
                        List.Add(SearchPedidoByID(Convert.ToInt32(data)));
                        //List.Add(SearchItembyID(Convert.ToInt32(data)));
                        List = List[0] == null ? new List<Pedido>() : List;

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
                        List = SearchPedidobyName(data);
                        return List;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al buscar por ID error: " + ex.Message);
                    }
                    break;
                case "Fecha":
                    try
                    {
                        char delimiterChars = '|';
                        string[] reg = data.Split(delimiterChars);

                        DateTime date1 = Convert.ToDateTime(reg[0]);
                        DateTime date2 = Convert.ToDateTime(reg[1]);


                        List = SearchPedidobyDate(date1, date2);
                        return List;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al buscar por Fecha error: " + ex.Message);
                    }
                    break;
            }

            return List;
        }/*
          * // SUMMARY
          * // Manage the search functions and calls the searchbyID or searchbyName depending on criteria received for ITEMS
          * // Return: DatabaseModel object
        */
        public List<Item> SearchByCriteria(string criteria, string data)
        {
            List<Item> List = new List<Item>();
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
                GeneratePedidoOnMemory(item, DBInstance, "");
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
        public Pedido GeneratePedidoOnMemory(Item item, DatabaseModel db, string editID)
        {
            Pedido newPedido = new Pedido();
            newPedido.Name = "Sin Nombre";
            //if (!DBInstance.EditOn)
            //{
            newPedido.id = new DBHandler().GenerateID("pedido", db);
            //}
            //else
            //{
            //    newPedido.id = Convert.ToInt32(editID);
            //}
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
        /*
            * // SUMMARY
            * // Handle the next back pedido buttons
            * // Return: int
        */
        public void HandlePedidoToDeliver(List<Pedido> listaPedidos, int newpedidoID, string action)
        {
            Pedido p = new Pedido();
            action = action.ToLower();
            //Filtrar la lista de todos los pedidos y solo los pedidos que estan confirmados, pero no entregados.
            listaPedidos = listaPedidos.FindAll(x => x.Status.Equals(1));

            //Obtener el index del pedido que esta dibujado actualmente
            int pedidoActual = 0;

            switch (action)
            {
                case "siguiente":
                    pedidoActual = listaPedidos.FindIndex(x => x.id.Equals(newpedidoID - 1));
                    break;
                case "anterior":
                    pedidoActual = listaPedidos.FindIndex(x => x.id.Equals(newpedidoID + 1));
                    break;
            }


            //Obtener el total de los pedidos confirmados
            int totalPedidosSinConfirmar = listaPedidos.Count;
            var index = listaPedidos.FindIndex(x => x.id.Equals(newpedidoID));
            var element = (Button)new UIHelper().FindChildByName(Application.Current.MainWindow, "button", "btnSiguientePedido");
            var element2 = (Button)new UIHelper().FindChildByName(Application.Current.MainWindow, "button", "btnAnteriorPedido");

            //Pintar el nuevo pedido en pantalla

            if (action.Equals("siguiente"))
            {
                element.IsEnabled = pedidoActual + 1 >= totalPedidosSinConfirmar - 1 ? false : true;
                element2.IsEnabled = (pedidoActual + 1).Equals(0) ? false : true;
                new UIRuntime().SetNextPedidoData(listaPedidos[pedidoActual + 1].id);
                new UIRuntime().DrawSelectedPedido(listaPedidos[pedidoActual + 1]);

            }
            else
            {
                element.IsEnabled = pedidoActual - 1 >= totalPedidosSinConfirmar - 1 ? false : true;
                element2.IsEnabled = (pedidoActual - 1).Equals(0) ? false : true;
                new UIRuntime().SetNextPedidoData(listaPedidos[pedidoActual - 1].id);
                new UIRuntime().DrawSelectedPedido(listaPedidos[pedidoActual - 1]);
            }

        }

        public List<Pedido> firstValidation(List<Pedido> pedidos)
        {
            List<Pedido> list = new List<Pedido>();

            list = pedidos.FindAll(x => x.Status == 1);

            if (list.Count > 0)
            {

                var response = MessageBox.Show("Se han encontrado sin completar, ¿Desea seguir trabajando con estos pedidos?", "Warning!!!", MessageBoxButton.YesNo);

                if (response.ToString().Equals("Yes"))
                {
                    //Se cargan los pedidos en el panel de pedidos Flujo normal
                    //INICIALIZAR COLA DE PEDIDOS
                    new UIRuntime().InitPedidosQueue(pedidos);
                }
                else //Se cancelan todos los pedidos que tengan el estatus de confirmados.
                {
                    foreach (var item in list)
                    {
                        item.Status = new PedidoStatus().Canceled;
                    }
                }
            }
            DBInstance = UpdateDBObject();
            DBInstance.EditOn = false;
            SaveDB(DBInstance);
            return list;
        }
        /*
            * // SUMMARY
            * // Obtain the next confirmed pedido
            * // Return: int
        */
        public int GetNextConfirmedPedido(string index_id)
        {
            index_id.ToLower();
            int ret = 0;
            DBInstance = UpdateDBObject();

            switch (index_id)
            {
                case "id":
                    ret = DBInstance.Pedidos.Find(x => x.Status.Equals(1)).id;
                    break;
                case "index":
                    ret = DBInstance.Pedidos.FindIndex(x => x.Status.Equals(1));
                    break;
            }

            return ret;
        }

        public bool DeletePedidoFromQueue(int index)
        {
            bool success = false;

            try
            {
                DBInstance = UpdateDBObject();

                DBInstance.Pedidos[index].Status = new PedidoStatus().Canceled;
                success = SaveDB(DBInstance);

                if (GetNextConfirmedPedido("index").Equals(-1))
                {
                    new UIRuntime().ShowHidePedidoData("", "hide");
                }
                return success;
            }
            catch (Exception)
            {
                return success;
            }

        }

        public bool CompletarPedidoConfirmado(int pedidoId)
        {
            var success = false;

            if (pedidoId.Equals(0))
            {
                return success;
            }

            try
            {
                DBInstance = UpdateDBObject();

                Pedido pedido = DBInstance.Pedidos.Find(x => x.id.Equals(pedidoId));
                pedido.Status = new PedidoStatus().Completed;
                SaveDB(DBInstance);
                //Validar si quedan pedidos en la cola si no ocultar los paneles
                if (GetNextConfirmedPedido("index").Equals(-1))
                {
                    new UIRuntime().ShowHidePedidoData("", "hide");
                }
                else
                {
                    new UIRuntime().validatebtnPedidoNextAndBack(DBInstance.Pedidos, DBInstance.Pedidos[GetNextConfirmedPedido("index")].id);
                }

                return success = true;

            }
            catch (Exception)
            {

                return success;
            }
        }

        public bool LoadPedidoToEdit(Pedido pedido)
        {
            bool success = false;

            try
            {
                DBInstance = UpdateDBObject();
                DBInstance.TempPedido = pedido;
                UIRuntime runtime = new UIRuntime();
                DBInstance.EditOn = true;
                SaveDB(DBInstance);
                foreach (var item in DBInstance.TempPedido.Items)
                {
                    addItemtoPedido(item.id, runtime);
                }
                success = true;
            }
            catch (Exception)
            {
                return false;
            }

            return success;
        }

        public bool EditPedido(int PedidoID)
        {
            var success = false;

            try
            {
                DBInstance = UpdateDBObject();
                int index = DBInstance.Pedidos.FindIndex(x => x.id.Equals(PedidoID));
                DBInstance.Pedidos[index] = DBInstance.TempPedido;
                DBInstance.TempPedido = new Pedido();
                SaveDB(DBInstance);
                var element = (DockPanel)new UIHelper().FindChildByName(Application.Current.MainWindow, "dockpanel", "PanelPedidos");
                element.Children.Clear();
                UIRuntime runtime = new UIRuntime();
                runtime.updateTotals(DBInstance.TempPedido.Total);
                runtime.DrawSelectedPedido(DBInstance.Pedidos[index]);
                success = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error editando pedido.");

            }

            return success;
        }

        #region Reporting

        public IList<DGPedido> CreateDGPedidoList(List<Pedido> list)
        {
            IList<DGPedido> data = new List<DGPedido>();

            foreach (var item in list)
            {
                DGPedido newPedido = new DGPedido();

                string status = "";
                switch (item.Status)
                {
                    case 0:
                        status = "No Registrado";
                        break;
                    case 1:
                        status = "Registrado";
                        break;
                    case 2:
                        status = "Completado/Entregado";
                        break;
                    case 3:
                        status = "Cancelado";
                        break;

                    default:
                        status = "Indefinido";
                        break;
                }

                newPedido.id = item.id;
                newPedido.Name = item.Name;
                newPedido.Status = status;
                newPedido.Time = item.Time;
                newPedido.Date = item.Date;
                newPedido.dateTime = item.dateTime;
                newPedido.Total = item.Total;

                foreach (var z in item.Items)
                {
                    newPedido.Items += item.ItemsQuantity[0].Qty.ToString() + " " + z.Name + Environment.NewLine;
                }

                data.Add(newPedido);
            }
            return data;
        }
      
        public IList<DGPedido> UtilityReport(DatabaseModel db, string texto, DateTime? fecha1, DateTime? fecha2)
        {
            IList<DGPedido> list = new List<DGPedido>();

            list = CreateDGPedidoList(db.Pedidos);

            if (!fecha2.Equals(new DateTime(01, 01, 0001)))
            {
                list = list.Where(x => (x.dateTime >= fecha1.Value.Date && x.dateTime <= fecha2.Value.Date.AddDays(1)) && (x.Status != "Cancelado")).ToList();
            }
            else if (!fecha1.Equals(new DateTime(01, 01, 0001)))
            {
                list = list.Where(x => (x.dateTime.Date.Equals(fecha1.Value.Date)) && (x.Status != "Cancelado")).ToList();
            }else
            {
                list = list.Where(x => x.Status != "Cancelado").ToList();
            }

            double total = 0;

            foreach (var item in list)
            {
                total += item.Total;
            }

            list.Add(new DGPedido
            {
                id = 0,
                Name = "Total General " + texto,
                Total = total,
                dateTime = System.DateTime.Now
            });

            return list;
        }

        public IList<PopularItem> PopularProduct(DatabaseModel db, string texto, DateTime? fecha1, DateTime? fecha2)
        {
            IList<PopularItem> list = new List<PopularItem>();

            //list = CreateDGPedidoList(db.Pedidos);

            int totalItems = db.Items.Count;

            List<PopularItem> popular = new List<PopularItem>();
            List<Pedido> ped = new List<Pedido>();
            if (!fecha2.Equals(new DateTime(01, 01, 0001)))
            {
                ped = db.Pedidos.Where(x => x.dateTime > fecha1.Value.Date && x.dateTime < fecha2.Value.Date.AddDays(1)).ToList();
            }
            else if (!fecha1.Equals(new DateTime(01, 01, 0001)))
            {
                ped = db.Pedidos.Where(x => x.dateTime.Date.Equals(fecha1.Value.Date)).ToList();
            }
            else
            {
                ped = db.Pedidos;
            }

            //inicializar la lista de pedidos con total
            foreach (var item in db.Items)
            {
                popular.Add(new PopularItem {
                    id = item.id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    TotalInPedidos = 0
                });
            }

            foreach (var pedido in ped)
            {
                foreach (var item in pedido.ItemsQuantity)
                {
                    popular.Find(x => x.id.Equals(item.Id)).TotalInPedidos += 1;
                }
            }

            Comparador c = new Comparador();

            popular = popular.Where(x => !x.TotalInPedidos.Equals(0)).ToList();

            popular.Sort(c);
            popular.Reverse();

            string pedidos_popularitem="Pedidos por ID: ";
            foreach (var item in ped)
            {
                foreach (var i in item.Items)
                {
                    if (i.id == popular[0].id)
                        pedidos_popularitem += item.id.ToString() + " a nombre de: " + item.Name + "." + Environment.NewLine;
                }
            }
            
            popular.Add(new PopularItem
            {
                id = 0,
                Name = "Productos populares " + texto,
                Description = string.Format("El item mas popular es el item {0}, encontrado {1} veces en los siguientes pedidos. {2}", popular[0].Name, popular[0].TotalInPedidos, pedidos_popularitem)
            });

            Console.WriteLine(popular);
            return popular;
        }

        public string SaveReport(string type, string path, string name, string fechas)
        {
            char sp = '|';
                var a = fechas.Split(sp);
            string texto = "";
            try
            {
                if (!string.IsNullOrEmpty(a[0]) && !string.IsNullOrEmpty(a[1]))
                {
                    texto = "Del dia: " + a[0] + " al dia " + Convert.ToDateTime(a[1]).ToShortDateString();
                }
                else if (!string.IsNullOrEmpty(a[0]) && string.IsNullOrEmpty(a[1]))
                {
                    texto = "Del dia: " + Convert.ToDateTime(a[0]).ToShortDateString();
                    a[1] = null;
                }
                else
                {
                    a[0] = null;
                    a[1] = null;
                }
            }
            catch (Exception)
            {
                texto = "De todos los pedidos";
            }

            try
            {
                DBInstance = UpdateDBObject();
                IList<DGPedido> Pedidos = new List<DGPedido>();
                IList<PopularItem> Items = new List<PopularItem>();
                type = type.ToLower();

                switch (type)
                {
                    case "utilidades":
                        Pedidos = UtilityReport(DBInstance, texto, Convert.ToDateTime(a[0]), Convert.ToDateTime(a[1]));
                        var DT = new ExcelGenerator().ToDataTable<DGPedido>(Pedidos);
                        new ExcelGenerator().GenerateExcel(DT, path, name);
                        break;
                    case "producto popular":
                        Items = PopularProduct(DBInstance, texto, Convert.ToDateTime(a[0]), Convert.ToDateTime(a[1]));
                        var DTP = new ExcelGenerator().ToDataTable<PopularItem>(Items);
                        new ExcelGenerator().GenerateExcel(DTP, path, name);
                        break;
                    case "personalizado":

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Generado el reporte. Error:"+ex.Message);
            }
            return path;
        }
        #endregion

    } //End of way
}