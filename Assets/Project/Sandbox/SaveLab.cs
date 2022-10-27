using System.Collections.Generic;
using UnityEngine;
using Wayway.Engine.Save;

public class SaveLab : MonoBehaviour, ISavable
{
    public int IntegerValue;
    public string StringValue;
    public float FloatValue;
    public List<int> ListValue;

    public void Save()
    {
        SaveManager.Save("SaveLabInt", IntegerValue);
        SaveManager.Save("SaveLabString", StringValue);
        SaveManager.Save("SaveLabFloat", FloatValue);
        SaveManager.Save("SaveLabListInt", ListValue);
    }

    public void Load()
    {
        IntegerValue = SaveManager.Load<int>("SaveLabInt");
        StringValue = SaveManager.Load<string>("SaveLabString");
        FloatValue = SaveManager.Load<float>("SaveLabFloat");
        ListValue = SaveManager.Load<List<int>>("SaveLabListInt");
    }
}
