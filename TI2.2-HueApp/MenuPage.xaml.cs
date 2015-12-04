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
        private bool _registerbuttonClickable = true, _loginbuttonClickable = true;

        public MenuPage()
        {
            this.InitializeComponent();
        }

        private bool CheckConnectorActive()
        {
            bool success = false;
            if (Global.Connector == null)
            {
                var setting = Global.Settings.FirstOrDefault(s => s.Name == BridgeNameTextBox.Text);
                if (setting != null)
                {
                    Global.InitializeConnector(new HueAPIConnector(setting));
                    success = true;
                }
                return success;
            }
            return true;
        }

        private async void Connect()
        {
            //bool success = CheckConnectorActive();
            bool success = true;
            Global.InitializeConnector(new HueAPIConnector());
            Global.Connector.Username = "newdeveloper";

            if (!success)
            {
                await new MessageDialog("Connection to the bridge failed").ShowAsync();
                return;
            }
                

            string returnJson = await Global.InitializeLightsAsync();
            success = !(returnJson == "" || returnJson == "[]" || returnJson == "{}");
            if (success)
            {
                string returnstr = JsonUtil.CheckIfErrorInJson(returnJson);
                if (returnstr == "false")
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    Debug.WriteLine(returnstr);
                    if (returnstr == "unauthorized user")
                    {
                        await new MessageDialog("You have not registered on this bridge").ShowAsync();
                    }
                    else
                    {
                        await new MessageDialog(returnstr).ShowAsync();
                    }
                    
                    return;
                }
            }
        }

        private async void Register()
        {
            string username = "";
            try
            {
                //username = await Global.Connector.Register();
            }
            catch (NullReferenceException e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            if (username != "")
            {
                RegisterButton.Content = "Registered!";
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
            Connect();
        }
    }
}
