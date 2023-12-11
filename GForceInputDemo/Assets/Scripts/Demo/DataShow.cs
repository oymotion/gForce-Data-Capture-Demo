using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataShow : MonoBehaviour
{
    [Header("EmgData")]
    [SerializeField] List<Scattergram> EmgListScattergram;
    [Header("QuaternionData")]
    [SerializeField] Text XText;
    [SerializeField] Text YText;
    [SerializeField] Text ZText;
    [SerializeField] Text WText;
    [SerializeField] Text gestureText;

    List<float> textList = new List<float>();

    float[] farray;

    List<List<float>> listFloats = new List<List<float>>();

    /// <summary>
    /// 更新肌电数据显示
    /// </summary>
    /// <param name="Values">肌电数据</param>
    /// <param name="bitCount">该设备打开的通道</param>
    public void UpdateEMGData(byte[] Values,int bitCount)
    {
        if(Values == null)
        {
            return;
        }

        if (listFloats.Count == 0) 
        {
            for (int i = 0; i < bitCount; i++)
            {
                List<float> listFloat = new List<float>();
                listFloats.Add(listFloat);
            }
        }
        else
        {
            for (int i = 0; i < bitCount; i++)
            {
                listFloats[i].Clear();
            }
        }

        for (int i = 0, j = 0; i < Values.Length; i++ )
        {
            listFloats[j].Add(Values[i]);
            if (j == bitCount -1)
            {
                j = 0;
            }
            else
            {
                j++;
            }
        }

        for (int i = 0; i < bitCount; i++)
        {
                farray = listFloats[i].ToArray();
                float angleSum = 0;
                float valueMax = 255;
                float[] shuzu=new float[farray.Length];
                for (int j = 0; j < farray.Length; j++)
                {
                    angleSum += farray[j];
                    if (valueMax < farray[j])
                        valueMax = farray[j];
                }

                for (int j = 0; j < farray.Length; j++)
                {
                    farray[j] = farray[j] / valueMax;
                }

            EmgListScattergram[i].point = farray;
            EmgListScattergram[i].SetAllDirty();
        }
    }
    
    /// <summary>
    /// 更新陀螺仪四元数数据
    /// </summary>
    /// <param name="value">陀螺仪数据</param>
    public void UpdateQuaternionDataText(Quaternion value)
    {
        XText.text = string.Format("{0:F4}", value.x);
        YText.text = string.Format("{0:F4}", value.y);
        ZText.text = string.Format("{0:F4}", value.z);
        WText.text = string.Format("{0:F4}", value.w);
    }

    /// <summary>
    /// 更新设备状态显示
    /// </summary>
    /// <param name="value">状态数据</param>
    public void UpdataGestureDataText(string value)
    {
        gestureText.text = value;
    }
}
