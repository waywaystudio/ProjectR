using System.Collections.Generic;
using System.Linq;
using GameEvents;
using UnityEngine;

// ReSharper disable MemberCanBeMadeStatic.Global
// ReSharper disable NotAccessedField.Local

namespace Manager.Save
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private GameEvent saveEvent;
        [SerializeField] private List<SaveInfo> saveInfoList = new ();

        private const string PlaySaveFile = "_playSaveFile";
        private const string AutoSaveFile = "_autoSaveFile";
        private const string Extension = "json";
        private const string SaveInfoKey = "IsSaveInfo";
        private bool isInitiated;

        public GameEvent SaveEvent => saveEvent;
        public List<SaveInfo> SaveInfoList => saveInfoList;

        private static string SaveFileDirectory => ES3Settings.defaultSettings.path;
        private static string PlaySavePath => GetFilePath(PlaySaveFile);
        private static string AutoSavePath => GetFilePath(AutoSaveFile);

        [Sirenix.OdinInspector.Button]
        public void Initialize()
        {
            if (isInitiated) return;
            if (!TryGetSaveInfo(PlaySaveFile, out _)) ES3.Save(SaveInfoKey, new SaveInfo(PlaySaveFile), PlaySavePath);
            if (!TryGetSaveInfo(AutoSaveFile, out _)) ES3.Save(SaveInfoKey, new SaveInfo(AutoSaveFile), AutoSavePath);

            isInitiated = true;
            RefreshSaveInfoList();
        }

        public void Save<T>(string key, T value)
        {
            ES3.Save(key, value, PlaySavePath);
        }

        public T Load<T>(string key) => Load<T>(key, default);
        public T Load<T>(string key, T defaultValue)
        {
            return ES3.Load(key, PlaySavePath, defaultValue);
        }

        public void Clear(string key)
        {
            ES3.DeleteKey(key, PlaySavePath);
        }

        public void PlaySave() => SaveEvent.Invoke();
        

        private void RefreshSaveInfoList()
        {
            SaveInfoList.Clear();
            
            var saveFileList = ES3.GetFiles(SaveFileDirectory)
                                  .Where(file => file.EndsWith($".{Extension}"))
                                  .Where(file => file.NotContains('_'));
            
            saveFileList.ForEach(saveFile =>
            {
                var saveFilePath = $"{SaveFileDirectory}/{saveFile}";
                
                if (ES3.KeyExists(SaveInfoKey, saveFilePath))
                {
                    var saveInfo = ES3.Load<SaveInfo>(SaveInfoKey, saveFilePath);
                    
                    SaveInfoList.Add(saveInfo);
                }
                else
                {
                    Debug.LogWarning($"{saveFile} Is not Valid File!!!");
                }
            });
            
            saveInfoList = SaveInfoList.OrderBy(x => x.SaveTime)
                                       .ToList();
            
            AttachAutoSaveFileToList();
            Refresh();
        }

        private void AttachAutoSaveFileToList()
        {
            SaveInfoList.Add(ES3.Load<SaveInfo>(SaveInfoKey, AutoSavePath));
        }

        private void AutoSave()
        {
            PlaySave();
            CopySaveFile(PlaySavePath, AutoSavePath);
        }
        
        private void CreateNewSaveFile(string saveFileName)
        {
            if (saveFileName.IsNullOrEmpty() || saveFileName.Contains('_'))
            {
                Debug.LogError("fileName is not profit covenant");
                return;
            }

            var saveFileFullPath = GetFilePath(saveFileName);
            
            if (ES3.FileExists(saveFileFullPath))
            {
                Debug.Log($"There is already exist <color=red>{saveFileName}</color> in Save Folder.");
                return;
            }
            
            ES3.Save(SaveInfoKey, new SaveInfo(saveFileName), saveFileFullPath);
        }
        
        private static void CopySaveFile(string fromName, string destName)
        {
            if (!IsValid(fromName) || !IsValid(destName)) return;
            
            var fromSaveFile = GetFilePath(fromName);
            var destSaveFile = GetFilePath(destName);

            ES3.CopyFile(fromSaveFile, destSaveFile);
        }

        private static void DeleteSaveFile(string fileName)
        {
            var filePath = GetFilePath(fileName);
            ES3.DeleteFile(filePath);

#if UNITY_EDITOR
            var metaPath = $"Assets/{filePath}.meta";
            System.IO.File.Delete(metaPath);
#endif
        }

        private static bool IsValid(string fileName, bool showDebug = false)
        {
            if (!ES3.FileExists(GetFilePath(fileName)))
            {
                if (showDebug)
                    Debug.LogWarning($"There isn't exist <color=red>{fileName}</color> saveFile");
            
                return false;
            }
            
            if (!ES3.KeyExists(SaveInfoKey, fileName))
            {
                if (showDebug)
                    Debug.LogWarning("Is <color=red>Not</color> IsValidFile");
            
                return false;
            }

            return true;
        }

        private static string GetFilePath(string fileName) => $"{SaveFileDirectory}/{fileName}.{Extension}";

        private static bool TryGetSaveInfo(string fileName, out SaveInfo saveInfo)
        {
            if (ES3.FileExists(GetFilePath(fileName)))
            {
                saveInfo = ES3.Load<SaveInfo>(SaveInfoKey, GetFilePath(fileName));
                return true;
            }

            saveInfo = null;
            return false;
        }

        private void OnDisable()
        {
            AutoSave();
        }

        private static void Refresh()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        
#if UNITY_EDITOR
        private void ResetSave()
        {
            isInitiated = false;
            
            DeleteSaveFile(AutoSaveFile);
            DeleteSaveFile(PlaySaveFile);
            
            ES3.Save(SaveInfoKey, new SaveInfo(AutoSaveFile), AutoSavePath);
            ES3.Save(SaveInfoKey, new SaveInfo(PlaySaveFile), PlaySavePath);

            Refresh();
        }
#endif
    }
}
