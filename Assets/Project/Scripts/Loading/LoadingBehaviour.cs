using System.Collections.Generic;
using SceneAdaption;
using UnityEngine;

namespace Loading
{
    public class LoadingBehaviour : MonoBehaviour
    {
        [SerializeField] private List<GameObject> loadingEffectList = new ();

        private void Awake()
        {
            if (loadingEffectList.IsNullOrEmpty())
            {
                Debug.LogError("Loading Effect List is Null!");
                return;
            }

            var loadingObject = loadingEffectList.Random();
            loadingObject.SetActive(true);
        }

        private void Start()
        {
            SceneManager.LoadNextScene();
            // MainManager.Scene.LoadNextScene();
        }

        private void OnDisable()
        {
            loadingEffectList.ForEach(x => x.SetActive(false));
        }
    }
}
