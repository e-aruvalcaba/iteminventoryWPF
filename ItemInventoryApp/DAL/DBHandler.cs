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
        private UIRuntime runtime = new UIRuntime();
        private static string fileName = @"C:\InventoryApp\_InventoryDB.json";

        public DBHandler()
        {
            DBInstance = new DatabaseModel();
            DBInstance.Items = new List<Item>();
            DBInstance.Pedidos = new List<Pedido>();
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
        public bool CreateItem(Item Newitem, List<DataGrid> dgList)
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
        public bool edit_delete_item(Item item, string action, List<DataGrid> dgList)
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


    } //End of way
}
