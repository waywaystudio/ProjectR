using System;
using UnityEngine;

namespace Wayway.Engine.Save
{
    [Serializable]
    public class SaveInfo
    {
        public SaveInfo(string saveName)
        {
            this.saveName = saveName;
            saveTime = DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss");
        }

        [SerializeField] private string saveName;
        [SerializeField] private string saveTime;

        public string SaveName => saveName;
        public string SaveTime => saveTime;
    }
}
