using System;
using System.Drawing;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ItemInventoryApp.Models;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using ItemInventoryApp.DAL;
using System.Data;

namespace ItemInventoryApp.Classes
{
    class UIRuntime
    {
        public bool isEdition = false;

        public UIRuntime()
        {

        }

        #region UI Panel Creation
        /*//
            // SUMMARY
            // Create the structure of all panels where the data display
            // Retuen an array with 2 lists of ui elements borders for the panels on the ui that have as content all the data from a item list and textblock to control responsive textblock
        */
        public List<Border> CreatePanels(List<Item> dataArray)
        {
            List<Border> borderRet = new List<Border>();
            int loop = 1;
            Color color = (Color)ColorConverter.ConvertFromString("#FFC1C8E4");
            SolidColorBrush myBrush = new SolidColorBrush(color);
            foreach (var item in dataArray)
            {
                #region First DockPanel
                //Creating the main panel dockpanel
                DockPanel panel = new DockPanel();
                panel.Name = "DockMain" + item.id;
                panel.Uid = item.id.ToString();
                panel.Background = Brushes.White;

                #endregion

                #region ScrollViewer
                //Creating the scrollviewer
                ScrollViewer scroll = new ScrollViewer();
                scroll.IsTabStop = true;
                scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                #endregion

                #region StackPanel

                //Creating the stackpanel for content
                StackPanel stack = new StackPanel();
                stack.Name = "MainViewer";
                stack.Background = myBrush;
                #endregion

                #region border
                if ((loop % 2).Equals(0))
                {
                    color = (Color)ColorConverter.ConvertFromString("#E8E8E8");
                }
                else
                {
                    color = (Color)ColorConverter.ConvertFromString("#C9C8C8");
                }

                myBrush = new SolidColorBrush(color);
                Border bd = new Border();
                bd.Background = myBrush;
                //color = (Color)ColorConverter.ConvertFromString("#8860D0");
                //myBrush = new SolidColorBrush(color);
                Thickness tk = new Thickness(3.0);
                Thickness margin = new Thickness(0, 0, 0, 0);

                bd.BorderBrush = Brushes.Black;
                bd.BorderThickness = tk;
                bd.Height = 105;
                bd.Margin = margin;
                bd.Uid = "border" + item.id.ToString();
                bd.Name = "border" + item.id.ToString();

                DockPanel.SetDock(bd, Dock.Top);
                #endregion

                #region Second DockPanel
                //Creating the main panel dockpanel
                DockPanel panel2 = new DockPanel();
                panel2.Height = 100;
                #endregion

                #region 2nd border
                //color = (Color)ColorConverter.ConvertFromString("#8860D0");
                //myBrush = new SolidColorBrush(color);
                Border bd2 = new Border();
                bd2.BorderBrush = Brushes.Black;
                bd2.BorderThickness = new Thickness(0, 0, 3, 0);
                bd2.Height = 100;
                bd2.Width = 100;
                bd2.CornerRadius = new CornerRadius(5);
                bd2.Padding = new Thickness(3);

                if (item.ImagePath != "")
                {
                    Image img = new Image();
                    Uri uri = new Uri(item.ImagePath, UriKind.RelativeOrAbsolute);
                    ImageSource source = new System.Windows.Media.Imaging.BitmapImage(uri);
                    img.SetValue(Image.SourceProperty, source);
                    bd2.Child = img;
                }

                panel2.Children.Add(bd2);

                #endregion

                #region Canvas
                Canvas cnv = new Canvas();
                cnv.Name = "CanvasDatos";
                cnv.Height = 100;
                //cnv.Background = myBrush;

                //Adding the textblock to canvas
                cnv.Children.Add(new TextBlock
                {
                    Name = "TextoTitulo",
                    Padding = new Thickness(5),
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    Width = 300,
                    Text = string.Format("{0} ${1}", item.Name, item.Price.ToString())
                });
                cnv.Children.Add(new TextBlock
                {
                    Name = "TextoDescripcion",
                    Height = 70,
                    Width = 200,
                    MaxWidth = 250,
                    //HorizontalAlignment = HorizontalAlignment.Left,
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 30, 0, 0),
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    Text = item.Description
                });
                //Adding Canvas to panel 2
                panel2.Children.Add(cnv);
                #endregion

                #region 3er border

                Button dynamicButton = new Button
                {
                    Content = "Agregar",
                    Width = 70,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = 30,
                    Name = "btnAgregar" + item.id.ToString(),
                    Uid = item.id.ToString()
                };
                dynamicButton.Click += dynamicButton_Click;
                //Adding 3er border to panel2
                panel2.Children.Add(new Border
                {
                    Name = "Border3",
                    Uid = "Border3",
                    Height = 100,
                    Width = 100,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(3, 0, 0, 0),
                    CornerRadius = new CornerRadius(5),
                    Padding = new Thickness(3),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Child = dynamicButton
                });
                #endregion

                #region adding the remaining elements to the dockpanel
                bd.Child = panel2;
                #endregion                
                borderRet.Add(bd);
                loop++;
            }
            return borderRet;
        }
        /*//
            // SUMMARY
            // Create the Click event behavior to all the buttons on the panel dynamically
            // Return click event on UI (Void type)
        */
        private void dynamicButton_Click(object sender, RoutedEventArgs e)
        {
            var element = (Button)sender;
            //MessageBox.Show(element.Uid);
            DBHandler handlers = new DBHandler();

            DatabaseModel DBInstance = new DatabaseModel();
            DBInstance = handlers.UpdateDBObject();

            if (!DBInstance.EditOn || DBInstance.TempPedido.id==DBInstance.LastPedidoID)
            {
                handlers.addItemtoPedido(Convert.ToInt32(element.Uid), this);
            }
            else
            {
                MessageBox.Show("Hay un pedido en edicion o creacion en este momento, se debe completar esa tarea primeramente para poder iniciar un pedido nuevo.");
            }
        }
        #endregion

        #region Textboxes Functionality
        /*
            // SUMMARY
            // Clear all the textboxes that receives from textbox list
            // Return: Void
        */
        public void ClearTextBoxes(List<TextBox> list)
        {
            foreach (var item in list)
            {
                item.Clear();
            }
        }

        /*
            // SUMMARY
            // Enables or disables all the textboxes that receives from textbox list
            // Return: Void
        */
        public void Enable_disableTextBoxes(List<TextBox> list, bool action)
        {
            foreach (var item in list)
            {
                item.IsEnabled = action;
            }

        }
        #endregion

        #region DataGrid Operations
        /*
           // SUMMARY
           // Sets the itemSource (DataSource) of all the datagrids on the datagrid list with the data from the JSON DB
           // Return: Void
       */
        public void PopulateAllDataGrids(List<DataGrid> dgList, DatabaseModel db)
        {
            ObservableCollection<Item> data = new ObservableCollection<Item>();

            foreach (var item in db.Items)
            {
                data.Add(item);
            }

            foreach (var dg in dgList)
            {
                dg.ItemsSource = data;
            }

        }        /*
           // SUMMARY
           // Sets the itemSource (DataSource) of datagrid pedido from the JSON DB
           // Return: Void
       */
        public void PopulatePedidosDataGrid(DataGrid dg, List<Pedido> list)
        {
            ObservableCollection<DGPedido> data = new ObservableCollection<DGPedido>();
            foreach (var item in new DBHandler().CreateDGPedidoList(list))
            {
                data.Add(item);
            }
            dg.ItemsSource = data;
        }
        /*
             // SUMMARY
             // Sets the data for all the textboxes on edit or delete modules::: //DELETE MODULES DEPRECATED, NO LONGER NEED TXTBOXES
             // Return: Void
         */
        public void SetTextBoxFromDataGrid(Item item, List<TextBox> list, RichTextBox rich, string action)
        {
            string[] array = new string[4];
            switch (action)
            {
                case "edit":
                    array[0] = "txtIDE";
                    array[1] = "txtNombreE";
                    array[2] = "txtPriceE";
                    array[3] = "txtImagePahE";
                    break;
                case "delete": //DEPRECATED DELETED MODULE NO LONGER NEED TEXTBOXES TO DELETE
                    array[0] = "txtIDEDel";
                    array[1] = "txtNombreDel";
                    array[2] = "txtPriceDel";
                    array[3] = "txtImgPahDel";
                    break;
            }

            string[] values = new string[4];
            values[0] = item.id.ToString();
            values[1] = item.Name;
            values[2] = item.Price.ToString();
            values[3] = item.ImagePath;
            rich.Document.ContentStart.InsertTextInRun(item.Description);
            foreach (var it in list)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (it.Name.Equals(array[i]))
                    {
                        it.Text = values[i];
                    }
                }
            }
        }
        /*
            // SUMMARY
            // Sets the datagrid datasource with the needed data from the textbox search
            // Return: Void
        */
        public void search(ComboBox combo, DBHandler handler, string texto, DataGrid dg)
        {
            string criteria = ((ComboBoxItem)combo.SelectedItem).Content.ToString();
            var list = handler.SearchByCriteria(criteria, texto);

            try
            {
                ObservableCollection<Item> datasource = new ObservableCollection<Item>();

                foreach (var item in list)
                {
                    datasource.Add(item);
                }
                dg.ItemsSource = datasource;
            }
            catch (Exception)
            {
                list = handler.SearchByCriteria("Nombre", "");
                dg.ItemsSource = list;
            }
        }
        /*
            // SUMMARY
            // Sets the pedido datagrid datasource with the needed data from the textbox search
            // Return: Void
        */
        public void searchPedido(ComboBox combo, DBHandler handler, string texto, DataGrid dg)
        {
            string criteria = ((ComboBoxItem)combo.SelectedItem).Content.ToString();
            var list = handler.SearchPedidoByCriteria(criteria, texto);

            try
            {
                //ObservableCollection<Pedido> datasource = new ObservableCollection<Pedido>();
                PopulatePedidosDataGrid(dg, list);

                //foreach (var item in list)
                //{
                //    datasource.Add(item);
                //}
                //dg.ItemsSource = datasource;
            }
            catch (Exception)
            {
                list = handler.SearchPedidoByCriteria("Nombre", "");
                dg.ItemsSource = list;
            }
        }
        #endregion

        #region UI Pedidos Creation
        /*//
            // SUMMARY
            // Create a panel that represents 1 item on a Pedido with the data, total, qty, name of item and number of item
            // Return: Canvas element to add on a Dockpanel
        */
        public Canvas CreatePedidoPanels(Item data, int PedidoId, ItemQty qty)
        {

            var element = (DockPanel)new UIHelper().FindChildByName(Application.Current.MainWindow, "dockpanel", "PanelPedidos");
            int loop = element.Children.Count;
            Color color = (Color)ColorConverter.ConvertFromString("#FFE84040");
            SolidColorBrush myBrush = new SolidColorBrush(color);

            if ((loop % 2).Equals(0))
            {
                color = (Color)ColorConverter.ConvertFromString("#E8E8E8");
            }
            else
            {
                color = (Color)ColorConverter.ConvertFromString("#C9C8C8");
            }
            myBrush = new SolidColorBrush(color);

            #region Creating Canvas
            Canvas Canv = new Canvas();
            Canv.Name = "Item" + PedidoId;
            Canv.Height = 100;
            Canv.Background = myBrush;
            Canv.Margin = new Thickness(0, 0, 0, 0);
            DockPanel.SetDock(Canv, Dock.Top);
            #endregion

            #region Grid
            //Creating 1st Grid
            Grid Grid1 = new Grid();

            Grid1.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(100, GridUnitType.Star),
            });
            Grid1.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(100, GridUnitType.Star),
            });
            Grid1.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(100, GridUnitType.Auto),
            });

            #endregion

            #region Textboxes
            Grid Grid2 = new Grid();
            Grid2.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength()
            });
            Grid2.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(20, GridUnitType.Star)
            });
            Grid2.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(20, GridUnitType.Star)
            });

            color = (Color)ColorConverter.ConvertFromString("#FFE84040");
            myBrush = new SolidColorBrush(color);
            Button btnReduce = new Button();
            btnReduce.Content = " - ";
            btnReduce.Height = 20;
            btnReduce.Width = 20;
            btnReduce.FontFamily = new FontFamily("Comic Sans MS");
            btnReduce.Foreground = Brushes.Black;
            btnReduce.FontWeight = FontWeights.Bold;
            btnReduce.FontSize = 11;
            btnReduce.Background = myBrush; //#FFE84040
            btnReduce.Margin = new Thickness(5, 5, 5, 5);
            btnReduce.HorizontalAlignment = HorizontalAlignment.Right;
            btnReduce.VerticalAlignment = VerticalAlignment.Center;
            btnReduce.Uid = data.id.ToString();

            btnReduce.Click += reduceItemQty;
            Grid.SetColumn(btnReduce, 1);

            color = (Color)ColorConverter.ConvertFromString("#FF138F00");
            myBrush = new SolidColorBrush(color);
            Button btnAddition = new Button();
            btnAddition.Content = " + ";
            btnAddition.Height = 20;
            btnAddition.Width = 20;
            btnAddition.FontFamily = new FontFamily("Comic Sans MS");
            btnAddition.Foreground = Brushes.Black;
            btnAddition.FontWeight = FontWeights.Bold;
            btnAddition.FontSize = 11;
            btnAddition.Background = myBrush;
            btnAddition.Margin = new Thickness(5, 5, 5, 5);
            btnAddition.HorizontalAlignment = HorizontalAlignment.Right;
            btnAddition.VerticalAlignment = VerticalAlignment.Center;
            btnAddition.Click += IncrementQty;
            btnAddition.Uid = data.id.ToString();

            Grid.SetColumn(btnAddition, 2);

            Grid.SetRow(Grid2, 0);

            TextBlock Texto2 = new TextBlock();
            Texto2.FontSize = 12;
            Texto2.FontWeight = FontWeights.Bold;
            Texto2.Padding = new Thickness(3);
            Texto2.VerticalAlignment = VerticalAlignment.Center;
            Texto2.HorizontalAlignment = HorizontalAlignment.Right;
            Texto2.Text = "Cantidad x " + qty.Qty;
            Texto2.Name = "Texto2id_" + data.id;
            //Grid.SetRow(Texto2, 0);
            Grid.SetColumn(Texto2, 0);
            DockPanel.SetDock(Texto2, Dock.Top);

            TextBlock Texto3 = new TextBlock();
            Texto3.FontSize = 13;
            Texto3.Padding = new Thickness(3);
            Texto3.HorizontalAlignment = HorizontalAlignment.Left;
            Texto3.Text = data.Description;
            Texto3.Height = 45;
            Texto3.Width = 160;
            Texto3.TextWrapping = TextWrapping.WrapWithOverflow;
            Texto3.Text = data.Name;
            Texto3.Name = "Texto3id_" + data.id;
            Grid.SetRow(Texto3, 1);
            DockPanel.SetDock(Texto3, Dock.Top);

            color = (Color)ColorConverter.ConvertFromString("#232D33");
            myBrush = new SolidColorBrush(color);
            TextBlock Texto4 = new TextBlock();
            Texto4.FontSize = 12;
            Texto4.Padding = new Thickness(2);
            Texto4.HorizontalAlignment = HorizontalAlignment.Right;
            Texto4.Text = string.Format("Total: ${0} pesos", (qty.Qty * qty.Price));
            Texto4.TextWrapping = TextWrapping.WrapWithOverflow;
            Texto4.Text = "Total: $" + qty.Price * qty.Qty;
            Texto4.Name = "Texto4id_" + data.id;
            Texto4.Background = myBrush;
            Texto4.Foreground = Brushes.White;
            Texto4.FontWeight = FontWeights.Bold;
            Texto4.Height = 20;
            Grid.SetRow(Texto4, 2);
            DockPanel.SetDock(Texto4, Dock.Top);

            Button btnEliminar = new Button();
            btnEliminar.Content = "Eliminar";
            btnEliminar.Height = 20;
            btnEliminar.Width = 50;
            btnEliminar.FontFamily = new FontFamily("Comic Sans MS");
            btnEliminar.Foreground = Brushes.White;
            btnEliminar.FontWeight = FontWeights.Bold;
            btnEliminar.FontSize = 11;
            btnEliminar.Background = Brushes.Black;
            btnEliminar.Margin = new Thickness(15, 5, 5, 5);
            btnEliminar.HorizontalAlignment = HorizontalAlignment.Left;
            btnEliminar.VerticalAlignment = VerticalAlignment.Center;
            btnEliminar.Click += deleteItemfromPedido;
            btnEliminar.Uid = data.id.ToString();
            Grid.SetRow(btnEliminar, 2);
            #endregion

            #region Adding elements to canvas

            //Grid1.Children.Add(Texto1);
            Grid2.Children.Add(btnReduce);
            Grid2.Children.Add(btnAddition);
            Grid2.Children.Add(Texto2);
            Grid1.Children.Add(Grid2);
            //Grid1.Children.Add(Texto2);
            Grid1.Children.Add(Texto3);
            Grid1.Children.Add(Texto4);
            Grid1.Children.Add(btnEliminar);
            Canv.Children.Add(Grid1);
            return Canv;
            #endregion
        }
        /*//
           // SUMMARY
           // Increment on 1 the qty of selected item
           // Return: Void
        */

        private void IncrementQty(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Increment item");
            var s = (Button)sender;
            DBHandler h = new DBHandler();
            h.incrementqty(Convert.ToInt32(s.Uid));
            redrawProcess(h);
        }
        /*//
           // SUMMARY
           // Decrement on 1 the qty of selected item
           // Return: Void
        */
        private void reduceItemQty(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Reduce Qty item");
            var s = (Button)sender;
            DBHandler h = new DBHandler();
            h.decrementQty(Convert.ToInt32(s.Uid), this);
            redrawProcess(h);
        }

        private void redrawProcess(DBHandler h)
        {
            List<Canvas> list = new List<Canvas>();
            DatabaseModel DB = new DatabaseModel();
            DB = h.UpdateDBObject();
            foreach (var item in DB.TempPedido.Items)
            {
                list.Add(CreatePedidoPanels(item, DB.TempPedido.id, DB.TempPedido.ItemsQuantity.Find(x => x.Id.Equals(item.id))));
            }
            object[] obj = new object[1];
            obj[0] = list;
            redraw(new UIHelper().FindChildByName(Application.Current.MainWindow, "dockpanel", "PanelPedidos"), "dockpanel", obj, "canvas");
        }
        /*//
           // SUMMARY
           // Deletes one item from the pedido ignoring the Qty of the item
           // Return: Void
        */
        private void deleteItemfromPedido(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Eliminar item");
            var s = (Button)sender;
            DBHandler h = new DBHandler();
            DatabaseModel DB = new DatabaseModel();
            DB = h.UpdateDBObject();

            h.removetemfromPedido(Convert.ToInt32(s.Uid), this);
            
        }
        #endregion

        #region UI re-draw
        /*//
            // SUMMARY
            // Redraw the elements of pedidos or items on its respective container in the UI
            // Return: Void
        */
        public void redraw(UIElement element, string typeElement, object[] list, string type)
        {
            typeElement = typeElement.ToLower();
            type = type.ToLower();
            List<Border> lista = new List<Border>();
            List<Canvas> listaC = new List<Canvas>();
            switch (type)
            {
                case "border":
                    lista = (List<Border>)list[0];
                    break;
                case "canvas":
                    listaC = (List<Canvas>)list[0];
                    break;

            }

            switch (typeElement)
            {
                case "dockpanel":
                    var nElement = (DockPanel)element;
                    nElement.Children.Clear();

                    foreach (var item in listaC)
                    {
                        nElement.Children.Add(item);
                    }
                    break;
                case "stackpanel":
                    var nElement2 = (StackPanel)element;
                    nElement2.Children.Clear();

                    foreach (var item in lista)
                    {
                        nElement2.Children.Add(item);
                    }
                    break;
            }
        }
        #endregion

        #region TotalManagement

        public void updateTotals(double total)
        {
            var element = (TextBlock)new UIHelper().FindChildByName(Application.Current.MainWindow, "textblock", "TotalPedido");
            element.Text = "$ " + total.ToString();
            var totaldesc = (TextBlock)new UIHelper().FindChildByName(Application.Current.MainWindow, "textblock", "descTotalPedido");
            var btnConfirmarPedido = (Button)new UIHelper().FindChildByName(Application.Current.MainWindow, "button", "ConfirmarPedido");

            if (total != 0)
            {
                totaldesc.Visibility = Visibility.Visible;
                btnConfirmarPedido.IsEnabled = true;

            }
            else
            {
                totaldesc.Visibility = Visibility.Hidden;
                btnConfirmarPedido.IsEnabled = false;
            }
        }

        #endregion

        #region UI Cola de pedidos Creation

        public List<Grid> CreateListaPedido(Pedido pedido)
        {

            List<Grid> list = new List<Grid>();
            int loop = 1;
            Color color = (Color)ColorConverter.ConvertFromString("#C9C8C8");
            SolidColorBrush myBrush = new SolidColorBrush(color);

            foreach (var item in pedido.Items)
            {
                if ((loop % 2).Equals(0))
                {
                    color = (Color)ColorConverter.ConvertFromString("#E8E8E8");
                }
                else
                {
                    color = (Color)ColorConverter.ConvertFromString("#C9C8C8");
                }

                myBrush = new SolidColorBrush(color);

                #region Grid
                //Creating 1st Grid
                Grid Grid1 = new Grid();

                var desc = pedido.Items.Find(x => x.id.Equals(item.id)).Description;

                Grid1.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(100, GridUnitType.Auto),
                    ToolTip = desc
                });
                Grid1.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Pixel),
                    ToolTip = desc
                });

                Grid1.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(20, GridUnitType.Pixel),
                });
                Grid1.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star),
                });

                DockPanel.SetDock(Grid1, Dock.Top);
                #endregion

                #region broder1Creation
                Border bd1 = new Border();
                bd1.Background = myBrush;
                bd1.Child = new TextBlock
                {
                    Foreground = Brushes.Black,
                    Margin = new Thickness(0),
                    FontSize = 12.5,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Text = pedido.ItemsQuantity.Find(x => x.Id.Equals(item.id)).Qty.ToString(),
                };

                Grid.SetRow(bd1, 0);
                Grid.SetColumn(bd1, 0);
                Grid1.Children.Add(bd1);
                #endregion

                #region broder2Creation
                Border bd2 = new Border();
                bd2.Background = myBrush;
                bd2.Child = new TextBlock
                {
                    Width = 120,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(0),
                    FontSize = 10.5,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Text = pedido.Items.Find(x => x.id.Equals(item.id)).Name,
                    TextWrapping = TextWrapping.WrapWithOverflow
                };

                Grid.SetRow(bd2, 0);
                Grid.SetColumn(bd2, 1);
                Grid1.Children.Add(bd2);
                #endregion
                loop++;
                list.Add(Grid1);
            }
            return list;
        }
        /*//
           // SUMMARY
           // Draw 1 pedido on the panel
           // Return: Void
        */
        public void DrawSelectedPedido(Pedido selectedPedido)
        {
            List<Grid> elements = CreateListaPedido(selectedPedido);

            ShowHidePedidoData(selectedPedido.Name, "show");
            DockPanel ViewElement = (DockPanel)new UIHelper().FindChildByName(Application.Current.MainWindow, "dockpanel", "CurrentPedidoInfo");
            ViewElement.Children.Clear();
            SetNextPedidoData(selectedPedido.id);

            foreach (var item in elements)
            {
                ViewElement.Children.Add(item);
            }
        }

        public void ShowHidePedidoData(string name, string action)
        {

            action.ToLower();
            DockPanel ViewElement = (DockPanel)new UIHelper().FindChildByName(Application.Current.MainWindow, "dockpanel", "CurrentPedidoInfo");
            ViewElement.Children.Clear();
            Grid PedidoViewer = (Grid)new UIHelper().FindChildByName(Application.Current.MainWindow, "grid", "PedidoViewer");
            TextBlock PedidoName = (TextBlock)new UIHelper().FindChildByName(Application.Current.MainWindow, "textblock", "textNombrePedido");
            PedidoName.Text = name;

            switch (action)
            {
                case "show":
                    PedidoViewer.Visibility = Visibility.Visible;
                    break;

                case "hide":
                    PedidoViewer.Visibility = Visibility.Hidden;
                    break;
            }
        }

        /*//
           // SUMMARY
           // Sets the pedido id that current drawed on the pedidos panel to get the info from other ui components
           // Return: Void
        */
        public void SetNextPedidoData(int pedidoID)
        {
            DockPanel element = (DockPanel)new UIHelper().FindChildByName(Application.Current.MainWindow, "dockpanel", "CurrentPedidoInfo");
            element.Uid = pedidoID.ToString();
        }

        public void InitPedidosQueue(List<Pedido> pedidos)
        {
            pedidos = pedidos.Where(x => x.Status.Equals(1)).ToList();

            DrawSelectedPedido(pedidos[0]);
            //Setear datos del pedido en el panel
            SetNextPedidoData(pedidos[0].id);
            //Generar las validaciones de los botones editar, eliminar, siguiente, y atras.

            var btn = (Button)new UIHelper().FindChildByName(Application.Current.MainWindow, "button", "btnEntregarPedido");
            btn.IsEnabled = true;

            if (pedidos.Count > 1)
            {
                btn = (Button)new UIHelper().FindChildByName(Application.Current.MainWindow, "button", "btnSiguientePedido");
                btn.IsEnabled = true;
            }
        }

        public void validatebtnPedidoNextAndBack(List<Pedido> list, int CurrentPedidoID)
        {
            list = list.Where(x => x.Status.Equals(1)).ToList();

            var element = (Button)new UIHelper().FindChildByName(Application.Current.MainWindow, "button", "btnSiguientePedido");
            var element2 = (Button)new UIHelper().FindChildByName(Application.Current.MainWindow, "button", "btnAnteriorPedido");

            int id = CurrentPedidoID;
            CurrentPedidoID = list.FindIndex(x => x.id.Equals(CurrentPedidoID));

            bool siguiente = false;
            try
            {
                var validate = list[CurrentPedidoID + 1];
                siguiente = true;
            }
            catch (Exception)
            {
                siguiente = false;
            }

            if (true)
            {
                element.IsEnabled = siguiente ? true : false;
                element2.IsEnabled = list[0].id.Equals(id) ? false : true;
            }

        }
        #endregion

        #region CopyImagetoLocal

        public bool CopyImagetoLocal()
        {
            return true;
        }

        #endregion

        #region FechaComboBoxPopulate
        /*
            // SUMMARY
            // Sets the values on Pedido FechaCombobox to identify the hrs to search on the query
            // Return: Void
        */
        public void PopulateComboFecha(ComboBox combo, int horainicio, string display)
        {
            if (horainicio > 23)
            {
                MessageBox.Show("Error solo puede mandar horas de 0 a 23 para inciializar el combo box");
            }

            display.ToLower();
            string[] displayMember = new string[24];
            switch (display)
            {
                case "inicio":
                    for (int i = 0; i < 24; i++)
                    {
                        if (i < 10)
                        {
                            displayMember[i] = "0" + i + ":00";
                        }
                        else
                        {
                            displayMember[i] = i + ":00";
                        }
                    }
                    break;
                case "final":
                    for (int i = horainicio; i < 24; i++)
                    {
                        if (i < 10)
                        {
                            displayMember[i] = "0" + i + ":59";
                        }
                        else
                        {
                            displayMember[i] = i + ":59";
                        }
                    }
                    break;
            }


            //Dictionary<int, string> Dictionary = new Dictionary<int, string>();
            DataTable dt = new DataTable();
            dt.Columns.Add("c1", typeof(int));
            dt.Columns.Add("c2");
            //List<int> horas = new List<int>();

            for (int i = horainicio; i < 24; i++)
            {
                dt.Rows.Add(i, displayMember[i]);
            }

            combo.ItemsSource = dt.DefaultView;
            combo.DisplayMemberPath = "c2";
            combo.SelectedValuePath = "c1";

        }

        #endregion
    } //End limit of code
}
