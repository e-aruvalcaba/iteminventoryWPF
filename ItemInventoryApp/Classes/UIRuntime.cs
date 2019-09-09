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

namespace ItemInventoryApp.Classes
{
    class UIRuntime
    {
        public List<Border> CreateBorders(List<Item> dataArray)
        {
            List<Border> borderRet = new List<Border>();

            var loop = 0;
            foreach (var item in dataArray)
            {
                Border bd = new Border();
                Label lb = new Label();
                lb.Content = string.Format("{0} ${1}", item.Name, item.Price);
                lb.ToolTip = item.Description;
                Thickness tk = new Thickness(1.0);
                //bd.Width = 100;
                bd.Height = 100;
                bd.BorderBrush = Brushes.Black;
                bd.BorderThickness = tk;
                bd.Name = "border1";
                bd.Uid = item.id.ToString();
                bd.Child = lb;
                Thickness margin = new Thickness(loop * 100, 0, 0, 0);
                bd.Margin = margin;
                borderRet.Add(bd);
            }

            return borderRet;
        }

        public List<Canvas> Initialize_Canvas(List<Item> dataArray)
        {
            List<Canvas> borderRet = new List<Canvas>();

            var loop = 0;
            foreach (var item in dataArray)
            {
                Canvas cnv = new Canvas();

                Border bd = new Border();
                Label lb = new Label();
                lb.Content = string.Format("{0} ${1}", item.Name, item.Price);
                lb.ToolTip = item.Description;
                Thickness tk = new Thickness(1.0);
                bd.Height = 100;
                bd.Background = Brushes.Black;
                bd.BorderBrush = Brushes.Black;
                bd.BorderThickness = tk;
                bd.Name = "border1";
                bd.Uid = item.id.ToString();
                //bd.Child = lb;
                //Thickness margin = new Thickness(loop * 100, 0, 0, 0);
                //bd.Margin = margin;
                cnv.Background = Brushes.Black;
                cnv.HorizontalAlignment = HorizontalAlignment.Stretch;
                cnv.Children.Add(bd);
                cnv.Children.Add(lb);

                borderRet.Add(cnv);
            }

            return borderRet;
        }

        public List<Border> CreatePanels(List<Item> dataArray)
        {
            List<Border> borderRet = new List<Border>();

            var loop = 0;
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
                Color color = (Color)ColorConverter.ConvertFromString("#FFC1C8E4");
                SolidColorBrush myBrush = new SolidColorBrush(color);
                //Creating the stackpanel for content
                StackPanel stack = new StackPanel();
                stack.Name = "MainViewer";
                stack.Background = myBrush;
                #endregion

                #region border
                color = (Color)ColorConverter.ConvertFromString("#8860D0");
                myBrush = new SolidColorBrush(color);
                Thickness tk = new Thickness(3.0);
                Thickness margin = new Thickness(0, 0, 0, 0);
                Border bd = new Border();
                bd.BorderBrush = myBrush;
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
                color = (Color)ColorConverter.ConvertFromString("#8860D0");
                myBrush = new SolidColorBrush(color);


                Border bd2 = new Border();
                bd2.BorderBrush = myBrush;
                bd2.BorderThickness = new Thickness(0, 0, 3, 0);
                bd2.Height = 100;
                bd2.Width = 100;
                bd2.CornerRadius = new CornerRadius(5);
                bd2.Padding = new Thickness(3);

                Image img = new Image();
                //Uri myUri = new Uri(@"C:\FTWh9O.png", UriKind.RelativeOrAbsolute);
                //BmpBitmapDecoder decoder = new BmpBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                //BitmapSource bitmapSource = decoder.Frames[0];
                //img.Source = bitmapSource;
                bd2.Child = img;

                panel2.Children.Add(bd2);

                #endregion

                #region Canvas
                Canvas cnv = new Canvas();
                cnv.Name = "CanvasDatos";
                cnv.Height = 100;

                //Adding the textblock to canvas

                cnv.Children.Add(new TextBlock
                {
                    Padding = new Thickness(5),
                    Text = string.Format("{0} ${1}", item.Name, item.Price.ToString())
                });

                cnv.Children.Add(new TextBlock
                {
                    Name = "TextoDescripcion",
                    Height = 70,
                    Width = 348,
                    Padding = new Thickness(5),
                    Margin = new Thickness(0, 30, 0, 0),
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    Text = item.Description
                });

                //Adding Canvas to panel 2
                panel2.Children.Add(cnv);
                #endregion

                #region 3er border
                //Adding 3er border to panel2
                panel2.Children.Add(new Border
                {
                    Height = 100,
                    Width = 100,
                    BorderBrush = myBrush,
                    BorderThickness = new Thickness(3, 0, 0, 0),
                    CornerRadius = new CornerRadius(5),
                    Padding = new Thickness(3),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Child = new Button
                    {
                        Content = "Agregar",
                        Width = 70,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Height = 30,
                        Name = "btnAgregar"
                    }
                    //.Click += new RoutedEventHandler(Click_Event)
                    //.AddHandler(Button.ClickEvent, new RoutedEventHandler(Click_Event))
                });
                #endregion

                #region adding the remaining elements to the dockpanel

                bd.Child = panel2;
                //stack.Children.Add(bd);
                //scroll.Content = stack;
                //panel.Children.Add(scroll);


                #endregion

                borderRet.Add(bd);

            }

            return borderRet;
        }

        public Border panel(Item item)
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
            Color color = (Color)ColorConverter.ConvertFromString("#FFC1C8E4");
            SolidColorBrush myBrush = new SolidColorBrush(color);
            //Creating the stackpanel for content
            StackPanel stack = new StackPanel();
            stack.Name = "MainViewer";
            stack.Background = myBrush;
            #endregion

            #region border
            color = (Color)ColorConverter.ConvertFromString("#8860D0");
            myBrush = new SolidColorBrush(color);
            Thickness tk = new Thickness(3.0);
            Thickness margin = new Thickness(0, 0, 0, 0);
            Border bd = new Border();
            bd.BorderBrush = myBrush;
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
            color = (Color)ColorConverter.ConvertFromString("#8860D0");
            myBrush = new SolidColorBrush(color);


            Border bd2 = new Border();
            bd2.BorderBrush = myBrush;
            bd2.BorderThickness = new Thickness(0, 0, 3, 0);
            bd2.Height = 100;
            bd2.Width = 100;
            bd2.CornerRadius = new CornerRadius(5);
            bd2.Padding = new Thickness(3);

            Image img = new Image();
            //Uri myUri = new Uri(@"C:\FTWh9O.png", UriKind.RelativeOrAbsolute);
            //BmpBitmapDecoder decoder = new BmpBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            //BitmapSource bitmapSource = decoder.Frames[0];
            //img.Source = bitmapSource;
            bd2.Child = img;

            panel2.Children.Add(bd2);

            #endregion

            #region Canvas
            Canvas cnv = new Canvas();
            cnv.Name = "CanvasDatos";
            cnv.Height = 100;

            //Adding the textblock to canvas

            cnv.Children.Add(new TextBlock
            {
                Padding = new Thickness(5),
                Text = string.Format("{0} ${1}", item.Name, item.Price.ToString())
            });

            cnv.Children.Add(new TextBlock
            {
                Name = "TextoDescripcion",
                Height = 70,
                Width = 348,
                Padding = new Thickness(5),
                Margin = new Thickness(0, 30, 0, 0),
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = item.Description
            });

            //Adding Canvas to panel 2
            panel2.Children.Add(cnv);
            #endregion

            #region 3er border
            //Adding 3er border to panel2
            panel2.Children.Add(new Border
            {
                Height = 100,
                Width = 100,
                BorderBrush = myBrush,
                BorderThickness = new Thickness(3, 0, 0, 0),
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(3),
                HorizontalAlignment = HorizontalAlignment.Right,
                Child = new Button
                {
                    Content = "Agregar",
                    Width = 70,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = 30,
                    Name = "btnAgregar"
                }
                //.Click += new RoutedEventHandler(Click_Event)
                //.AddHandler(Button.ClickEvent, new RoutedEventHandler(Click_Event))
            });
            #endregion

            #region adding the remaining elements to the dockpanel

            bd.Child = panel2;
            stack.Children.Add(bd);
            scroll.Content = stack;
            panel.Children.Add(scroll);


            #endregion

            return bd;
        }

        public void ClearTextBoxes(List<TextBox> list)
        {
            foreach (var item in list)
            {
                item.Clear();

            }
        }

        public void Enable_disableTextBoxes(List<TextBox> list, bool action)
        {
            foreach (var item in list)
            {
                item.IsEnabled = action;
            }

        }


        public void PopulateDataGrid(DataGrid dg, DatabaseModel db)
        {
            ObservableCollection<Item> data = new ObservableCollection<Item>();

            foreach (var item in db.Items)
            {
                data.Add(item);
            }

            dg.ItemsSource = data;
        }

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

        }

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
                case "delete":
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


    } //End limit of code
}
