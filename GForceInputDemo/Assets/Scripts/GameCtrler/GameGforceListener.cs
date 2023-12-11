using GForce;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCtrler;
using gf;
using static gf.Device;

public class GameGforceListener : MonoBehaviour
{
    #region GforceListener
    public class UIListenerImpl : UIListener
    {
        GameGforceListener gameGforceListener;

        /// 
        /// <param name="device"></param>
        public override void onDeviceConnected(Device device)
        {
            Debug.Log("onDeviceConnected  address: " + device.getAddress());
        }

        /// 
        /// <param name="device"></param>
        public override void onDeviceDiscard(Device device)
        {
            Debug.Log("onDeviceDiscard  address: " + device.getAddress());

        }

        /// 
        /// <param name="device"></param>
        /// <param name="reason"></param>
        public override void onDeviceDisconnected(Device device, int reason)
        {
            Debug.Log("onDeviceDisconnected  address: " + device.getAddress());
        }

        /// 
        /// <param name="device"></param>
        public override void onDeviceFound(Device device)
        {
            Debug.Log("onDeviceFound  address: " + device.getAddress());

            gameGforceListener.FoundDevice(device);
        }

        public override void onScanFinished()
        {
            Debug.Log("onScanFinished ");

        }

        /// 
        /// <param name="state"></param>
        public override void onStateChanged(Hub.HubState state)
        {
            Debug.Log("onStateChanged  state: " + state);

        }

        ///
        /// <param name="device"></param>
        /// <param name="state"></param>
        public override void onDeviceStatusChanged(Device device, Status status)
        {
            Debug.Log("onDeviceStatusChanged  status: " + status);
            gameGforceListener.Tip("Button Event Detected! Mapped to:" + status);
            if (status == Status.ReCenter)
            {
                gameGforceListener.RestOrientationData(device);
            }
        }

        public UIListenerImpl(GameGforceListener gameGforceListener)
        {
            this.gameGforceListener = gameGforceListener;
        }
    }
    #endregion

    public enum FunctionType
    {
        none,
        Tip,
    }

    [SerializeField]GameMenu gameMenu;
    [SerializeField] GameMain gameMain;
    private UIListenerImpl uiListenerImpl;
    GForceListener gForceListener=null;

    public FunctionType functionType;
    string TipValue;

    private void Awake()
    {
        
    }

    private void Start()
    {

    }

    private void Update()
    {
        if(gForceListener==null)
        {
            gForceListener = gameMain.GetGForceListener();

            if (gForceListener != null)
            {
                Debug.Log("[GameMenu::OnEnable] gForceListener.RegisterUIListener(uiListenerImpl)");

                if (uiListenerImpl == null)
                {
                    uiListenerImpl = new UIListenerImpl(this);
                    gForceListener.RegisterUIListener(uiListenerImpl);
                }
            }
        }

        switch (functionType)
        {
            case FunctionType.none:
                break;
            case FunctionType.Tip:
                gameMain.Tip(TipValue);
                TipValue = "";
                functionType = FunctionType.none;
                break;
            default:
                break;
        }
    }


    public void Tip(string value)
    {
        functionType = FunctionType.Tip;
        TipValue = value;
    }

    public void FoundDevice(Device value)
    {
        gameMenu.FoundDevice(value);
    }

    public void RestOrientationData(Device value)
    {
        gameMain.RestOrientationData(value);
    }
}
