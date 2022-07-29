using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class DataPanel : MonoBehaviour
{
    static int dataLine = 0;
    [SerializeField] Text dataLineText;
    static float runTime = 0;
    static bool isrun = false;
    [SerializeField] Button beginButton;
    [SerializeField] Button overButton;

    static StringBuilder content = new StringBuilder();

    static float beginTime;
    static float overTime;

    /// <summary>
    /// Add Data To Save
    /// </summary>
    /// <param name="values"></param>
    /// <param name="deviceName"></param>
    public static void AddData(byte[] values,string deviceName)
    {

        if(!isrun)
        {
            return;
        }

        content.Append(deviceName);
        content.Append(": ");
        for (int i = 0; i < values.Length; i++)
        {
            content.Append(values[i] +" ");
        }
        content.Append("\n");
        dataLine++;
    }


    private void Start()
    {
        overButton.onClick.AddListener(SaveData);
        beginButton.onClick.AddListener(()=> 
        {
            isrun = true;
            beginTime = Time.time;
        });
    }

    private void Update()
    {

        if (!isrun)
        {
            return;
        }

        runTime = Time.time - beginTime;
        dataLineText.text = "RunTime:" + runTime + "sample:" + dataLine;

    }

    /// <summary>
    /// Save Sample Data
    /// </summary>
    void SaveData()
    {

        isrun = false;
        runTime = Time.time - beginTime;
        dataLineText.text = "time:" + runTime + "sample:" + dataLine;

        string path = Application.persistentDataPath + "/"+ "time" + runTime+"Data.txt";
        Debug.Log(path);
        using (FileStream nFile = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(nFile, System.Text.Encoding.Default))
            {
                sw.Write(content.ToString());
                sw.Close();
            }
        }

        dataLine = 0;
        content.Clear();
    }

}
