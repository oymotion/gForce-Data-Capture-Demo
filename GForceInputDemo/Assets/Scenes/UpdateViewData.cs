using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateViewData : MonoBehaviour
{
    public Text Text_Position;
    public Slider slider_z;
    public Image imageA;
    public Image imageB;
    public Image imageC;
    public Image imageD;

    public void setPosition(float _x, float _y, float _z)
    {
        Text_Position.GetComponent<RectTransform>().localPosition = new Vector3((_x - 0.5f) * 400f, (_y - 0.5f) * 400f, 0);
        slider_z.value = _z;
    }

    public void setABCD(float _a, float _b, float _c, float _d)
    {
        imageA.color = (_a > 0.5f) ? Color.red : Color.white;
        imageB.color = (_b > 0.5f) ? Color.red : Color.white;
        imageC.color = (_c > 0.5f) ? Color.red : Color.white;
        imageD.color = (_d > 0.5f) ? Color.red : Color.white;
    }

}
