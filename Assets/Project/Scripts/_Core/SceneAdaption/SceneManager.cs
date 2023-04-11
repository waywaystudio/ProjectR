using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace SceneAdaption
{
    public class SceneManager : UniqueScriptableObject<SceneManager>
    {
        [SerializeField] private float loadingScenePreserveTimer = 2f;
        [SerializeField] private List<string> sceneNameList;

        private static List<SceneListener> ListenerList { get; } = new();
        public static string NextScene { get; private set; }

        private AsyncOperation sceneChangeOperation;
        
        /* Preset */
        public void ToRaidScene() => LoadScene("Raid");
        public void ToLobbyScene() => LoadScene("Lobby");


        public static void LoadNextScene()
        {
            if (string.IsNullOrEmpty(NextScene))
                NextScene = "Town";

            LoadSceneAsync(NextScene, Instance.loadingScenePreserveTimer).Forget();
            NextScene = null;
        }

        public static void LoadScene(string sceneName)
        {
            if (!Instance.sceneNameList.Exists(x => x == sceneName))
            {
                Debug.LogError($"Not Exist <b>{sceneName}</b> in scene name list.");
                return;
            }

            NextScene  = sceneName;
            LoadSceneAsync("Loading").Forget();
        }

        public static void AddListener(SceneListener listener) => ListenerList.AddUniquely(listener);
        public static void RemoveListener(SceneListener listener) => ListenerList.RemoveSafely(listener);

        
        private static async UniTask LoadSceneAsync(string sceneName, float delayTime = 0f)
        {
            ListenerList.ForEach(listener => listener.SceneChanging());
            var operation = UnitySceneManager.LoadSceneAsync(sceneName);

            operation.allowSceneActivation = false;
            
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            
            operation.allowSceneActivation = true;
            ListenerList.ForEach(listener => listener.SceneChanged());
        }
        
#if UNITY_EDITOR
        #region EditorOnly
        [SerializeField] private List<UnityEditor.SceneAsset> sceneAssetList;
        
        /// <summary>
        /// For Editor Only Scene Translator
        /// </summary>
        /// <param name="sceneName">target SceneName</param>
        [Sirenix.OdinInspector.Button]
        public void LoadSceneEditorOnly(string sceneName)
        {
            if (!sceneNameList.Exists(x => x == sceneName))
            {
                Debug.LogError($"Not Exist <b>{sceneName}</b> in scene name list.");
                return;
            }

            LoadSceneAsync(sceneName).Forget();
        }
        
        [Sirenix.OdinInspector.Button]
        private void AssignSceneNameList()
        {
            if (sceneNameList.IsNullOrEmpty()) 
                sceneNameList = new List<string>();
            
            sceneNameList.Clear();
            sceneAssetList.ForEach(x => sceneNameList.Add(x.name));
        }
        #endregion
#endif
    }
}
