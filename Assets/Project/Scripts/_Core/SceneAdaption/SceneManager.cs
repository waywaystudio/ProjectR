using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace SceneAdaption
{
    public class SceneManager : ScriptableObject
    {
        [SerializeField] private float loadingScenePreserveTimer = 2f;
        [SerializeField] private List<string> sceneNameList;

        private List<SceneListener> ListenerList { get; } = new();
        public static string NextScene { get; private set; }

        private AsyncOperation sceneChangeOperation;
        
        /* Preset */
        public void ToRaidScene() => LoadScene("Raid");
        public void ToLobbyScene() => LoadScene("Lobby");


        public void LoadNextScene()
        {
            if (string.IsNullOrEmpty(NextScene))
                NextScene = "Town";

            LoadSceneAsync(NextScene, loadingScenePreserveTimer).Forget();
            NextScene = null;
        }

        public void LoadScene(string sceneName)
        {
            if (!sceneNameList.Exists(x => x == sceneName))
            {
                Debug.LogError($"Not Exist <b>{sceneName}</b> in scene name list.");
                return;
            }

            NextScene  = sceneName;
            LoadSceneAsync("Loading").Forget();
        }

        public void AddListener(SceneListener listener) => ListenerList.AddUniquely(listener);
        public void RemoveListener(SceneListener listener) => ListenerList.RemoveSafely(listener);

        
        private async UniTask LoadSceneAsync(string sceneName, float delayTime = 0f)
        {
            ListenerList.ForEach(listener => listener.SceneChanging());
            var operation = UnitySceneManager.LoadSceneAsync(sceneName);

            operation.allowSceneActivation = false;
            
            await UniTask.Delay(TimeSpan.FromSeconds(delayTime));
            
            operation.allowSceneActivation = true;
            ListenerList.ForEach(listener => listener.SceneChanged());
        }
    }
}
