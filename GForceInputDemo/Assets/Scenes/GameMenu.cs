using System;
using System.Collections.Generic;
using GameCtrler;
using gf;
using GForce;
using UnityEngine;
using UnityEngine.UI;
using static gf.Device;

public class GameMenu : MonoBehaviour
{

    public GameMenu()
    {

    }

    private long GetTimeInSeconds()
    {
        return DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
    }

    private struct ScannedDevice
    {
        public long lastActiveTime;
        public Device device;
    }

    //private UIListenerImpl uiListenerImpl;
    public GameMain gameMain;
    private Dictionary<string/*address*/, ScannedDevice> devices = new Dictionary<string/*address*/, ScannedDevice>();      // Newly scanned devices, to be added to prepDevices
    private Dictionary<string, DeviceShow> deviceUIs = new Dictionary<string, DeviceShow>();
    
    [Tooltip("Device container")] public GameObject deviceCtl;
    public GameObject deviceUI;
    public Button scanButton;
    public Button exitButton;

    // Start is called before the first frame update
    void OnEnable()
    {

        Hub.Instance.startScan();

        scanButton.onClick.AddListener(StartScan);
        exitButton.onClick.AddListener(ExitMenu);
        
        StartScan();
    }

    void OnDisable()
    {
        Hub.Instance.stopScan();
    }

    /// <summary>
    /// Begin Scan
    /// </summary>
    public void StartScan()
    {
        RetCode ret;

        if (Hub.Instance.getStatus() != Hub.HubState.Scanning)
        {
            devices.Clear();
            deviceUIs.Clear();
            ClearDevices();

            Debug.Log("GForceGameController.GetConnectedDevices().length: " + GForceGameController.GetConnectedDevices().Count);

            foreach (var dev in GForceGameController.GetConnectedDevices())
            {
                if (dev == null) continue;

                Debug.Log("Device: " + dev.getName());

                if (!devices.ContainsKey(dev.getAddress()))
                {
                    ScannedDevice scanned = new ScannedDevice();

                    scanned.device = dev;
                    scanned.lastActiveTime = GetTimeInSeconds();

                    devices.Add(dev.getAddress(), scanned);
                }

                CreateDevice(dev);
            }

            ret = Hub.Instance.startScan();
            Debug.Log("Hub.Instance.startScan() returned: " + ret);
        }
    }

    /// <summary>
    /// Found Device 
    /// </summary>
    /// <param name="device"></param>
    public void FoundDevice(Device device)
    {
        lock (devices)
        {
            if (devices.ContainsKey(device.getAddress()))
            {
                devices.Remove(device.getAddress());
            }

            ScannedDevice scanned = new ScannedDevice();
            scanned.device = device;
            scanned.lastActiveTime = GetTimeInSeconds();

            devices.Add(device.getAddress(), scanned);
        }
    }

    /// <summary>
    /// Create Device UI
    /// </summary>
    /// <param name="device"></param>
    public void CreateDevice(Device device)
    {
        Debug.Log("CreateDevice");
        GameObject gameObj = Instantiate(deviceUI);
        gameObj.transform.SetParent(deviceCtl.transform);
        gameObj.GetComponent<DeviceShow>().Init(device);

        deviceUIs.Add(device.getAddress(), gameObj.GetComponent<DeviceShow>());
    }

    /// <summary>
    /// Clear Device UI
    /// </summary>
    public void ClearDevices()
    {
        foreach (var item in deviceCtl.GetComponentsInChildren<DeviceShow>())
        {
            Destroy(item.gameObject);
        }
    }

    public void RestOrientationData(Device device)
    {
        gameMain.RestOrientationData(device);
    }


    private void Update()
    {
        lock (devices)
        {
            foreach (var dev in devices)
            {
                if (!deviceUIs.ContainsKey(dev.Key))
                {
                    CreateDevice(dev.Value.device);
                }
                else
                {
                    deviceUIs[dev.Key].UpdateInfo(dev.Value.device);
                }
            }
        }

        scanButton.interactable = (Hub.Instance.getStatus() == Hub.HubState.Idle);
    }

    private void ExitMenu()
    {
        gameObject.SetActive(false);
    }
}


