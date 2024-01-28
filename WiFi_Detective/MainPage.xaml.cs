using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WiFi_Detective
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void ButtonScan_Click(object sender, RoutedEventArgs e)
        {
            buttonScan.IsEnabled = false;
            progressScan.IsActive = true;
            listViewAPs.Items.Clear();

            WifiScanner scanner = new WifiScanner(); //TODO: see below
            List<string> ssids;
            try
            {
                ssids = await scanner.ScanForNetworks();
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
                buttonScan.IsEnabled = true;
                progressScan.IsActive = false;
                return;
            }

            populateSsids(ssids);
            buttonScan.IsEnabled = true;
            progressScan.IsActive = false;
        }

        private void populateSsids(List<string> ssids)
        {
            if (ssids == null)
            {
                ListViewItem item = new ListViewItem();
                item.Content = "No wireless access points found";
                listViewAPs.Items.Add(item);
                return;
            }

            foreach (string ssid in ssids)
                listViewAPs.Items.Add(new ListViewItem().Content = ssid);
        }

        private async void AppWifiInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await new MessageDialog(await new WifiScanner().GetAdapterInfo()).ShowAsync(); //TODO: instantiate object at page initialization(?) or better yet, app initialization.
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private async void AppAbout_Click(object sender, RoutedEventArgs e)
        {
            await new MessageDialog(await new WifiScanner().GetAdapterInfo()).ShowAsync(); //TODO: instantiate object at page initialization(?) or better yet, app initialization.
        }
    }
}
