using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ItemInventoryApp.Classes
{
    public class UIHelper
    {

        public UIElement FindChildByName(Window mainwindow, string typeChild, string childName)
        {
            typeChild = typeChild.ToLower();
            var el = Application.Current.MainWindow;

            switch (typeChild)
            {
                case "dockpanel":
                    var Panelchild = (DockPanel)el.FindName(childName);
                    return Panelchild;
                case "stackpanel":
                    var Stackchild = (StackPanel)el.FindName(childName);
                    return Stackchild;
                case "textblock":
                    var textblock = (TextBlock)el.FindName(childName);
                    return textblock;
                case "button":
                    var button = (Button)el.FindName(childName);
                    return button;
                default:
                    return new UIElement();
            }
        }    
    } //End of the way
}
