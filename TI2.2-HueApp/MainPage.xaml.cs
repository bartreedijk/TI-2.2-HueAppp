using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TI2._2_HueApp.Connector;
using TI2._2_HueApp.Enitity;
using TI2._2_HueApp.lib;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace TI2._2_HueApp
{
   
    public sealed partial class MainPage : Page
    {
        public List<Light> Lights
        {
            get { return Global.Instance.Lights; }
        }

        private bool IsLoaded;

        public HueAPIConnector Connector
        {
            get { return Global.Instance.Connector; }
        }
        public MainPage()
        {
            double h, s, v;
            ColorUtil.RGBtoHSV(255, 0, 0, out h, out s, out v);
            Init();
        }

        private async void Init()
        {
            await Global.Instance.InitializeConnection();
            InitializeComponent();
        }
        


        private void ListBox_Selection(object sender, SelectionChangedEventArgs e)
        {
            if (lampListView.SelectedIndex == -1)
                return;

            ListBoxItem currentItem = lampListView.ContainerFromIndex(lampListView.SelectedIndex) as ListBoxItem;

            if (currentItem == null)
                return;

            // Iterate whole listbox tree and search for this items
            Grid grid = FindDescendantByName<Grid>(currentItem, "ItemGrid");

            if(grid.Height > 90)
            {
                lampListView.SelectedIndex = -1;
                grid.Height = 80;
            }
            else
            {
                grid.Height = 250;
                lampListView.SelectedIndex = -1;
            }
           
        }

        public T FindDescendantByName<T>(DependencyObject obj, string objname) where T : DependencyObject
        {
            string controlneve = "";

            Type tyype = obj.GetType();
            if (tyype.GetProperty("Name") != null)
            {
                PropertyInfo prop = tyype.GetProperty("Name");
                controlneve = prop.GetValue(obj, null).ToString();
            }
            else
            {
                return null;
            }

            if (obj is T && objname.ToString().ToLower() == controlneve.ToString().ToLower())
            {
                return obj as T;
            }

            // Check for children
            int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
            if (childrenCount < 1)
                return null;

            // First check all the children
            for (int i = 0; i <= childrenCount - 1; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && objname.ToString().ToLower() == controlneve.ToString().ToLower())
                {
                    return child as T;
                }
            }

            // Then check the childrens children
            for (int i = 0; i <= childrenCount - 1; i++)
            {
                string checkobjname = objname;
                DependencyObject child = FindDescendantByName<T>(VisualTreeHelper.GetChild(obj, i), objname);
                if (child != null && child is T && objname.ToString().ToLower() == checkobjname.ToString().ToLower())
                {
                    return child as T;
                }
            }

            return null;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void HueSlider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            Slider slider = sender as Slider;
            Light light = slider.DataContext as Light;
            await Connector.HttpPut("lights\\" + light.ID + "\\state", light.getHueToJson());
        }

        private async void SatSlider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            Slider slider = sender as Slider;
            Light light = slider.DataContext as Light;
            await Connector.HttpPut("lights\\" + light.ID + "\\state", light.getSatToJson());
        }

        private async void BriSlider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            Slider slider = sender as Slider;
            Light light = slider.DataContext as Light;
            await Connector.HttpPut("lights\\" + light.ID + "\\state", light.getBriToJson());
        }

        private async void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            
            ToggleSwitch OnOff = sender as ToggleSwitch;
            Light light = OnOff.DataContext as Light;
            Light LightOut = Lights.Where(lightInternal => lightInternal.ID == light.ID).FirstOrDefault();
            if (!IsLoaded)
                return;
            light.State = !light.State;
            await Connector.HttpPut("lights\\" + light.ID + "\\state", light.getStateToJson());
        }

        private void ToggleSwitch_Toggled(object sender, TappedRoutedEventArgs e)
        {

        }

        private void Page_LayoutUpdated(object sender, object e)
        {
            IsLoaded = true;
        }
    }
}
