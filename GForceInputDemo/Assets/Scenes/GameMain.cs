using System;
using System.Collections;
using System.Collections.Generic;
using GameCtrler;
using gf;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Android;
using GForce;


public class GameMain : MonoBehaviour
{
    private GForceListener gForceListener;
    public Button deviceButton;
    public GameObject menuPanel;
    public Text curDeviceText;
    public Text curDeviceText_t;

    public Transform showTransform;
    public Transform showTransform_t;
    public Text TipText;

    public DataShow dataShow;
    public DataShow dataShow_t;

    Device restDevice = null;

    IGameController gForceGameController_1, gForceGameController_2;

    GForceGameController ctrler_1, ctrler_2;

    public GForceListener GetGForceListener(int value = 0)
    {
        return gForceListener;
    }

    #region Unity
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
        menuPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

        unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            GForceHub.instance.Prepare();
        }));

        string[] strs = new string[] {
            "android.permission.BLUETOOTH",
            "android.permission.BLUETOOTH_ADMIN",
            //"android.permission.ACCESS_COARSE_LOCATION",
            "android.permission.ACCESS_FINE_LOCATION"
        };

        strs.ToList().ForEach(s =>
        {
            Permission.RequestUserPermission(s);
            Debug.Log("add RequestUserPermission: " + s);
        });
#else
        GForceHub.instance.Prepare();
#endif

        gForceListener = new GForceListener();
        Hub.Instance.registerListener(gForceListener);

        gForceGameController_1 = GameControllerManager.CreateGameController("GForceGameController");
        gForceGameController_2 = GameControllerManager.CreateGameController("GForceGameController");

        gForceListener.RegisterGForceDevice(gForceGameController_1 as GForceDevice);
        gForceListener.RegisterGForceDevice(gForceGameController_2 as GForceDevice);
        ctrler_1 = (GForceGameController)gForceGameController_1;
        ctrler_2 = (GForceGameController)gForceGameController_2;

        deviceButton.onClick.AddListener(
            () => menuPanel.SetActive(true)
            );
    }

    // Update is called once per frame
    void Update()
    {
        #region DeviceOne
        ctrler_1 = (GForceGameController)gForceGameController_1;
        if (ctrler_1!=null)
        {
            ctrler_1.Tick();
            if (ctrler_1.GetDevice() == restDevice
                && restDevice != null)
            {
                ctrler_1.RestOrientationData();
                restDevice = null;
            }

            curDeviceText.text = "Device : " + ((ctrler_1 == null || (ctrler_1.GetStatus() != ControllerState.STATE_CONNECTED && ctrler_1.GetStatus() != ControllerState.STATE_READY) || ctrler_1.GetName().Length == 0) ? "<not connected>" : ctrler_1.GetName());

            if (ctrler_1 != null && ctrler_1.GetStatus() == ControllerState.STATE_READY)
            {
                Quaternion deivceQuaternion = new Quaternion(
                    -ctrler_1.GetQuaternionX(),
                    -ctrler_1.GetQuaternionZ(),
                    -ctrler_1.GetQuaternionY(),
                    ctrler_1.GetQuaternionW());
                showTransform.rotation = deivceQuaternion;
                Debug.Log("DeviceOne:DeviceName:" + ctrler_1.GetName() + "DeivceQuaternion:" + deivceQuaternion);
                dataShow.UpdateEMGData(ctrler_1.GetEmgValue(),ctrler_1.ChannelMapCount);
                dataShow.UpdateQuaternionDataText(deivceQuaternion);
                dataShow.UpdataGestureDataText(ctrler_1.GetGesture() + "");
            }

        }
        #endregion

        #region DeviceTwo
        ctrler_2 = (GForceGameController)gForceGameController_2;
        if(ctrler_2!=null)
        {
            ctrler_2.Tick();
            if (ctrler_2.GetDevice() == restDevice
                && restDevice != null)
            {
                ctrler_2.RestOrientationData();
                restDevice = null;
            }

            curDeviceText_t.text = "Device : " + ((ctrler_2 == null || (ctrler_2.GetStatus() != ControllerState.STATE_CONNECTED && ctrler_2.GetStatus() != ControllerState.STATE_READY) || ctrler_2.GetName().Length == 0) ? "<not connected>" : ctrler_2.GetName());

            if (ctrler_2 != null && ctrler_2.GetStatus() == ControllerState.STATE_READY)
            {
                Quaternion deivceQuaternion = new Quaternion(
                    -ctrler_2.GetQuaternionX(),
                    -ctrler_2.GetQuaternionZ(),
                    -ctrler_2.GetQuaternionY(),
                    ctrler_2.GetQuaternionW());
                showTransform_t.rotation = deivceQuaternion;
                Debug.Log("DeviceTwo:DeviceName:" + ctrler_2.GetName() + "DeivceQuaternion:" + deivceQuaternion);
                dataShow_t.UpdateEMGData(ctrler_2.GetEmgValue(),ctrler_2.ChannelMapCount);
                dataShow_t.UpdateQuaternionDataText(deivceQuaternion);
                dataShow_t.UpdataGestureDataText(ctrler_2.GetGesture() + "");
            }
        }
        #endregion
    }

    private void OnDestroy()
    {
        GForceHub.instance.Terminate();
    }
    #endregion

    public void RestOrientationData(Device device)
    {
        restDevice = device;
    }

    public void SetdataSwitch(DataNotifFlags value)
    {
        ctrler_1.AddDataSwitch((uint)value);
        ctrler_2.AddDataSwitch((uint)value);
    }

    public void RestDataSwitch()
    {
        ctrler_1.RestDataSwitch();
        ctrler_2.RestDataSwitch();
    }

    #region UI
    public void Tip(string value)
    {
        if (value == string.Empty)
            return;
        TipText.text = value;
        StartCoroutine(TipOver());
    }

    IEnumerator TipOver()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        TipText.text = "";
    }
    #endregion

}