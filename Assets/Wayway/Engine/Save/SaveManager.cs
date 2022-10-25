using System.Collections.Generic;
using System.IO;
using System.Linq;
using ES3Internal;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;
using Wayway.Engine.Singleton;
// ReSharper disable NotAccessedField.Local

#pragma warning disable CS0414

namespace Wayway.Engine.Save
{
    // SaveManager;
    // SaveEvent, LoadEvent; : GameEvent;
    // SaveEventListener;
    // ISavable (for inherit to Class);
    public class SaveManager : MonoSingleton<SaveManager>
    {
        [SerializeField] private int integerValue;
        [SerializeField] private string stringValue;
        [SerializeField] private List<SaveSlot> saveSlotList;

        private const string SaveFileManager = "SaveFileManager";
        private const string CurrentSaveFile = "CurrentSaveFile";
        private const string AutoSaveFile = "AutoSaveFile";
        private const string DefaultSaveFileName = "Save";
        private const string Extension = "es3";
        private static string DefaultSavePath => ES3Settings.defaultSettings.path;

        [Button]
        public void SetUp()
        {
            var saveFileManagerPath = $"{DefaultSavePath}/{SaveFileManager}.{Extension}";
            
            ES3.Init();
            
            if (!ES3.FileExists(saveFileManagerPath))
            {
                ES3.Save("Initialize", true, saveFileManagerPath);
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif
            }
            // 세이브 파일 매니저 필요없을 수도 있다.
            
            // ES3.GetFiles(DefaultSavePath)
            //    .Where(fileName => fileName.EndsWith(".es3"))
            //    .ForEach(saveFileName =>
            //    {
            //        if (ES3.KeyExists(saveFileName[..^4], saveFileManagerPath))
            //        {
            //            
            //        }
            //    });

            // Generate SaveSlot by [*.es3]
            // Find fileName in SaveFileManager;
            // if find, go on
            // else not find, check InstanceID
            // if find, SaveFileManager key Try change by fileName;
            // if key exist, generate unique key

            // saveFileList.ForEach(Debug.Log);
        }

        [Button]
        public void SaveTest(string saveFileName)
        {
            SetUp();

            var saveFilePath = $"{DefaultSavePath}/{saveFileName}.{Extension}";

            ES3.Save("saveFileName", saveFileName, saveFilePath);
            ES3.Save("integerValue", integerValue, saveFilePath);
            ES3.Save("stringValue", stringValue, saveFilePath);
            // 하나의 클래스로 묶어보자.
        }

        public void SaveOriginalToDest(string original, string dest)
        {
            // var originalFilePath = GetSaveFileFullPath(original);
            // var destFilePath = GetSaveFileFullPath(dest);

            // ES3.CopyFile(originalFilePath, destFilePath);
        }
    }
}
