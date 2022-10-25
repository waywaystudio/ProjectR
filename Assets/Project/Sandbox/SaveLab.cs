using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SaveLab : MonoBehaviour
{
    public int IntegerValue;
    public string StringValue;
    public float FloatValue;
    public List<int> ListValue;

    [FolderPath]
    public string defaultPath = "Project/Data/Save";
    public string saveFile1 = "NewSaveFile1.es3";
    public string saveFile2 = "NewSaveFile2.es3";
    public string saveFile3 = "NewSaveFile3.es3";
    
    

    [Button]
    public void LoadFrom1()
    {
        IntegerValue = ES3.Load<int>("IntegerValue", $"{defaultPath}/{saveFile1}");
    }

    [Button]
    public void SaveAtSlot1()
    {
        ES3.Save("IntegerValue", IntegerValue, $"{defaultPath}/{saveFile1}");
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
