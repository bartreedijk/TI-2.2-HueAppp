using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Security.Tokens;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TI2._2_HueApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MenuPage : Page
    {
        private bool _registerbuttonClickable = true, _loginbuttonClickable = true;

        public MenuPage()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
            {
                Global.Settings.Add(new Enitity.Setting("Lampen Lokaal", Windows.Networking.Proximity.PeerFinder.DisplayName, "145.048.205.190", 80));
                Global.Settings.Add(new Enitity.Setting("Emulator", Windows.Networking.Proximity.PeerFinder.DisplayName, "127.0.0.1", 80));
                JsonUtil.SaveSettings();
            }
            this.InitializeComponent();
            Global.Settings.Clear();
            JsonUtil.GetSettings();
        

            BridgeComboBox.ItemsSource = Global.Settings;
            BridgeComboBox.DisplayMemberPath = "Name";

        }

        private async void Connect()
        {
            
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

        private async void Register()
        {
            string username = "";
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
                username = await Global.Connector.Register(deviceName);
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            if (username != "")
            {
                RegisterButton.Content = "Registered!";
                if ((bool)RememberCheckBox.IsChecked)
                {
                    Global.Settings.Add(new Enitity.Setting(NameTextBox.Text, deviceName, BridgeIpTextBox.Text, Convert.ToInt32(ServerPortTextBox.Text)));
                }
            }
            else
            {
                RegisterButton.Content = "Registration failed!";
                _registerbuttonClickable = false;
            }
                
        }

        private async void Login()
        {
            await Global.InitializeLightsAsync();
            this.Frame.Navigate(typeof (MainPage));
        }

        private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_registerbuttonClickable) return;
            //Global.Instance.Connector.IP = ServerIpTextBox.Text;
            Register();
            _registerbuttonClickable = false;
        }

        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_loginbuttonClickable) return;
            if (_registerbuttonClickable) return;
            //Global.Instance.Connector.IP = ServerIpTextBox.Text;
            Login();
            _loginbuttonClickable = false;
        }

        private void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
