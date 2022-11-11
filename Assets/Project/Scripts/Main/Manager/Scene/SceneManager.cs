using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Wayway.Engine;
using GameEvent.Listener;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Main.Scene
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private GameEvent.GameEvent beforeChanged;
        [SerializeField] private GameEvent.GameEvent afterChanged;
        [SerializeField, ReadOnly] private List<string> sceneNameList;

        private AsyncOperation asyncOperation;
        private WaitForSeconds waitFadeDuration = new (2f);
        private Func<bool> isEffectFinished;

        public float Progress => asyncOperation?.progress ?? 0f;
        public string NextScene { get; private set; } = string.Empty;

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

        public void LoadNextScene()
        {
            if (string.IsNullOrEmpty(NextScene))
                NextScene = "Town";
            
            StartCoroutine(LoadSceneAsync(NextScene));
            NextScene = null;
        }
        
        public void LoadScene(string sceneName)
        {
            if (!sceneNameList.Exists(x => x == sceneName))
            {
                Debug.LogError($"Not Exist <b>{sceneName}</b> in scene name list.");
                return;
            }

            NextScene = sceneName;
            StartCoroutine(LoadSceneAsync("Loading"));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            beforeChanged.Invoke();
            
            asyncOperation = UnitySceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            yield return waitFadeDuration;
            
            asyncOperation.allowSceneActivation = true;

            while (!IsSceneChangeable && !asyncOperation.isDone)
            {
                yield return null;
            }
            
            afterChanged.Invoke();
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
