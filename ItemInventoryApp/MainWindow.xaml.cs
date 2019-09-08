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
                id = 1,
                Name = "Tacos al vapor",
                Description = "Paquete de 5 tacos bañados en salsa roja de habanero",
                Price = 25
            });
            itemList.Add(new Item
            {
                id = 1,
                Name = "Tacos al vapor",
                Description = "Paquete de 5 tacos bañados en salsa roja de habanero",
                Price = 25
            });
            itemList.Add(new Item
            {
                id = 1,
                Name = "Tacos al vapor",
                Description = "Paquete de 5 tacos bañados en salsa roja de habanero",
                Price = 25
            });
            itemList.Add(new Item
            {
                id = 1,
                Name = "Tacos al vapor",
                Description = "Paquete de 5 tacos bañados en salsa roja de habanero",
                Price = 25
            });
            itemList.Add(new Item
            {
                id = 1,
                Name = "Tacos al vapor",
                Description = "Paquete de 5 tacos bañados en salsa roja de habanero",
                Price = 25
            });
            itemList.Add(new Item
            {
                id = 1,
                Name = "Tacos al vapor",
                Description = "Paquete de 5 tacos bañados en salsa roja de habanero",
                Price = 25
            });

            var lista = obj.CreateBorders(itemList);

            //for (int i = 0; i < lista.Count; i++)
            //{
            //    //MainCanvas.Children.Add(lista[i]);
            //    MainViewer.Children.Add(lista[i]);
            //}
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
                TextoDescripcion.Width = DockMain.ActualWidth - 200;

            }
            else
            {
                TextoDescripcion.Width = 348;
            }
        }
    }
}
                             