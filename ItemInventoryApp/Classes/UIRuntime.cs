using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ItemInventoryApp.Operations
{
    class UIRuntime
    {
        public List<Border> CreateBorders(List<string> dataArray)
        {
            List<Border> borderRet=null;

            var loop = 0;
            foreach (var item in dataArray)
            {
                Label lb = new Label();
                lb.Content = "Contendio del label";

                Thickness tk = new Thickness(1.0);
                Border bd = new Border();
                bd.Width = 100;
                bd.Height = 100;
                bd.BorderBrush = Brushes.Black;
                bd.BorderThickness = tk;
                bd.Name = "border1";
                bd.Child. = lb;
                Thickness margin = new Thickness(loop * 100, 0, 0, 0);
                bd.Margin = margin;
                borderRet.Add(bd)
            }

            

            return borderRet;
        }
    }
}
