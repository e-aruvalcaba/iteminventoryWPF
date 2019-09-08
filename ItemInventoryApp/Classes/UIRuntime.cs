using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ItemInventoryApp.Models;
namespace ItemInventoryApp.Classes
{
    class UIRuntime
    {
        public List<Border> CreateBorders(List<Item> dataArray)
        {
            List<Border> borderRet=new List<Border>();

            var loop = 0;
            foreach (var item in dataArray)
            {
                Border bd = new Border();
                Label lb = new Label();
                lb.Content = string.Format("{0} ${1}",item.Name, item.Price);
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


    } //End limit of code
}
