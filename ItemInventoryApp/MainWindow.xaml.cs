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
        //Dependency injection for all librarys
        Library Global = new Library();
        List<Border> borderList = new List<Border>();
        List<TextBlock> textBlock = new List<TextBlock>();

        public MainWindow()
        {
            InitializeComponent();
            Global.DatbaseInstance = new DatabaseModel();
            Global.DBHandler = new DBHandler();
            Global.UIRuntime = new UIRuntime();
            Global.DataGridList = new List<DataGrid>();
            Global.TextBoxList = new List<TextBox>();
            Global.ValidationsHandler = new Validations();
            //Create an instance of dbhandler        
            DBHandler handler = new DBHandler();
            //Create the mainObject to retrieve de json data
            DatabaseModel MainObject = handler.InitializeDB();
            //Create an instance of UIruntime class
            UIRuntime obj = new UIRuntime();
            //Create a list of border UIComponent dinamically that shows the app to the final user 
            /*List<Border> */
            borderList = obj.CreatePanels(MainObject.Items);
            //Set the list on the content viewer
            for (int i = 0; i < borderList.Count; i++)
            {
                MainViewer.Children.Add(borderList[i]);
            }
            //Set the global main object
            Global.DatbaseInstance = MainObject;
            Global.DBHandler = handler;
            Global.UIRuntime = obj;

            //Populate datagrid list
            Global.DataGridList.Add(DGEdit);
            Global.DataGridList.Add(DGDelete);
            //Poblate DatagridView with items
            Global.UIRuntime.PopulateAllDataGrids(Global.DataGridList, Global.DatbaseInstance);
        }

        #region ResponsiveElementsModule
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Console.WriteLine(DockMain.ActualWidth);
            int entero = 670;
            if (DockMain.ActualWidth > Convert.ToDouble(entero))
            {
                foreach (var item in borderList)
                {
                    var element = LogicalTreeHelper.FindLogicalNode(item, "TextoDescripcion");
                    if (element != null)
                    {
                        var newelement = (TextBlock)element;
                        newelement.Width = DockMain.ActualWidth - 200;
                        newelement.MaxWidth = newelement.Width;
                    }
                }
            }
            else
            {
                foreach (var item in borderList)
                {
                    var element = LogicalTreeHelper.FindLogicalNode(item, "TextoDescripcion");
                    if (element != null)
                    {
                        var newelement = (TextBlock)element;
                        newelement.Width = 300;
                        newelement.MaxWidth = newelement.Width;
                    }
                }
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

                        try
                        {
                            Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
                            if(Global.DBHandler.CreateItem(new Item
                            {
                                id = Global.DatbaseInstance.Items[Global.DatbaseInstance.Items.Count - 1].id + 1,
                                Name = txtBoxNombreC.Text,
                                Description = richtext,
                                Price = Convert.ToInt32(txtPrecioC.Text),
                                ImagePath = txtImagePahC.Text
                            }, Global.DataGridList))
                            {
                                List<TextBox> txtlist = new List<TextBox>();

                                txtlist.Add(txtBoxNombreC);
                                txtlist.Add(txtImagePahC);
                                txtlist.Add(txtPrecioC);
                                txtDescC.Document.Blocks.Clear();

                                Global.UIRuntime.ClearTextBoxes(txtlist);
                                Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
                                Global.UIRuntime.PopulateAllDataGrids(Global.DataGridList, Global.DatbaseInstance);
                                MessageBox.Show("Se ha creado exitosamente el nuevo producto.");
                            }
                            else
                            {
                                MessageBox.Show("No se pudo crear el nuevo producto.");
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error creando nuevo producto: "+ex.Message);
                        }

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

                        //Create List of textboxes
                        List<TextBox> txtlist = new List<TextBox>();
                        txtlist.Add(txtNombreE);
                        txtlist.Add(txtImagePahE);
                        txtlist.Add(txtPriceE);
                        txtlist.Add(txtIDE);
                        //Set text extracted from the datagrid
                        Global.UIRuntime.SetTextBoxFromDataGrid(dr, txtlist, txtDescE, "edit");
                        //Enable textboxes to edit
                        txtDescE.IsEnabled = true;
                        Global.UIRuntime.Enable_disableTextBoxes(txtlist, true);
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
                        var id = dr.id;
                        if ( MessageBox.Show("Eliminar el registro con ID: " + id + "?", "Caption", MessageBoxButton.OKCancel).ToString().Equals("OK"))
                        {
                            if (Global.DBHandler.edit_delete_item(dr, "delete", Global.DataGridList))
                            {
                                MessageBox.Show("Se elimino correctamente el registro con el id: "+id);
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar el registro con el id: " + id);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        //private void DGDelete_MouseDoubleClick(object sender, MouseButtonEventArgs e) //This code is to enable the form on delete zone
        //{
        //    try
        //    {
        //        if (sender != null)
        //        {
        //            DataGrid grid = sender as DataGrid;
        //            if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
        //            {
        //                //This is the code which helps to show the data when the row is double clicked.
        //                DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
        //                Item dr = (Item)dgr.Item;

        //                //Create List of textboxes
        //                List<TextBox> txtlist = new List<TextBox>();
        //                txtlist.Add(txtNombreDel);
        //                txtlist.Add(txtImgPahDel);
        //                txtlist.Add(txtPriceDel);
        //                txtlist.Add(txtIDEDel);
        //                //Set text extracted from the datagrid
        //                Global.UIRuntime.SetTextBoxFromDataGrid(dr, txtlist, txtDescDel, "delete");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //    }
        //}

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
                        Item theitem = Global.DBHandler.SearchItembyID(Convert.ToInt32(txtIDE.Text));

                        theitem.Name = txtNombreE.Text;
                        theitem.Description = richtext;
                        theitem.Price = Convert.ToDouble(txtPriceE.Text);
                        theitem.ImagePath = txtImagePahE.Text;

                        if (Global.DBHandler.edit_delete_item(theitem, "edit", Global.DataGridList))
                        {
                            MessageBox.Show("Se ha editado correctamente el producto con el id: " + theitem.id);
                        }
                        else
                        {
                            MessageBox.Show("No se pudo editar el producto con el id: " + theitem.id);
                        }


                        Global.UIRuntime.ClearTextBoxes(txtlist);
                        txtDescE.Document.Blocks.Clear();
                        txtDescE.IsEnabled = false;
                        Global.UIRuntime.Enable_disableTextBoxes(txtlist, false);
                        Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
                        Global.UIRuntime.PopulateAllDataGrids(Global.DataGridList, Global.DatbaseInstance);
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

        //private void BtnDeleteItem_Click(object sender, RoutedEventArgs e)
        //{
        //    List<TextBox> txtlist = new List<TextBox>();

        //    txtlist.Add(txtNombreDel);
        //    txtlist.Add(txtImgPahDel);
        //    txtlist.Add(txtPriceDel);
        //    txtlist.Add(txtIDEDel);

        //    Item theitem = Global.DBHandler.SearchItembyID(Convert.ToInt32(txtIDEDel.Text));

        //    if (Global.DBHandler.edit_delete_item(theitem, "delete"))
        //    {
        //        MessageBox.Show("Se ha eliminado correctamente el producto con el id: " + theitem.id);
        //    }
        //    else
        //    {
        //        MessageBox.Show("No se pudo eliminar el producto con el id: " + theitem.id);
        //    }

        //    Global.UIRuntime.ClearTextBoxes(txtlist);
        //    txtDescDel.Document.Blocks.Clear();
        //    //Update the datagrids after delete
        //    Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
        //    Global.UIRuntime.PopulateAllDataGrids(Global.DataGridList, Global.DatbaseInstance);
        //}
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

                var List = Global.DBHandler.SearchByCriteria(criteria, txtSearchDel.Text);

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
                Global.UIRuntime.search(ComboEdit, Global.DBHandler, txtSearchE.Text, DGEdit);
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
            Global.UIRuntime.search(ComboEdit, Global.DBHandler, txtSearchE.Text, DGEdit);
        }

        private void TxtSearchDel_KeyUp(object sender, KeyEventArgs e)
        {
            Global.UIRuntime.search(comboEliminar, Global.DBHandler, txtSearchDel.Text, DGDelete);
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

        #region Validation for searchTextboxes
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
                    e.Handled = !Global.ValidationsHandler.isNumber(e.Text);
                    //e.Handled = !Global.ValidationsHandler.IsTextAllowed(e.Text);
                }
            }
        }
        #endregion

    } //End of the way
}
