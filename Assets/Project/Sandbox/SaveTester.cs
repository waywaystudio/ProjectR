using System.Collections.Generic;
using Core;
using Main;
using Main.Save;
using UnityEngine;

public class SaveTester : MonoBehaviour, ISavable
{
    public int IntegerValue;
    public string StringValue;
    public float FloatValue;
    public List<int> ListValue;

    public void Save()
    {
        MainGame.SaveManager.Save("SaveLabInt", IntegerValue);
        MainGame.SaveManager.Save("SaveLabString", StringValue);
        MainGame.SaveManager.Save("SaveLabFloat", FloatValue);
        MainGame.SaveManager.Save("SaveLabListInt", ListValue);
    }

    public void Load()
    {
        IntegerValue = MainGame.SaveManager.Load("SaveLabInt", IntegerValue);
        StringValue = MainGame.SaveManager.Load("SaveLabString", StringValue);
        FloatValue = MainGame.SaveManager.Load("SaveLabFloat", FloatValue);
        ListValue = MainGame.SaveManager.Load("SaveLabListInt", ListValue);
    }
}
