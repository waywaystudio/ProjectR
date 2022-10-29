using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wayway.Engine.Events;
using Wayway.Engine.Singleton;
// ReSharper disable NotAccessedField.Local

namespace Wayway.Engine.Save
{
    public class SaveManager : MonoSingleton<SaveManager>
    {
        [SerializeField] private GameEvent saveEvent;
        [SerializeField] private List<SaveInfo> saveInfoList = new (); 

        private const string PlaySaveFile = "_playSaveFile";
        private const string AutoSaveFile = "_autoSaveFile";
        private const string Extension = "es3";
        private bool isSetUp;

        public GameEvent SaveEvent => saveEvent;
        public static List<SaveInfo> SaveInfoList => Instance.saveInfoList;
        
        private static string SaveFileDirectory => ES3Settings.defaultSettings.path;
        private static string AutoSavePath => GetPath(AutoSaveFile);
        private static string PlaySavePath => GetPath(PlaySaveFile);

        public void OnEnable()
        {
            if (isSetUp is not true)
                SetUp();
        }

        public void SetUp()
        {
            if (!ES3.FileExists(AutoSavePath))
            {
                CreateNewSaveFile(AutoSavePath);
            }
            
            if (!ES3.FileExists(PlaySavePath))
            {
                CreateNewSaveFile(PlaySavePath);
            }

            isSetUp = true;
            RefreshSaveInfoList();
        }

        public static void Save<T>(string key, T value)
        {
            ES3.Save(key, value, PlaySavePath);
        }

        public static T Load<T>(string key) => Load<T>(key, default);
        public static T Load<T>(string key, T defaultValue)
        {
            return ES3.Load(key, PlaySavePath, defaultValue);
        }

        public static void CreateNewSlot(string saveFileName)
        {
            CreateNewSaveFile(saveFileName);
            RefreshSaveInfoList();
            
            Instance.Refresh();
        }

        public static void LoadFromSlot(SaveInfo saveInfo)
        {
            CopySaveFile(saveInfo.SaveName, PlaySaveFile);
            
            // TODO.
            // Scene Change Action in here?
        }

        public static void SaveToSlot(SaveInfo saveInfo)
        {
            PlaySave();
            CopySaveFile(PlaySaveFile, saveInfo.SaveName);
        }

        public static void DeleteSlot(SaveInfo saveInfo)
        {
            DeleteSaveFile(saveInfo.SaveName);
            RefreshSaveInfoList();
        }
        
        public static void PlaySave() => Instance.SaveEvent.Invoke();

        
        private static void CreateNewSaveFile(string saveFileName)
        {
            if (saveFileName.IsNullOrEmpty())
            {
                Debug.LogError("SaveFile Name is Empty. Creat Slot Skipped");
                return;
            }

            if (saveFileName.Contains('_'))
            {
                Debug.LogError("SaveFile Can't Contains under bar(_) text. Try Another Name");
                return;
            }
            
            var saveFileFullPath = GetPath(saveFileName);
            
            if (ES3.FileExists(saveFileFullPath))
            {
                Debug.Log($"There is already exist <color=red>{saveFileName}</color> in Save Folder.");
                return;
            }
            
            ES3.Save("IsValidFile", new SaveInfo(saveFileName), saveFileFullPath);
        }

        private static void RefreshSaveInfoList()
        {
            SaveInfoList.Clear();
            
            var saveFileList = ES3.GetFiles(SaveFileDirectory)
                                  .Where(file => file.EndsWith($".{Extension}"))
                                  .Where(file => file.NotContains('_'));

            saveFileList.ForEach(saveFile =>
            {
                var saveFilePath = $"{SaveFileDirectory}/{saveFile}";
                
                if (ES3.KeyExists("IsValidFile", saveFilePath))
                {
                    var saveInfo = ES3.Load<SaveInfo>("IsValidFile", saveFilePath);
                    
                    SaveInfoList.Add(saveInfo);
                }
                else
                {
                    Debug.LogWarning($"{saveFile} Is not Valid File!!!");
                }
            });

            Instance.saveInfoList = SaveInfoList.OrderBy(x => x.SaveTime)
                                                .ToList();

            AttachAutoSaveFileToList();
        }

        private static void AttachAutoSaveFileToList()
        {
            SaveInfoList.Add(ES3.Load<SaveInfo>("IsValidFile", AutoSavePath));
        }

        private void AutoSave()
        {
            PlaySave();
            CopySaveFile(PlaySavePath, AutoSavePath);
        }
        
        private static void CopySaveFile(string fromName, string destName)
        {
            if (!IsValid(fromName) || !IsValid(destName)) return;
            
            var fromSaveFile = GetPath(fromName);
            var destSaveFile = GetPath(destName);

            ES3.CopyFile(fromSaveFile, destSaveFile);
        }

        private static void DeleteSaveFile(string fileName)
        {
            var filePath = GetPath(fileName);
            ES3.DeleteFile(filePath);

#if UNITY_EDITOR
            var metaPath = $"Assets/{filePath}.meta";
            System.IO.File.Delete(metaPath);
#endif
        }

        private static bool IsValid(string fileName, bool showDebug = false)
        {
            if (!ES3.FileExists(GetPath(fileName)))
            {
                if (showDebug)
                    Debug.LogWarning($"There isn't exist <color=red>{fileName}</color> saveFile");

                return false;
            }

            if (!ES3.KeyExists("IsValidFile", fileName))
            {
                if (showDebug)
                    Debug.LogWarning("Is <color=red>Not</color> IsValidFile");

                return false;
            }

            return true;
        }

        private static string GetPath(string fileName) => $"{SaveFileDirectory}/{fileName}.{Extension}";

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
