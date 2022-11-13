using System.Collections.Generic;
using Core;
using Main;
using Main.Manager.Save;
using UnityEngine;

public class SaveTester : MonoBehaviour, ISavable
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
        IntegerValue = SaveManager.Load("SaveLabInt", IntegerValue);
        StringValue = SaveManager.Load("SaveLabString", StringValue);
        FloatValue = SaveManager.Load("SaveLabFloat", FloatValue);
        ListValue = SaveManager.Load("SaveLabListInt", ListValue);
    }
}
