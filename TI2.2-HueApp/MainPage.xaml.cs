using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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


namespace TI2._2_HueApp
{
   
    public sealed partial class MainPage : Page
    {
        private List<Light> Lights;
        public MainPage()
        {
            Lights = new List<Light>();
            this.InitializeConnection();
        }

        private async void InitializeConnection()
        {
            Connector.HueAPIConnector connector = new Connector.HueAPIConnector();
            await connector.Register();
            string json = await connector.RetrieveLights();
            Lights = JsonUtil.convertJsonToLights(json);

            this.InitializeComponent();
        }
        
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
