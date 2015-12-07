using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Security.Tokens;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TI2._2_HueApp.Connector;
using TI2._2_HueApp.lib;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TI2._2_HueApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuPage : Page
    {
        public MenuPage()
        {
            this.InitializeComponent();
            this.BridgePortTextBox.Text = Global.Connector.Port.ToString();
            this.BridgeIpTextBox.Text = Global.Connector.Ip;
        }
        
        private async void Connect()
        {
            string returnJson = await Global.InitializeLightsAsync();
            bool success = !(returnJson == "" || returnJson == "[]" || returnJson == "{}");
            if (success)
            {
                string returnstr = JsonUtil.CheckIfErrorInJson(returnJson);
                if (returnstr == "false")
                {
                    if ((bool) RememberCheckBox.IsChecked)
                    {
                        Global.Connector.SaveSettings(BridgeIpTextBox.Text, null, BridgePortTextBox.Text);
                    }
                    this.Frame.Navigate(typeof (MainPage));
                }
                else
                {
                    Debug.WriteLine(returnstr);
                    if (returnstr == "unauthorized user")
                    {
                        //await new MessageDialog("You have not registered on this bridge").ShowAsync();
                        await Register();

                    }
                    else if (returnstr == "method, GET, not available for resource, /")
                    {
                        await Register();
                    }
                    else
                    {
                        await new MessageDialog(returnstr).ShowAsync();
                    }

                    return;
                }
            }
            else
            {
                await new MessageDialog("There whas a problem with retrieving the lights from the bridge").ShowAsync();
                return;
            }
        }

        private void textBoxNumeric_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Int32 selectionStart = textBox.SelectionStart;
            Int32 selectionLength = textBox.SelectionLength;
            String newText = String.Empty;
            int count = 0;
            foreach (Char c in textBox.Text.ToCharArray())
            {
                if (Char.IsDigit(c) || Char.IsControl(c) || (c == '.' && count == 0))
                {
                    newText += c;
                    if (c == '.')
                        count += 1;
                }
            }
            textBox.Text = newText;
            textBox.SelectionStart = selectionStart <= textBox.Text.Length ? selectionStart : textBox.Text.Length;
        }

        private async Task Register()
        {
            string deviceName = "";

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
            {
                deviceName = Windows.Networking.Proximity.PeerFinder.DisplayName;
            }
            else
            {
                deviceName = "UnownDevice";
            }

            try
            {
                await Global.Connector.Register(deviceName);
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            if (Global.Connector.Username != "")
            {
                if ((bool) RememberCheckBox.IsChecked)
                {
                    Global.Connector.SaveSettings(BridgeIpTextBox.Text, deviceName, BridgePortTextBox.Text);   
                }
                await Global.InitializeLightsAsync();
                this.Frame.Navigate(typeof(MainPage));
            }
            else
            {
                await new MessageDialog("Please press on the link button").ShowAsync();
            }
                
        }

        private void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            Global.Connector.Ip = this.BridgeIpTextBox.Text;
            Global.Connector.Port = int.Parse(this.BridgePortTextBox.Text);
            Connect();
        }
    }
}
