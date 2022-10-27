using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine.Events;
using Wayway.Engine.Singleton;
// ReSharper disable NotAccessedField.Local

namespace Wayway.Engine.Save
{
    public class SaveManager : MonoSingleton<SaveManager>
    {
        [SerializeField] private GameEvent saveEvent;

        private const string PlaySaveFile = "_playSaveFile";
        private const string AutoSaveFile = "AutoSaveFile";
        private const string Extension = "es3";

        public static GameEvent SaveEvent => Instance.saveEvent;
        
        private static string SaveFileDirectory => ES3Settings.defaultSettings.path;
        private static string AutoSavePath => GetPath(AutoSaveFile);
        private static string CurrentSavePath => GetPath(PlaySaveFile);

        [Button]
        public void SetUp()
        {
            if (!ES3.FileExists(AutoSavePath))
            {
                ES3.Save("IsValidFile", true, AutoSavePath);
            }
            
            if (!ES3.FileExists(CurrentSavePath))
            {
                ES3.Save("IsValidFile", true, CurrentSavePath);
            }
        }

        public static void Save<T>(string key, T value) => ES3.Save(key, value, CurrentSavePath);
        public static T Load<T>(string key) => ES3.Load<T>(key, CurrentSavePath);

        [Button]
        public void CreateSlot(string saveFileName)
        {
            if (saveFileName.Contains('_'))
            {
                Debug.LogError("SaveFile Can't Contains under bar(_) text. Try Another Name");
                return;
            }
            
            var saveFileFullPath = GetPath(saveFileName);
            
            if (ES3.FileExists(saveFileFullPath))
            {
                Debug.Log($"There is already exist <color=red>{saveFileName}</color> in Save Folder. Creation Skipped");
                return;
            }
            
            ES3.Save("IsValidFile", true, saveFileFullPath);
            
            Refresh();
        }

        public void LoadFromSlot(string loadSlot)
        {
            var loadFilePath = GetPath(loadSlot);
            
            CopySaveFile(loadFilePath, CurrentSavePath);
        }

        public void SaveToSlot(string targetSlot)
        {
            var targetPath = GetPath(targetSlot);
            
            CopySaveFile(CurrentSavePath, targetPath);
        }
        
        private void AutoSave()
        {
            CopySaveFile(CurrentSavePath, AutoSavePath);
        }

        [Button]
        private void GetSaveFileList()
        {
            var saveFileList = ES3.GetFiles(SaveFileDirectory)
                                  .Where(file => file.EndsWith($".{Extension}"))
                                  .Where(file => file.NotContains('_'));

            saveFileList.ForEach(saveFile =>
            {
                if (ES3.KeyExists("IsValidFile", GetPath(saveFile[..^4])))
                {
                    // isValidation true. create slot list
                }
                else
                {
                    Debug.LogWarning($"{saveFile} Is not Valid File!!!");
                }
            });
        }

        [Button]
        public void LoadTest(string saveFileName)
        {
            SetUp();

            var saveFilePath = $"{SaveFileDirectory}/{saveFileName}.{Extension}";
            
            if (!ES3.FileExists(saveFilePath))
            {
                Debug.LogError($"There isn't exist {saveFilePath}. Load Failed");
            }
        }

        private void CopySaveFile(string fromName, string destName)
        {
            if (!IsValid(fromName) || !IsValid(destName)) return;
            
            var fromSaveFile = GetPath(fromName);
            var destSaveFile = GetPath(destName);

            ES3.CopyFile(fromSaveFile, destSaveFile);
        }

        private static string GetPath(string fileName)
        {
            return $"{SaveFileDirectory}/{fileName}.{Extension}";
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
