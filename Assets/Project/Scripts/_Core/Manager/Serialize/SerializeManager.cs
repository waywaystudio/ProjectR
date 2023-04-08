using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Manager.Serialize
{
    public class SerializeManager : UniqueScriptableObject<SerializeManager>
    {
        [ShowInInspector] 
        private List<SerializeListener> listenerList = new();
        
        [ShowInInspector] 
        private List<SerializeInfo> saveInfoList = new();

        private const string PlaySaveName = "_playSaveFile";
        private const string SaveInfo = "_SaveInfo";
        private const string Extension = "json";

        private static string PlaySavePath => GetPathByName(PlaySaveName);
        private static string SaveFileDirectory => ES3Settings.defaultSettings.path;

        public static string GetPathByName(string filename) => $"{SaveFileDirectory}/{filename}.{Extension}";

        /// <summary>
        /// SerializeManager 사용 시에 반드시 호출되어야 함. 
        /// </summary>
        [Button]
        public void LoadAllSaveFile()
        {
            CreatePlaySaveFile();
            
            saveInfoList.Clear();
            
            var saveFileList = ES3.GetFiles(SaveFileDirectory)
                                  .Where(file => file.EndsWith($".{Extension}"))
                                  .Where(file => file.NotContains('_'));
            
            saveFileList.ForEach(saveFile =>
            {
                var saveFilePath = $"{SaveFileDirectory}/{saveFile}";
                if (!ES3.KeyExists(SaveInfo, saveFilePath))
                {
                    Debug.LogWarning($"Invalid SaveFile Exist. FilePath:{saveFilePath}");
                    return;
                }
                
                var saveInfo = ES3.Load<SerializeInfo>(SaveInfo, saveFilePath);
                if (saveInfo.FilePath != saveFilePath)
                {
                    Debug.Log($"SaveFile Name Changed. FilePath:{saveFilePath}");
                }
                
                saveInfoList.Add(saveInfo);
            });
            
            saveInfoList.Sort((a, b) => string.Compare(a.SaveTime, b.SaveTime, StringComparison.Ordinal));
        }

        /// <summary>
        /// 새로운 세이브 파일을 기초 값으로 생성 시도하며 이미 이름이 있다면 취소된다.
        /// </summary>
        [Button]
        public bool CreateNewSaveFile(string filename)
        {
            if (TryGetSaveInfo(filename, out _)) return false;

            LoadAllSaveFile();

            var serializeInfo = new SerializeInfo(filename, GetPathByName(filename));
            
            ES3.Save(SaveInfo, serializeInfo, GetPathByName(filename));
            saveInfoList.Add(serializeInfo);
            Refresh();
            
            return true;
        }
        
        /// <summary>
        /// 새로운 세이브 파일을 현재 데이타 값으로 생성한다.
        /// </summary>
        [Button]
        public void SaveToNewFile(string filename)
        {
            if (!CreateNewSaveFile(filename)) return;

            SaveToFile(filename);
        }
        
        /// <summary>
        /// 이미 생성된 세이브 파일을 덮어쓴다.
        /// </summary>
        [Button]
        public void SaveToFile(string existSaveFileName)
        {
            if (!TryGetSaveInfo(existSaveFileName, out _)) return;

            SaveAll();

            TransferSaveInfo(PlaySaveName, existSaveFileName);
        }

        /// <summary>
        /// 생성된 세이브 파일로부터 데이타를 받아온다.
        /// </summary>
        [Button]
        public void LoadFromFile(string fromSaveFilename)
        {
            TransferSaveInfo(fromSaveFilename, PlaySaveName);

            LoadAll();
        }

        /// <summary>
        /// 세이브 파일을 삭제한다.
        /// 에디터 상태에선 메타파일도 같이 지워준다.
        /// </summary>
        [Button]
        public void DeleteSaveFile(string filename)
        {
            if (!TryGetSaveInfo(filename, out var result)) return;
            
            saveInfoList.RemoveSafely(result);
            ES3.DeleteFile(GetPathByName(filename));
            
#if UNITY_EDITOR
            var metaPath = $"Assets/{GetPathByName(filename)}.meta";
            System.IO.File.Delete(metaPath);
            Refresh();
#endif
        }

        /// <summary>
        /// 세이브 파일 상태를 초기화 한다.
        /// 세이브 시스템에 핵심 파일인 _playSaveFile 을 초기화 한다.
        /// </summary>
        [Button]
        public void ResetFile()
        {
            ES3.DeleteDirectory(SaveFileDirectory);
            saveInfoList.Clear();
            
#if UNITY_EDITOR
            var metaPath = $"Assets/{SaveFileDirectory}.meta";
            System.IO.File.Delete(metaPath);
#endif
            LoadAllSaveFile();
            Refresh();
        }
        
        public void AddListener(SerializeListener listener) => listenerList.AddUniquely(listener);
        public void RemoveListener(SerializeListener listener) => listenerList.RemoveSafely(listener);
        public void SaveAll() => listenerList.ForEach(listener => listener.Save());
        public void LoadAll() => listenerList.ForEach(listener => listener.Load());
        
        public static void Save<T>(string key, T value)
        {
            ES3.Save(key, value, PlaySavePath);
        }

        public static T Load<T>(string key) => Load<T>(key, default);
        public static T Load<T>(string key, T defaultValue)
        {
            return ES3.Load(key, PlaySavePath, defaultValue);
        }


        /// <summary>
        /// 서로 다른 두 세이브 파일을 한 쪽으로 옮긴다.
        /// </summary>
        /// <param name="fromName">복사시킬 대상</param>
        /// <param name="destName">복사받을 대상</param>
        private static void TransferSaveInfo(string fromName, string destName)
        {
            ES3.DeleteFile(GetPathByName(destName));
            ES3.Save(SaveInfo, new SerializeInfo(destName, GetPathByName(destName)), GetPathByName(destName));
            ES3.GetKeys(GetPathByName(fromName)).ForEach(key =>
            {
                if (key == SaveInfo) return;
                
                Debug.Log(key);
                ES3.Save(key, ES3.Load(key, GetPathByName(fromName)), GetPathByName(destName));
            });
            
            Refresh();
        }
        
        private static void CreatePlaySaveFile()
        {
            ES3.Save(SaveInfo,            new SerializeInfo(PlaySaveName, PlaySavePath), GetPathByName(PlaySaveName));
            ES3.Save("_FromPlaySaveFile", "Check",                         GetPathByName(PlaySaveName));
        }

        private bool TryGetSaveInfo(string saveFilename, out SerializeInfo result)
        {
            result = null;
            
            foreach (var info in saveInfoList)
            {
                if (info.Filename != saveFilename) continue;
                
                result = info;
                break;
            }

            return result != null;
        }

        private static void Refresh()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}
