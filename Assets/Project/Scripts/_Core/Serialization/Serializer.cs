namespace Serialization
{
    public static class Serializer
    {
        private const string PlaySaveName = "_playSaveFile";
        private const string Extension = "json";
        
        private static string SaveFileDirectory => ES3Settings.defaultSettings.path;
        private static string PlaySavePath => GetPathByName(PlaySaveName);

        /// <summary>
        /// 특정 데이터를 저장하는 구현부에서 사용
        /// </summary>
        public static void Save<T>(string key, T value) => Save(key, value, PlaySavePath);
        public static void Save<T>(string key, T value, string filePath) 
            => ES3.Save(key, value, filePath);
        
        
        /// <summary>
        /// 특정 데이터를 불러오는 구현부에서 사용
        /// </summary>
        public static T Load<T>(string key, T defaultValue = default) => Load(key, PlaySavePath, defaultValue);
        public static T Load<T>(string key, string filePath, T defaultValue = default) 
            => ES3.Load(key, filePath, defaultValue);
        
        
        private static string GetPathByName(string filename) 
            => $"{SaveFileDirectory}/{filename}.{Extension}";
    }
}
