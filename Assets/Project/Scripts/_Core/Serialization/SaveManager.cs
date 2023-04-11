using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Serialization
{
    public class SaveManager : UniqueScriptableObject<SaveManager>
    {
        private readonly List<SaveListener> listenerList = new();
        private readonly List<SaveInfo> saveInfoList = new();

        private static string PlaySavePath => GetPathByName(PlaySaveName);
        private static string SaveFileDirectory => ES3Settings.defaultSettings.path;
        private const string PlaySaveName = "_playSaveFile";
        private const string InfoKey = "_SaveInfo";
        private const string Extension = "json";

        public static List<SaveInfo> SaveInfoList => Instance.saveInfoList;
        

        /// <summary>
        /// 특정 데이터를 저장하는 구현부에서 사용
        /// </summary>
        public static void Save<T>(string key, T value) => Save(key, value, PlaySavePath);

        /// <summary>
        /// 특정 데이터를 불러오는 구현부에서 사용
        /// </summary>
        public static T Load<T>(string key, T defaultValue = default) => Load(key, PlaySavePath, defaultValue);


        // TODO.어떤 방식으로 호출할지 생각해보자..
        /// <summary>
        /// SerializeManager 사용 시에 반드시 호출되어야 함.
        /// </summary>
        public void LoadAllSaveFile()
        {
            CreatePlaySaveFile();
            
            saveInfoList.Clear();
            
            GetSaveFiles.ForEach(saveFile =>
            {
                var saveFilePath = $"{SaveFileDirectory}/{saveFile}";
                
                if (!HasKeyInPath(InfoKey, saveFilePath))
                {
                    Debug.LogWarning($"Invalid SaveFile Exist. FilePath:{saveFilePath}");
                    return;
                }

                var saveInfo = Load<SaveInfo>(InfoKey, saveFilePath);
                
                saveInfoList.Add(saveInfo);
            });

            SortByTimeStamp();
        }

        /// <summary>
        /// 새로운 세이브 파일을 기초 값으로 생성 시도하며 이미 이름이 있다면 취소된다.
        /// </summary>
        public bool CreateNewSaveFile(string filename)
        {
            if (!IsValidFileName(filename)) return false;
            if (TryGetSaveInfo(filename, out _)) return false;

            var serializeInfo = new SaveInfo(filename);
            
            Save(InfoKey, serializeInfo, GetPathByName(filename));
            saveInfoList.Add(serializeInfo);
            SortByTimeStamp();
            Refresh();
            
            return true;
        }
        
        /// <summary>
        /// 새로운 세이브 파일을 현재 데이타 값으로 생성한다.
        /// </summary>
        public void SaveToNewFile(string filename)
        {
            if (!CreateNewSaveFile(filename)) return;

            SaveToFile(filename);
        }
        
        /// <summary>
        /// 현재 데이터를 이미 생성된 세이브 파일에 덮어쓴다.
        /// </summary>
        public void SaveToFile(string existSaveFileName)
        {
            if (!TryGetSaveInfo(existSaveFileName, out _)) return;

            SaveAll();

            TransferSaveInfo(PlaySaveName, existSaveFileName);
        }

        /// <summary>
        /// 기존 세이브 파일로부터 현재 데이터를 갱신한다.
        /// </summary>
        public void LoadFromFile(string fromSaveFilename)
        {
            TransferSaveInfo(fromSaveFilename, PlaySaveName);

            LoadAll();
        }

        /// <summary>
        /// 세이브 파일을 삭제한다.
        /// 에디터 상태에선 메타파일도 같이 지워준다.
        /// </summary>
        public void DeleteSaveFile(string filename)
        {
            if (TryGetSaveInfo(filename, out var result)) 
                saveInfoList.RemoveSafely(result);

            DeleteFilePath(GetPathByName(filename));
            
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
        public void ResetFile()
        {
            DeleteSaveFileDirectory(SaveFileDirectory);
            saveInfoList.Clear();
            
#if UNITY_EDITOR
            var metaPath = $"Assets/{SaveFileDirectory}.jason";
            System.IO.File.Delete(metaPath);
#endif
            LoadAllSaveFile();
            Refresh();
        }
        
        public void AddListener(SaveListener listener) => listenerList.AddUniquely(listener);
        public void RemoveListener(SaveListener listener) => listenerList.RemoveSafely(listener);
        public void SaveAll() => listenerList.ForEach(listener => listener.Save());
        public void LoadAll() => listenerList.ForEach(listener => listener.Load());

        
        private static string GetPathByName(string filename) => $"{SaveFileDirectory}/{filename}.{Extension}";

        /// <summary>
        /// 서로 다른 두 세이브 파일을 한 쪽으로 옮긴다.
        /// </summary>
        /// <param name="fromName">복사시킬 대상</param>
        /// <param name="destName">복사받을 대상</param>
        private void TransferSaveInfo(string fromName, string destName)
        {
            DeleteSaveFile(destName);
            CreateNewSaveFile(destName);
            
            ES3.GetKeys(GetPathByName(fromName)).ForEach(key =>
            {
                if (key == InfoKey) return;

                ES3.Save(key, ES3.Load(key, GetPathByName(fromName)), GetPathByName(destName));
            });
            
            Refresh();
        }
        
        private static void CreatePlaySaveFile()
        {
            DeleteFilePath(PlaySavePath);

            ES3.Save(InfoKey, new SaveInfo(PlaySaveName), PlaySavePath);
            ES3.Save("_FromPlaySaveFile", "Check", PlaySavePath);
        }

        private static bool IsValidFileName(string filename)
        {
            if (filename == PlaySaveName) return true;
            
            if (filename.StartsWith('_'))
            {
                Debug.LogWarning("Can't Create Name of StartWith _");
                return false;
            }

            if (filename == string.Empty)
            {
                Debug.LogWarning("Can't Create Empty FileName");
                return false;
            }

            return true;
        }

        private bool TryGetSaveInfo(string saveFilename, out SaveInfo result)
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

        private void SortByTimeStamp()
        {
            saveInfoList.Sort((b, a) => string.Compare(a.SaveTime, b.SaveTime, StringComparison.Ordinal));
        }

        #region ES3 Packing
        private static void Save<T>(string key, T value, string filePath) 
            => ES3.Save(key, value, filePath);

        private static T Load<T>(string key, string filePath, T defaultValue = default) 
            => ES3.Load(key, filePath, defaultValue);
        
        private static IEnumerable<string> GetSaveFiles 
            => ES3.GetFiles(SaveFileDirectory)
                  .Where(file => file.EndsWith($".{Extension}"))
                  .Where(file => file.NotContains('_'));

        private static bool HasKeyInPath(string key, string filePath)
            => ES3.KeyExists(key, filePath);
        
        private static void DeleteFilePath(string filePath) => ES3.DeleteFile(filePath);
        private static void DeleteSaveFileDirectory(string directory) => ES3.DeleteDirectory(directory);
        #endregion

        private static void Refresh()
        {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.EditorUtility.SetDirty(Instance);
#endif
        }
#if UNITY_EDITOR
        public void OpenSaveFilePath()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:Folder, SaveFile", new []{"Assets/Project/Data"});
            
            foreach (var guid in guids)
            {
                var path         = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var prefabFolder = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
                
                UnityEditor.Selection.activeObject = prefabFolder;
                UnityEditor.EditorGUIUtility.PingObject(prefabFolder);
                break;
            }
        }
#endif

        [Sirenix.OdinInspector.Button]
        public void ShowTime()
        {
            saveInfoList.ForEach(saveInfo => Debug.Log(saveInfo.SaveTime));
        }
        
    }
}
