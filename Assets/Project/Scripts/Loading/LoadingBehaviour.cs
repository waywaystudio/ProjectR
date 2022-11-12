using System.Collections.Generic;
using Core;
using Main;
using UnityEngine;
using Wayway.Engine;
using Debug = Debug;

namespace Loading
{
    public class LoadingBehaviour : MonoBehaviour
    {
        [SerializeField] private List<GameObject> loadingEffectList = new ();

        private void Awake()
        {
            if (loadingEffectList.IsNullOrEmpty())
            {
                global::Debug.LogError("Loading Effect List is Null!");
                return;
            }

            var loadingObject = loadingEffectList.Random();
            loadingObject.SetActive(true);

            var tester = "";
            tester.NotContains('/');
        }

        private void Start()
        {
            MainGame.SceneManager.LoadNextScene();
        }
    }
}
