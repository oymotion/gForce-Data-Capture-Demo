using gf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static gf.Device;

public class DeviceShow : MonoBehaviour
{
    public Text deviceInfo;
    public Text rssiInfo;
    public Text connectStatus;
    public Button connectButton;
    private Device device;

    public void Init(Device device)
    {
        Debug.Log("[DeviceShow] init ");

        UpdateInfo(device);

        connectButton.onClick.AddListener(Connect);
    }

    public void UpdateInfo(Device device)
    {
        int rssi = (int)device.getRssi();
        string connectStatus;

        switch (device.getConnectionStatus())
        {
            case ConnectionStatus.Disconnected: connectStatus = "Connect"; break;
            case ConnectionStatus.Disconnecting: connectStatus = "Disconnecting"; break;
            case ConnectionStatus.Connecting: connectStatus = "Connecting"; break;
            case ConnectionStatus.Connected: connectStatus = "Disconnect"; break;
            default: connectStatus = "Unknown"; break;
        }

        if (rssi > 0)
        {
            this.deviceInfo.text = device.getName();
            this.rssiInfo.text = "rssi :" + rssi.ToString();
            this.connectStatus.text = connectStatus;
        }
        else
        {
            this.connectStatus.text = "BAD_STATE";
        }

        this.device = device;
    }



    void Connect()
    {
        RetCode ret;

        Debug.Log("Click Connect Button, conn status=" + device.getConnectionStatus());

        if (device.getConnectionStatus() == Device.ConnectionStatus.Disconnected)
        {
            if (Hub.Instance.getStatus() == Hub.HubState.Scanning)
            {
                ret = Hub.Instance.stopScan();
                Debug.Log("Hub.Instance.stopScan() returned: " + ret);
            }

            ret = device.connect();
            Debug.Log("device.connect() returned: " + ret);
        }
        else if (device.getConnectionStatus() == Device.ConnectionStatus.Connected)
        {
            ret = device.disconnect();
            Debug.Log("device.disconnect() returned: " + ret);

            if (Hub.Instance.getStatus() != Hub.HubState.Scanning)
            {
                ret = Hub.Instance.startScan();
                Debug.Log("Hub.Instance.startScan() returned: " + ret);
            }
        }
    }
}
