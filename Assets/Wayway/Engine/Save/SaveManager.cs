using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine.Singleton;
// ReSharper disable NotAccessedField.Local

#pragma warning disable CS0414

namespace Wayway.Engine.Save
{
    // SaveManager;
    public class SaveManager : MonoSingleton<SaveManager>
    {
        [SerializeField] private List<SaveSlot> saveSlotList = new ();

        private const string CurrentSaveFile = "_currentSaveFile";
        private const string AutoSaveFile = "AutoSaveFile";
        private const string Extension = "es3";

        private Action saveAction;

        private static string DefaultSavePath => ES3Settings.defaultSettings.path;
        private static string AutoSaveFullPath => GetFullPath(AutoSaveFile);
        private static string CurrentSaveFullPath => GetFullPath(CurrentSaveFile);


        public static void RegisterSave(Action action)
        {
            Instance.saveAction += action;
        }

        public static void UnregisterSave(Action action)
        {
            Instance.saveAction -= action;
        }

        [Button]
        public void SaveAll() => saveAction?.Invoke();

        [Button]
        public void SetUp()
        {
            if (!ES3.FileExists(AutoSaveFullPath))
            {
                ES3.Save("Initialize", true, AutoSaveFullPath);
            }
            
            if (!ES3.FileExists(CurrentSaveFullPath))
            {
                ES3.Save("Initialize", true, CurrentSaveFullPath);
            }
            
            Refresh();
        }

        public static void Save<T>(string key, T value)
        {
            ES3.Save(key, value, CurrentSaveFullPath);
        }

        public static T Load<T>(string key)
        {
            return ES3.Load<T>(key, CurrentSaveFullPath);
        }

        public void CreateSlot(string saveFileName)
        {
            if (saveFileName.Contains('_'))
            {
                Debug.LogError("SaveFile Can't Contains under bar(_) text. Try Another Name");
                return;
            }
            
            var saveFileFullPath = GetFullPath(saveFileName);
            
            if (ES3.FileExists(saveFileFullPath))
            {
                Debug.Log($"There is already exist <color=red>{saveFileName}</color> in Save Folder. Creation Skipped");
                return;
            }
            
            ES3.Save("Initialize", true, saveFileFullPath);
        }

        public void LoadFromSlot(string targetSlotName)
        {
            var targetFullPath = GetFullPath(targetSlotName);
            
            CopySaveFile(targetFullPath, CurrentSaveFullPath);
        }

        public void SaveToSlot(string targetSlotName)
        {
            var targetFullPath = GetFullPath(targetSlotName);
            
            CopySaveFile(CurrentSaveFullPath, targetFullPath);
        }
        
        private void AutoSave()
        {
            CopySaveFile(CurrentSaveFullPath, AutoSaveFullPath);
        }

        [Button]
        private void GetSaveFileList()
        {
            var saveFileList = ES3.GetFiles(DefaultSavePath)
                                  .Where(ext => ext.EndsWith(".ecs3"))
                                  .Where(spelling => !spelling.Contains('_'));
            
            // saveSlotList

            saveFileList.ForEach(saveFile =>
            {
                var saveSlot = new SaveSlot();

                saveSlot = Load<SaveSlot>(saveFile);
            });
        }

        [Button]
        public void SaveTest(string saveFileName)
        {
            SetUp();

            var saveFilePath = $"{DefaultSavePath}/{saveFileName}.{Extension}";

            Refresh();
        }

        [Button]
        public void LoadTest(string saveFileName)
        {
            SetUp();

            var saveFilePath = $"{DefaultSavePath}/{saveFileName}.{Extension}";
            
            if (!ES3.FileExists(saveFilePath))
            {
                Debug.LogError($"There isn't exist {saveFilePath}. Load Failed");
                return;
            }
        }

        private void CopySaveFile(string fromName, string destName)
        {
            if (!IsValid(fromName) || !IsValid(destName)) return;
            
            var fromSaveFile = GetFullPath(fromName);
            var destSaveFile = GetFullPath(destName);

            ES3.CopyFile(fromSaveFile, destSaveFile);
        }

        private static string GetFullPath(string fileName)
        {
            return $"{DefaultSavePath}/{fileName}.{Extension}";
        }

        private static bool IsValid(string fileName, bool showDebug = false)
        {
            if (!ES3.FileExists(GetFullPath(fileName)))
            {
                if (showDebug)
                    Debug.Log($"There isn't exist <color=red>{fileName}</color> saveFile");

                return false;
            }

            if (!ES3.KeyExists("Initialize", fileName))
            {
                if (showDebug)
                    Debug.Log($"There isn't exist Key of <color=red>Initialize</color> in saveFile");

                return false;
            }

            return true;
        }

        private void OnDisable()
        {
            AutoSave();
        }

        private void Refresh()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}
