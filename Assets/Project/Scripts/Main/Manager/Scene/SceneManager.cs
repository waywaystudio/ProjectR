using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine;
using Wayway.Engine.Events;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Main.Scene
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private GameEvent onSceneChangedEvent;
        [SerializeField, ReadOnly] private List<string> sceneNameList;
        // SF private LoadingSceneEffect loadingEffect;

        private string nextScene = string.Empty;
        private AsyncOperation asyncOperation;
        private Func<bool> isEffectFinished;

        public bool IsSceneChangeable
        {
            get
            {
                if (asyncOperation?.progress >= 0.9f)
                {
                    return isEffectFinished is null || isEffectFinished.Invoke();
                }
                
                return false;
            }
        }
        
        public float Progress => asyncOperation?.progress ?? 1f;
        public string NextScene => nextScene;

        public void LoadScene()
        {
            StartCoroutine(LoadSceneAsync(NextScene));
            nextScene = null;
        }
        
        public void LoadScene(string sceneName)
        {
            if (!sceneNameList.Exists(x => x == sceneName))
            {
                Debug.LogError($"Not Exist <b>{sceneName}</b> in scene name list.");
                return;
            }

            nextScene = sceneName;
            StartCoroutine(LoadSceneAsync("Loading"));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            asyncOperation = UnitySceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            while (IsSceneChangeable)
            {
                yield return null;
            }
            
            asyncOperation.allowSceneActivation = true;

            while (!asyncOperation.isDone)
            {
                yield return null;
            }
            
            onSceneChangedEvent.Invoke();
        }

#if UNITY_EDITOR
        #region EditorOnly
        [SerializeField] private List<UnityEditor.SceneAsset> sceneAssetList;
        
        /// <summary>
        /// For Editor Only Scene Translator
        /// </summary>
        /// <param name="sceneName">target SceneName</param>
        [Button]
        public void LoadSceneEditorOnly(string sceneName)
        {
            if (!sceneNameList.Exists(x => x == sceneName))
            {
                Debug.LogError($"Not Exist <b>{sceneName}</b> in scene name list.");
                return;
            }

            StartCoroutine(LoadSceneAsync(sceneName));
        }
        
        [OnInspectorInit]
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
