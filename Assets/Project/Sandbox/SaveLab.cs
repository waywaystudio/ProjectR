using UnityEngine;
using Sirenix.OdinInspector;

public class SaveLab : MonoBehaviour
{
    public int IntegerValue;

    [FolderPath]
    public string defaultPath = "Project/Data/Save";
    public string saveFile1 = "NewSaveFile1.es3";
    public string saveFile2 = "NewSaveFile2.es3";
    public string saveFile3 = "NewSaveFile3.es3";
    
    [Button]
    public void Save()
    {
        ES3.Save("IntegerValue", IntegerValue);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
        Debug.Log(IntegerValue + " : UI Save Loaded");
    }

    [Button]
    public void LoadFrom1()
    {
        IntegerValue = ES3.Load<int>("IntegerValue", $"{defaultPath}/{saveFile1}");
    }
    
    [Button]
    public void LoadFrom2()
    {
        IntegerValue = ES3.Load<int>("IntegerValue", $"{defaultPath}/{saveFile2}");
    }
    
    [Button]
    public void LoadFrom3()
    {
        IntegerValue = ES3.Load<int>("IntegerValue", $"{defaultPath}/{saveFile3}");
    }

    [Button]
    public void SaveAtSlot1()
    {
        ES3.Save("IntegerValue", IntegerValue, $"{defaultPath}/{saveFile1}");
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
    
    [Button]
    public void SaveAtSlot2()
    {
        ES3.Save("IntegerValue", IntegerValue, $"{defaultPath}/{saveFile2}");
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
    
    [Button]
    public void SaveAtSlot3()
    {
        ES3.Save("IntegerValue", IntegerValue, $"{defaultPath}/{saveFile3}");
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
