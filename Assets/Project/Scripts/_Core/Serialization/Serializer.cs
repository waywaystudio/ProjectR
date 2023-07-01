using UnityEngine;

namespace Serialization
{
    public static class Serializer
    {
        private const string PlaySaveName = "_playSaveFile";
        private const string Extension = "json";
        
        private static string SaveFileDirectory => ES3Settings.defaultSettings.path;
        private static string PlaySavePath => GetPathByName(PlaySaveName);

        public static void Save<T>(string key, T value) => Save(key, value, PlaySavePath);
        public static void Save<T>(string key, T value, string filePath) 
            => ES3.Save(key, value, filePath);
        
        public static T Load<T>(string key, T defaultValue = default) => Load(key, PlaySavePath, defaultValue);
        public static T Load<T>(string key, string filePath, T defaultValue = default) 
            => ES3.Load(key, filePath, defaultValue);
        
        
        private static string GetPathByName(string filename) 
            => $"{SaveFileDirectory}/{filename}.{Extension}";
    }
}
