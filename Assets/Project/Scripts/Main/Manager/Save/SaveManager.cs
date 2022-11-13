using System.Collections.Generic;
using System.Linq;
using Core;
using Main.Save;
using UnityEngine;

// ReSharper disable NotAccessedField.Local

namespace Main.Manager.Save
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private Core.GameEvents.GameEvent saveEvent;
        [SerializeField] private List<SaveInfo> saveInfoList = new (); 

        private const string PlaySaveFile = "_playSaveFile";
        private const string AutoSaveFile = "_autoSaveFile";
        private const string Extension = "es3";
        private bool isSetUp;

        public Core.GameEvents.GameEvent SaveEvent => saveEvent;
        public List<SaveInfo> SaveInfoList => saveInfoList;
        
        private string SaveFileDirectory => null;
        //  private string SaveFileDirectory => ES3Settings.defaultSettings.path;
        private string PlaySavePath => GetPath(PlaySaveFile);
        private string AutoSavePath => GetPath(AutoSaveFile);

        private void OnEnable()
        {
            if (isSetUp is not true)
                SetUp();
        }

        public void SetUp()
        {
            CreateCoreFile();

            isSetUp = true;
            RefreshSaveInfoList();
        }

        public void Save<T>(string key, T value)
        {
            // ES3.Save(key, value, PlaySavePath);
        }

        public T Load<T>(string key) => Load<T>(key, default);
        public T Load<T>(string key, T defaultValue)
        {
            return default;
            // return ES3.Load(key, PlaySavePath, defaultValue);
        }

        public void CreateNewSlot(string saveFileName)
        {
            CreateNewSaveFile(saveFileName);
            RefreshSaveInfoList();
            
            Refresh();
        }

        public void LoadFromSlot(SaveInfo saveInfo)
        {
            CopySaveFile(saveInfo.SaveName, PlaySaveFile);
            
            // TODO.
            // Scene Change Action in here?
        }

        public void SaveToSlot(SaveInfo saveInfo)
        {
            PlaySave();
            CopySaveFile(PlaySaveFile, saveInfo.SaveName);
        }

        public void DeleteSlot(SaveInfo saveInfo)
        {
            DeleteSaveFile(saveInfo.SaveName);
            RefreshSaveInfoList();
        }
        
        public void PlaySave() => SaveEvent.Invoke();

        
        private void CreateNewSaveFile(string saveFileName)
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
            
            // if (ES3.FileExists(saveFileFullPath))
            // {
            //     Debug.Log($"There is already exist <color=red>{saveFileName}</color> in Save Folder.");
            //     return;
            // }
            //
            // ES3.Save("IsValidFile", new SaveInfo(saveFileName), saveFileFullPath);
        }

        private void CreateCoreFile()
        {
            // ES3.Save("IsValidFile", new SaveInfo(PlaySaveFile), PlaySavePath);
            // ES3.Save("IsValidFile", new SaveInfo(AutoSaveFile), AutoSavePath);
        }

        private void RefreshSaveInfoList()
        {
            SaveInfoList.Clear();
            
            // var saveFileList = ES3.GetFiles(SaveFileDirectory)
            //                       .Where(file => file.EndsWith($".{Extension}"))
            //                       .Where(file => file.NotContains('_'));
            //
            // saveFileList.ForEach(saveFile =>
            // {
            //     var saveFilePath = $"{SaveFileDirectory}/{saveFile}";
            //     
            //     if (ES3.KeyExists("IsValidFile", saveFilePath))
            //     {
            //         var saveInfo = ES3.Load<SaveInfo>("IsValidFile", saveFilePath);
            //         
            //         SaveInfoList.Add(saveInfo);
            //     }
            //     else
            //     {
            //         Debug.LogWarning($"{saveFile} Is not Valid File!!!");
            //     }
            // });
            //
            // saveInfoList = SaveInfoList.OrderBy(x => x.SaveTime)
            //                                     .ToList();
            //
            // AttachAutoSaveFileToList();
        }

        private void AttachAutoSaveFileToList()
        {
            // SaveInfoList.Add(ES3.Load<SaveInfo>("IsValidFile", AutoSavePath));
        }

        private void AutoSave()
        {
            PlaySave();
            CopySaveFile(PlaySavePath, AutoSavePath);
        }
        
        private void CopySaveFile(string fromName, string destName)
        {
            if (!IsValid(fromName) || !IsValid(destName)) return;
            
            var fromSaveFile = GetPath(fromName);
            var destSaveFile = GetPath(destName);

            // ES3.CopyFile(fromSaveFile, destSaveFile);
        }

        private void DeleteSaveFile(string fileName)
        {
            var filePath = GetPath(fileName);
            // ES3.DeleteFile(filePath);

#if UNITY_EDITOR
            var metaPath = $"Assets/{filePath}.meta";
            System.IO.File.Delete(metaPath);
#endif
        }

        private bool IsValid(string fileName, bool showDebug = false)
        {
            // if (!ES3.FileExists(GetPath(fileName)))
            // {
            //     if (showDebug)
            //         Debug.LogWarning($"There isn't exist <color=red>{fileName}</color> saveFile");
            //
            //     return false;
            // }
            //
            // if (!ES3.KeyExists("IsValidFile", fileName))
            // {
            //     if (showDebug)
            //         Debug.LogWarning("Is <color=red>Not</color> IsValidFile");
            //
            //     return false;
            // }

            return true;
        }

        private string GetPath(string fileName) => $"{SaveFileDirectory}/{fileName}.{Extension}";

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
