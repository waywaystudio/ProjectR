using System;
using System.Collections.Generic;
using Core;
using Main;
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
            MainGame.SceneManager.LoadNextScene();
        }

        private void OnDisable()
        {
            loadingEffectList.ForEach(x => x.SetActive(false));
        }
    }
}
