using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wayway.Engine.Save;

public class UISave : Savable
{
    [SerializeField] private TMP_Text sampleText;
    private ES3File es3File;
    
    public override void Save()
    {
        ES3.Save("uiSampleText", sampleText.text);
    }

    public override void Load()
    {
        sampleText.text = ES3.Load<string>("uiSampleText");
    }
}
