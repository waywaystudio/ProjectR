using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Wayway.Engine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Main.Scene
{
    public class SceneManager : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField] private List<string> sceneNameList;
        
        private AsyncOperation asyncOperation;
        
        [Button]
        public void LoadScene(string sceneName)
        {
            if (!sceneNameList.Exists(x => x == sceneName))
            {
                Debug.LogError($"Not Exist <b>{sceneName}</b> in scene name list.");
                return;
            }

            UnitySceneManager.LoadScene(sceneName);
        }

        // ------------
#if UNITY_EDITOR
        [SerializeField] private List<SceneAsset> sceneAssetList;
        [OnInspectorInit]
        private void AssignSceneNameList()
        {
            if (sceneNameList.IsNullOrEmpty()) 
                sceneNameList = new List<string>();
            
            sceneNameList.Clear();
            sceneAssetList.ForEach(x => sceneNameList.Add(x.name));
        }
#endif
    }
}
