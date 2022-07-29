#if UNITY_EDITOR
// Editor specific code here
using UnityEditor;

[InitializeOnLoad]
public class GlobalConfig
{
    static GlobalConfig()
    {
        PlayerSettings.Android.keystorePass = "Oy123654";
        PlayerSettings.Android.keyaliasName = "key";
        PlayerSettings.Android.keyaliasPass = "Oy123654";
    }
}
#endif



