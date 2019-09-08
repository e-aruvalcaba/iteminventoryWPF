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
using ItemInventoryApp.Models;

namespace ItemInventoryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UIRuntime obj = new UIRuntime();

            List<Item> itemList = new List<Item>();

            itemList.Add(new Item {
                id = 1,
                Name = "Tacos al vapor",
                Description = "Paquete de 5 tacos bañados en salsa roja de habanero",
                Price = 25
            });
            itemList.Add(new Item
            {
                id = 2,
                Name = "Tacos de pastor",
                Description = "Paquete de 5 tacos de pastor con cilantro cebolla caramelizada, salsa medio limon y doble tortilla.",
                Price = 35
            });
            itemList.Add(new Item
            {
                id = 3,
                Name = "Tacos de bisteck",
                Description = "Paquete de 5 tacos de bisteck de res con cilantro cebolla caramelizada, salsa medio limon y doble tortilla.",
                Price = 40
            });
            itemList.Add(new Item
            {
                id = 4,
                Name = "Papa asada Mixta",
                Description = "Deliciosa papa machacada con matenquilla en un contenedor de aluminio sasonada con especias finas y mezclada con queso amarillo derretido y crema y cubierta con carne de bisteck y de pastor, un chile en vinagre y 4 paquetes de galletas saladitas.",
                Price = 60
            });
            itemList.Add(new Item
            {
                id = 5,
                Name = "Papa asada de pastor",
                Description = "Deliciosa papa machacada con matenquilla en un contenedor de aluminio sasonada con especias finas y mezclada con queso amarillo derretido y crema y cubierta con carne de pastor, un chile en vinagre y 4 paquetes de galletas saladitas.",
                Price = 45
            });
            itemList.Add(new Item
            {
                id = 6,
                Name = "Papa asada de bisteck",
                Description = "Deliciosa papa machacada con matenquilla en un contenedor de aluminio sasonada con especias finas y mezclada con queso amarillo derretido y crema y cubierta con carne de bisteck, un chile en vinagre y 4 paquetes de galletas saladitas.",
                Price = 55
            });
            itemList.Add(new Item
            {
                id = 7,
                Name = "Bomba",
                Description = "5 Quesadillas en tostada con carne de pastor, cilantro cebolla, salsa y tocino.",
                Price = 35
            });

            //var lista = obj.CreateBorders(itemList);
            var lista = obj.CreatePanels(itemList);

            for (int i = 0; i < lista.Count; i++)
            {
                MainViewer.Children.Add(lista[i]);
            }
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
    }
}
                             