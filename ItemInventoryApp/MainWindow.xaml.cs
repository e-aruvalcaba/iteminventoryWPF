using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ItemInventoryApp.Classes;
using ItemInventoryApp.DAL;
using ItemInventoryApp.Models;

namespace ItemInventoryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseModel GlobalMainObject = new DatabaseModel();
        private DBHandler GlobalHandler;
        private UIRuntime UiruntimeHandler;
        private List<DataGrid> DataGridList = new List<DataGrid>();
        private Validations validationsHandler = new Validations();

        public MainWindow()
        {
            InitializeComponent();
            //Create an instance of dbhandler        
            DBHandler handler = new DBHandler();
            //Create the mainObject to retrieve de json data
            DatabaseModel MainObject = handler.InitializeDB();
            //Populate a hardcoded list of items 
            //MainObject.Items.Add(new Item {
            //    id = 1,
            //    Name = "Tacos al vapor",
            //    Description = "Paquete de 5 tacos bañados en salsa roja de habanero",
            //    Price = 25
            //});
            //MainObject.Items.Add(new Item
            //{
            //    id = 2,
            //    Name = "Tacos de pastor",
            //    Description = "Paquete de 5 tacos de pastor con cilantro cebolla caramelizada, salsa medio limon y doble tortilla.",
            //    Price = 35
            //});
            //MainObject.Items.Add(new Item
            //{
            //    id = 3,
            //    Name = "Tacos de bisteck",
            //    Description = "Paquete de 5 tacos de bisteck de res con cilantro cebolla caramelizada, salsa medio limon y doble tortilla.",
            //    Price = 40
            //});
            //MainObject.Items.Add(new Item
            //{
            //    id = 4,
            //    Name = "Papa asada Mixta",
            //    Description = "Deliciosa papa machacada con matenquilla en un contenedor de aluminio sasonada con especias finas y mezclada con queso amarillo derretido y crema y cubierta con carne de bisteck y de pastor, un chile en vinagre y 4 paquetes de galletas saladitas.",
            //    Price = 60
            //});
            //MainObject.Items.Add(new Item
            //{
            //    id = 5,
            //    Name = "Papa asada de pastor",
            //    Description = "Deliciosa papa machacada con matenquilla en un contenedor de aluminio sasonada con especias finas y mezclada con queso amarillo derretido y crema y cubierta con carne de pastor, un chile en vinagre y 4 paquetes de galletas saladitas.",
            //    Price = 45
            //});
            //MainObject.Items.Add(new Item
            //{
            //    id = 6,
            //    Name = "Papa asada de bisteck",
            //    Description = "Deliciosa papa machacada con matenquilla en un contenedor de aluminio sasonada con especias finas y mezclada con queso amarillo derretido y crema y cubierta con carne de bisteck, un chile en vinagre y 4 paquetes de galletas saladitas.",
            //    Price = 55
            //});
            //MainObject.Items.Add(new Item
            //{
            //    id = 7,
            //    Name = "Bomba",
            //    Description = "5 Quesadillas en tostada con carne de pastor, cilantro cebolla, salsa y tocino.",
            //    Price = 35
            //});
            //Create an instance of UIruntime class
            UIRuntime obj = new UIRuntime();
            //var lista = obj.CreateBorders(MainObject.Items);

            //Create a list of border UIComponent dinamically that shows the app to the final user 
            List<Border> lista = obj.CreatePanels(MainObject.Items);

            //Set the list on the content viewer
            for (int i = 0; i < lista.Count; i++)
            {
                MainViewer.Children.Add(lista[i]);
            }
            //Set the global main object
            GlobalMainObject = MainObject;
            GlobalHandler = handler;
            UiruntimeHandler = obj;

            //Populate datagrid list
            DataGridList.Add(DGEdit);
            DataGridList.Add(DGDelete);
            //Poblate DatagridView with items
            UiruntimeHandler.PopulateAllDataGrids(DataGridList, GlobalMainObject);
        }

        #region ResponsiveElementsModule
        private void CanvasDatos_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Console.WriteLine(DockMain.ActualWidth);
            int entero = 670;
            if (DockMain.ActualWidth > Convert.ToDouble(entero))
            {
                //TextoDescripcion.Width = DockMain.ActualWidth - 200;
            }
            else
            {
                //TextoDescripcion.Width = 348;
            }
        }
        #endregion

        #region CreateNewProduct
        private void BtnCreateNewProduct_Click(object sender, RoutedEventArgs e)
        {
            string richtext = new TextRange(txtDescC.Document.ContentStart, txtDescC.Document.ContentEnd).Text;
            if (!string.IsNullOrEmpty(txtBoxNombreC.Text) && !string.IsNullOrEmpty(richtext) && !string.IsNullOrEmpty(txtPrecioC.Text))
            {
                string noAccepted = "qwertyuiop´+asdfghjklñ{zxcvbnm,-!#$%%&/()=?[]*¨¨_:;¬{}¡*";

                if (!txtPrecioC.Text.Contains(noAccepted))
                {
                    if (!txtPrecioC.Equals("."))
                    {
                        GlobalMainObject = GlobalHandler.UpdateDBObject();
                        GlobalHandler.CreateItem(new Item
                        {
                            id = GlobalMainObject.Items[GlobalMainObject.Items.Count - 1].id + 1,
                            Name = txtBoxNombreC.Text,
                            Description = richtext,
                            Price = Convert.ToInt32(txtPrecioC.Text),
                            ImagePath = txtImagePahC.Text
                        });

                        List<TextBox> txtlist = new List<TextBox>();

                        txtlist.Add(txtBoxNombreC);
                        txtlist.Add(txtImagePahC);
                        txtlist.Add(txtPrecioC);
                        txtDescC.Document.Blocks.Clear();

                        UiruntimeHandler.ClearTextBoxes(txtlist);
                        GlobalMainObject = GlobalHandler.UpdateDBObject();
                        UiruntimeHandler.PopulateAllDataGrids(DataGridList, GlobalMainObject);
                    }
                    else
                    {
                        MessageBox.Show("No puede poner un punto como precio a un articulo por favor introduzca una suma real.");
                    }
                }
                else
                {
                    MessageBox.Show("Los datos introducidos en el campo de precio no son validos, recuerde que este campo es numerico y por tanto solo acepta numeros y puntos (.)");
                }
            }
            else
            {
                MessageBox.Show("Todos los campos excepto el campo para imagen deben estar llenos antes de guardar un articulo.");
            }
        }
        #endregion

        #region EditAndDeleteModule_DatagridDoubleClickEvents
        private void DGEdit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        //This is the code which helps to show the data when the row is double clicked.
                        DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                        Item dr = (Item)dgr.Item;
                        //txtIDE.Text = dr.id.ToString();
                        //txtNombreE.Text = dr.Name;
                        //txtDescE.Document.ContentStart.InsertTextInRun(dr.Description);
                        //txtPriceE.Text = dr.Price.ToString();
                        //txtImagePahE.Text = dr.ImagePath;

                        //Create List of textboxes
                        List<TextBox> txtlist = new List<TextBox>();
                        txtlist.Add(txtNombreE);
                        txtlist.Add(txtImagePahE);
                        txtlist.Add(txtPriceE);
                        txtlist.Add(txtIDE);
                        //Set text extracted from the datagrid
                        UiruntimeHandler.SetTextBoxFromDataGrid(dr, txtlist, txtDescE, "edit");
                        //Enable textboxes to edit
                        txtDescE.IsEnabled = true;
                        UiruntimeHandler.Enable_disableTextBoxes(txtlist, true);
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void DGDelete_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DataGrid grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                    {
                        //This is the code which helps to show the data when the row is double clicked.
                        DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                        Item dr = (Item)dgr.Item;

                        //Create List of textboxes
                        List<TextBox> txtlist = new List<TextBox>();
                        txtlist.Add(txtNombreDel);
                        txtlist.Add(txtImgPahDel);
                        txtlist.Add(txtPriceDel);
                        txtlist.Add(txtIDEDel);
                        //Set text extracted from the datagrid
                        UiruntimeHandler.SetTextBoxFromDataGrid(dr, txtlist, txtDescDel, "delete");
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> txtlist = new List<TextBox>();

            txtlist.Add(txtNombreE);
            txtlist.Add(txtImagePahE);
            txtlist.Add(txtPriceE);
            txtlist.Add(txtIDE);

            string richtext = new TextRange(txtDescE.Document.ContentStart, txtDescE.Document.ContentEnd).Text;
            if (!string.IsNullOrEmpty(txtNombreE.Text) && !string.IsNullOrEmpty(txtPriceE.Text))
            {
                string noAccepted = "qwertyuiop´+asdfghjklñ{zxcvbnm,-!#$%%&/()=?[]*¨¨_:;¬{}¡*";

                if (!txtPriceE.Text.Contains(noAccepted))
                {
                    if (!txtPriceE.Equals("."))
                    {
                        //Obtener el item con el id especifico
                        Item theitem = GlobalHandler.SearchItembyID(Convert.ToInt32(txtIDE.Text));

                        theitem.Name = txtNombreE.Text;
                        theitem.Description = richtext;
                        theitem.Price = Convert.ToDouble(txtPriceE.Text);
                        theitem.ImagePath = txtImagePahE.Text;

                        if (GlobalHandler.edit_delete(theitem, "edit"))
                        {
                            MessageBox.Show("Se ha editado correctamente el producto con el id: " + theitem.id);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo editar el producto con el id: " + theitem.id);
                        }


                        UiruntimeHandler.ClearTextBoxes(txtlist);
                        txtDescE.Document.Blocks.Clear();
                        txtDescE.IsEnabled = false;
                        UiruntimeHandler.Enable_disableTextBoxes(txtlist, false);
                        GlobalMainObject = GlobalHandler.UpdateDBObject();
                        UiruntimeHandler.PopulateAllDataGrids(DataGridList, GlobalMainObject);
                    }
                    else
                    {
                        MessageBox.Show("No puede poner un punto como precio a un articulo por favor introduzca una suma real.");
                    }
                }
                else
                {
                    MessageBox.Show("Los datos introducidos en el campo de precio no son validos, recuerde que este campo es numerico y por tanto solo acepta numeros y puntos (.)");
                }
            }
            else
            {
                MessageBox.Show("Todos los campos excepto el campo para imagen deben estar llenos antes de guardar un articulo.");
            }
        }

        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> txtlist = new List<TextBox>();

            txtlist.Add(txtNombreDel);
            txtlist.Add(txtImgPahDel);
            txtlist.Add(txtPriceDel);
            txtlist.Add(txtIDEDel);

            Item theitem = GlobalHandler.SearchItembyID(Convert.ToInt32(txtIDEDel.Text));

            if (GlobalHandler.edit_delete(theitem, "delete"))
            {
                MessageBox.Show("Se ha eliminado correctamente el producto con el id: " + theitem.id);
            }
            else
            {
                MessageBox.Show("No se pudo eliminar el producto con el id: " + theitem.id);
            }

            UiruntimeHandler.ClearTextBoxes(txtlist);
            txtDescDel.Document.Blocks.Clear();
            //Update the datagrids after delete
            GlobalMainObject = GlobalHandler.UpdateDBObject();
            UiruntimeHandler.PopulateAllDataGrids(DataGridList, GlobalMainObject);
        }
        #endregion

        #region ComboboxAndSearchModule
        private void ComboEliminar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var a = ((ComboBoxItem)comboEliminar.SelectedItem).Content.ToString();
            if (comboEliminar.SelectedItem != null)
            {
                btnSearchDel.IsEnabled = true;
                txtSearchDel.IsEnabled = true;
            }
            else
            {
                btnSearchDel.IsEnabled = false;
            }
        }

        private void ComboEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var a = ((ComboBoxItem)comboEliminar.SelectedItem).Content.ToString();
            if (ComboEdit.SelectedItem != null)
            {
                btnSearchE.IsEnabled = true;
                txtSearchE.IsEnabled = true;
            }
            else
            {

            }
        }
        private void BtnSearchDel_Click(object sender, RoutedEventArgs e)
        {
            if (!txtSearchDel.Text.Equals("") && comboEliminar.SelectedItem != null)
            {
                string criteria = ((ComboBoxItem)comboEliminar.SelectedItem).Content.ToString();

                var List = GlobalHandler.SearchByCriteria(criteria, txtSearchDel.Text);

                ObservableCollection<Item> datasource = new ObservableCollection<Item>();

                foreach (var item in List)
                {
                    datasource.Add(item);
                }
                DGDelete.ItemsSource = datasource;
            }
            else
            {
                if (txtSearchDel.Text.Equals("") && comboEliminar.SelectedItem == null)
                {
                    //Retrieve the original datasource for deletegrid 
                }
            }
        }

        private void BtnSearchE_Click(object sender, RoutedEventArgs e)
        {
            if (!txtSearchE.Text.Equals("") && ComboEdit.SelectedItem != null)
            {
                UiruntimeHandler.search(ComboEdit, GlobalHandler, txtSearchE.Text, DGEdit);
            }
            else
            {
                if (txtSearchE.Text.Equals("") && ComboEdit.SelectedItem == null)
                {
                    //Retrieve the original datasource for deletegrid 
                }
            }
        }
        #endregion

        #region SearchTextboxEmptyEvents
        private void TxtSearchE_KeyUp(object sender, KeyEventArgs e)
        {
            UiruntimeHandler.search(ComboEdit, GlobalHandler, txtSearchE.Text, DGEdit);
        }

        private void TxtSearchDel_KeyUp(object sender, KeyEventArgs e)
        {
            UiruntimeHandler.search(comboEliminar, GlobalHandler, txtSearchDel.Text, DGDelete);
        }


        #endregion

        #region txtBoxValidations
        private void TxtPrecioC_KeyDown(object sender, KeyEventArgs e)
        {
            //string[] accepted = new string[21];
            //accepted[0] = "D0";
            //accepted[1] = "D1";
            //accepted[2] = "D2";
            //accepted[3] = "D3";
            //accepted[4] = "D4";
            //accepted[5] = "D5";
            //accepted[6] = "D6";
            //accepted[7] = "D7";
            //accepted[8] = "D8";
            //accepted[9] = "D9";
            //accepted[10] = ".";
            //accepted[11] = "NumPad0";
            //accepted[12] = "NumPad1";
            //accepted[13] = "NumPad2";
            //accepted[14] = "NumPad3";
            //accepted[15] = "NumPad4";
            //accepted[16] = "NumPad5";
            //accepted[17] = "NumPad6";
            //accepted[18] = "NumPad7";
            //accepted[19] = "NumPad8";
            //accepted[20] = "NumPad9";

            //string noAccepted = "qwertyuiop´+asdfghjklñ{zxcvbnm,-!#$%%&/()=?[]*¨¨_:;¬{}¡*";

            //if()


        }

        #endregion

        private void TxtSearchDel_KeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void TxtSearchE_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TxtSearchE_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Console.WriteLine("Letra presionandolo " + e.Text);

            if (ComboEdit.SelectedItem != null)
            {
                string selection = ((ComboBoxItem)ComboEdit.SelectedItem).Content.ToString();
                if (selection == "ID")
                {
                    e.Handled = !validationsHandler.isNumber(e.Text);
                    //e.Handled = !validationsHandler.IsTextAllowed(e.Text);
                }
            }
        }
    } //End of the way
}
