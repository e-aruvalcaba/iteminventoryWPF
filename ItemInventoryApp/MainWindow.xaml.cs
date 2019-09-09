using System;
using System.Collections.Generic;
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
        }

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

        private void BtnCreateNewProduct_Click(object sender, RoutedEventArgs e)
        {
            string richtext = new TextRange(txtDescC.Document.ContentStart, txtDescC.Document.ContentEnd).Text;
            if (!string.IsNullOrEmpty(txtBoxNombreC.Text)&&!string.IsNullOrEmpty(richtext) && !string.IsNullOrEmpty(txtPrecioC.Text))
            {
                string noAccepted = "qwertyuiop´+asdfghjklñ{zxcvbnm,-!#$%%&/()=?[]*¨¨_:;¬{}¡*";

                if (!txtPrecioC.Text.Contains(noAccepted))
                {
                    if (!txtPrecioC.Equals("."))
                    {

                        GlobalMainObject = GlobalHandler.UpdateDBObject();
                        GlobalHandler.CreateItem(new Item {
                            id = GlobalMainObject.Items[GlobalMainObject.Items.Count-1].id+1,
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
    } //End of the way
}
                             