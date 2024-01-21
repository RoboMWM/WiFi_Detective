﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;

namespace WiFi_Detective
{
    class WifiScanner
    {
        public WiFiAdapter WiFiAdapter { get; private set; }

        private async Task InitializeFirstAdapter()
        {
            WiFiAccessStatus access = await WiFiAdapter.RequestAccessAsync();
            if (access != WiFiAccessStatus.Allowed)
            {
                throw new Exception("WifiAccessStatus not allowed");
            }

            DeviceInformationCollection wifiAdapterResults = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
            if (wifiAdapterResults.Count == 0)
            {
                throw new Exception("WiFi Adapter not found.");
            }

            this.WiFiAdapter = await WiFiAdapter.FromIdAsync(wifiAdapterResults[0].Id);
        }

        public async Task<List<string>> ScanForNetworks()
        {
            if (this.WiFiAdapter == null)
                return null;

            await this.WiFiAdapter.ScanAsync();

            List<string> ssids = new List<string>();

            foreach (WiFiAvailableNetwork availableNetwork in WiFiAdapter.NetworkReport.AvailableNetworks)
            {
                ssids.Add(availableNetwork.Ssid);
            }

            return ssids;
        }
    }
}
