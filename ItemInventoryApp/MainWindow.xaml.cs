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
            Global.Elements = new List<UIElement>();
            //Create an instance of dbhandler        

            //Create the mainObject to retrieve de json data
            Global.DatbaseInstance = Global.DBHandler.InitializeDB();
            //Create an instance of UIruntime class
            UIRuntime obj = new UIRuntime();
            //Create a list of border UIComponent dinamically that shows the app to the final user 
            /*List<Border> */
            borderList = obj.CreatePanels(Global.DatbaseInstance.Items);
            //Set the list on the content viewer
            for (int i = 0; i < borderList.Count; i++)
            {
                MainViewer.Children.Add(borderList[i]);
            }
            //Set the global main object
            //Global.DatbaseInstance = MainObject;
            Global.UIRuntime = obj;

            //Populate datagrid list
            Global.DataGridList.Add(DGEdit);
            Global.DataGridList.Add(DGDelete);
            //Poblate DatagridView with items
            Global.UIRuntime.PopulateAllDataGrids(Global.DataGridList, Global.DatbaseInstance);
            Global.Elements.Add(PanelPedidos);

            //First Validation to the pedidos in progress
            Global.DBHandler.firstValidation(Global.DatbaseInstance.Pedidos);
            //textPedidoInfo.Text = "5 x Orden de tacos de bisteck."+Environment.NewLine+"3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion" + Environment.NewLine + "3 Hamburguesa con queso Promocion";
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
                    var element =  (TextBlock)LogicalTreeHelper.FindLogicalNode(item, "TextoDescripcion");
                    var element2 = (TextBlock)LogicalTreeHelper.FindLogicalNode(item, "TextoTitulo");

                    if (element != null)
                    {
                        element.Width = DockMain.ActualWidth - 200;
                        element.MaxWidth = element.Width;
                        element2.Width = DockMain.ActualWidth - 200;
                        element2.MaxWidth = element2.Width;
                    }
                }
            }
            else
            {
                foreach (var item in borderList)
                {
                    var element = (TextBlock)LogicalTreeHelper.FindLogicalNode(item, "TextoDescripcion");
                    var element2 = (TextBlock)LogicalTreeHelper.FindLogicalNode(item, "TextoTitulo");
                    if (element != null)
                    {
                        element.Width = 300;
                        element.MaxWidth = element.Width;
                        element2.Width = 300;
                        element2.MaxWidth = element2.Width;
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
                            //FixMe cambiar esta logica al DBHndler
                            Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
                            if (Global.DBHandler.CreateItem(new Item
                            {
                                //id = Global.DatbaseInstance.Items.Count.Equals(0) ? 1 : Global.DatbaseInstance.Items[Global.DatbaseInstance.Items.Count].id + 1,
                                id = Global.DBHandler.GenerateID("item", Global.DatbaseInstance),
                                Name = txtBoxNombreC.Text,
                                Description = richtext,
                                Price = Convert.ToInt32(txtPrecioC.Text),
                                ImagePath = txtImagePahC.Text
                            }, Global.DataGridList, Global.UIRuntime))
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

                                //Actualizar la vista principal //ACOMODAR LOGICA EN OTRA CLASE
                                var obj = new object[1];
                                borderList = Global.UIRuntime.CreatePanels(Global.DatbaseInstance.Items);
                                obj[0] = borderList;
                                Global.UIRuntime.redraw(MainViewer, "stackpanel", obj, "border");
                            }
                            else
                            {
                                MessageBox.Show("No se pudo crear el nuevo producto.");
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error creando nuevo producto: " + ex.Message);
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
                        if (MessageBox.Show("Eliminar el registro con ID: " + id + "?", "Caption", MessageBoxButton.OKCancel).ToString().Equals("OK"))
                        {
                            if (Global.DBHandler.edit_delete_item(dr, "delete", Global.DataGridList, Global.UIRuntime))
                            {
                                MessageBox.Show("Se elimino correctamente el registro con el id: " + id);
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

                        if (Global.DBHandler.edit_delete_item(theitem, "edit", Global.DataGridList, Global.UIRuntime))
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

        #region Validation for searchTextboxes
        private void TxtSearchE_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Console.WriteLine("Letra presionandolo " + e.Text);

            if (ComboEdit.SelectedItem != null)
            {
                string selection = ((ComboBoxItem)ComboEdit.SelectedItem).Content.ToString();
                if (selection == "ID")
                {
                    e.Handled = !Global.ValidationsHandler.isNumber(e.Text);
                }
            }
        }

        private void TxtSearchDel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (comboEliminar.SelectedItem != null)
            {
                string selection = ((ComboBoxItem)comboEliminar.SelectedItem).Content.ToString();
                if (selection == "ID")
                {
                    e.Handled = !Global.ValidationsHandler.isNumber(e.Text);
                }
            }
        }

        private void TxtPrecioC_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Global.ValidationsHandler.isNumber(e.Text);
        }

        private void TxtPriceE_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Global.ValidationsHandler.isNumber(e.Text);
        }

        #endregion

        #region Pedido Module
        private void ConfirmarPedido_Click(object sender, RoutedEventArgs e)
        {
            Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
            if (!Global.DatbaseInstance.EditOn)
            {
                InputBox.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                Global.DBHandler.EditPedido(Global.DatbaseInstance.TempPedido.id);
            }
        }



        #endregion




        #region PROMPT

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            // YesButton Clicked! Let's hide our InputBox and handle the input text.
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            // Do something with the Input
            String input = InputTextBox.Text;

            if (!string.IsNullOrEmpty(input))
            {
                Global.DBHandler.CreatePedido(input);
            }
            else
            {
                Global.DBHandler.CreatePedido("Sin Nombre");
            }

            //Validar si el padre de los pedidos tiene hijos activos

            var element = (DockPanel)new UIHelper().FindChildByName(Application.Current.MainWindow, "dockpanel", "CurrentPedidoInfo");
            Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
            if (element.Children.Count.Equals(0))
            {
                new UIRuntime().InitPedidosQueue(Global.DatbaseInstance.Pedidos);
            }
            else
            {
                //Validar botones siguiente y atras
                Global.UIRuntime.validatebtnPedidoNextAndBack(Global.DatbaseInstance.Pedidos, Convert.ToInt32(CurrentPedidoInfo.Uid));
            }

            // Clear InputBox.
            InputTextBox.Text = String.Empty;
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            // NoButton Clicked! Let's hide our InputBox.
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            // Clear InputBox.
            InputTextBox.Text = String.Empty;
        }

        #endregion
        private void BtnSiguientePedido_Click(object sender, RoutedEventArgs e)
        {
            Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
            Global.DBHandler.HandlePedidoToDeliver(Global.DatbaseInstance.Pedidos, Convert.ToInt32(CurrentPedidoInfo.Uid)+1, "siguiente");
        }

        private void BtnAnteriorPedido_Click(object sender, RoutedEventArgs e)
        {
            Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
            Global.DBHandler.HandlePedidoToDeliver(Global.DatbaseInstance.Pedidos, Convert.ToInt32(CurrentPedidoInfo.Uid)-1, "anterior");
        }

        private void BtnEliminarPedidoEnCola_Click(object sender, RoutedEventArgs e)
        {
            Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
            //Global.DatbaseInstance.Pedidos.RemoveAt();
            //Pintar el primer pedido disponible o limpiar en caso de que no encuentre
            try
            {
                if(Global.DBHandler.DeletePedidoFromQueue(Global.DatbaseInstance.Pedidos.FindIndex(x => x.id.Equals(Convert.ToInt32(CurrentPedidoInfo.Uid)))))
                {
                    MessageBox.Show("Se ha eliminado correctamente el pedido de la lista de pedidos confirmados.");
                }
                Global.UIRuntime.DrawSelectedPedido(Global.DatbaseInstance.Pedidos[Global.DBHandler.GetNextConfirmedPedido("index")]);
                Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
                Global.UIRuntime.validatebtnPedidoNextAndBack(Global.DatbaseInstance.Pedidos, Convert.ToInt32(CurrentPedidoInfo.Uid));
            }
            catch (Exception ex)
            {
                var element = (DockPanel)new UIHelper().FindChildByName(Application.Current.MainWindow, "dockpanel", "CurrentPedidoInfo");
                element.Children.Clear();
                btnEntregarPedido.IsEnabled = false;
            }
        }

        private void BtnEditarPedidoEnCola_Click(object sender, RoutedEventArgs e)
        {
            Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
            Pedido pedido = Global.DatbaseInstance.Pedidos.Find(x => x.id.Equals(Convert.ToInt32(new UIHelper().FindChildByName(Application.Current.MainWindow, "dockpanel", "CurrentPedidoInfo").Uid)));

            Global.DBHandler.LoadPedidoToEdit(pedido);
        }

        private void BtnEntregarPedido_Click(object sender, RoutedEventArgs e)
        {
            //Cambiar estatus del pedido de 1 a 2
            if (Global.DBHandler.CompletarPedidoConfirmado(Convert.ToInt32(CurrentPedidoInfo.Uid)))
            {
                MessageBox.Show("El pedido fue completado exitosamente.");
                Global.DatbaseInstance = Global.DBHandler.UpdateDBObject();
                if (!Global.DBHandler.GetNextConfirmedPedido("index").Equals(-1))
                {
                    Global.UIRuntime.InitPedidosQueue(Global.DatbaseInstance.Pedidos);
                }
                else
                {
                    btnEntregarPedido.IsEnabled = false;
                }
            }
            else
            {
                MessageBox.Show("Error completando el pedido.");
            }

        }




    } //End of the way
}