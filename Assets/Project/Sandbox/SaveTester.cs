using System.Collections.Generic;
using Manager;
using Manager.Save;
using UnityEngine;

public class SaveTester : MonoBehaviour, ISavable
{
    public int IntegerValue;
    public string StringValue;
    public float FloatValue;
    public List<int> ListValue;

    public void Save()
    {
        MainManager.Save.Save("SaveLabInt", IntegerValue);
        MainManager.Save.Save("SaveLabString", StringValue);
        MainManager.Save.Save("SaveLabFloat", FloatValue);
        MainManager.Save.Save("SaveLabListInt", ListValue);
    }

    public void Load()
    {
        IntegerValue = MainManager.Save.Load("SaveLabInt", IntegerValue);
        StringValue =  MainManager.Save.Load("SaveLabString", StringValue);
        FloatValue =   MainManager.Save.Load("SaveLabFloat", FloatValue);
        ListValue =    MainManager.Save.Load("SaveLabListInt", ListValue);
    }
}
