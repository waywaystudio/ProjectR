using System;
using UnityEngine;

namespace Manager.Serialize
{
    [Serializable]
    public class SerializeInfo
    {
        [SerializeField] private string filename;
        [SerializeField] private string filePath;
        [SerializeField] private string saveTime;

        public string Filename => filename;
        public string FilePath => filePath;
        public string SaveTime => saveTime;

        public SerializeInfo(string filename, string filePath)
        {
            this.filename = filename;
            this.filePath = filePath;
            saveTime      = DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss");
        }
    }
}
