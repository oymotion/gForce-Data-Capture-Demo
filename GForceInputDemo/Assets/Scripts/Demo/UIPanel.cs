using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [SerializeField] Button DataShowButton;
    [SerializeField] Button ModelShowButton;
    [SerializeField] Button DataSizeButton;
    [SerializeField] GameObject panle;
    [SerializeField] GameMain gamemain;
    [SerializeField] GameObject dataSizePanel;

    private void Awake()
    {
        DataShowButton.onClick.AddListener(() =>
        {
            panle.SetActive(true);
            dataSizePanel.SetActive(false);
            gamemain.RestDataSwitch();
            gamemain.SetdataSwitch(gf.DataNotifFlags.DNF_EMG_RAW);
        });

        ModelShowButton.onClick.AddListener(() =>
        {
            panle.SetActive(false);
            dataSizePanel.SetActive(false);
            gamemain.RestDataSwitch();
            gamemain.SetdataSwitch(gf.DataNotifFlags.DNF_EMG_GESTURE);
        });

        DataSizeButton.onClick.AddListener(() =>
        {
            dataSizePanel.SetActive(true);
        });
    }
}
