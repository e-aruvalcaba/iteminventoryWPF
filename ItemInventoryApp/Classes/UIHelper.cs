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
                case "grid":
                    var grid = (Grid)el.FindName(childName);
                    return grid;
                case "tabcontrol":
                    var tabcontrol = (TabControl)el.FindName(childName);
                    return tabcontrol;
                case "border":
                    var border = (Border)el.FindName(childName);
                    return border;
                case "combobox":
                    var combobox = (ComboBox)el.FindName(childName);
                    return combobox;
                case "datagrid":
                    var datagrid = (DataGrid)el.FindName(childName);
                    return datagrid;

                default:
                    return new UIElement();
            }
        }    
    } //End of the way
}
