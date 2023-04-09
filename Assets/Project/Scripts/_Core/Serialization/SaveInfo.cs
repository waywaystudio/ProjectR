using System;
using UnityEngine;

namespace Serialization
{
    [Serializable]
    public class SaveInfo
    {
        [SerializeField] private string filename;
        [SerializeField] private string saveTime;

        public string Filename => filename;
        public string SaveTime => saveTime;

        public SaveInfo(string filename)
        {
            this.filename = filename;
            saveTime      = DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss");
        }

        public void Save() => SaveManager.Instance.SaveToFile(filename);
        public void Load() => SaveManager.Instance.LoadFromFile(filename);
        public void Delete() => SaveManager.Instance.DeleteSaveFile(filename);
    }
}
