using System.Collections.Generic;
using UnityEngine;

namespace SceneAdaption
{
    public class SceneListener : MonoBehaviour
    {
        private readonly List<ISceneChangeHandler> handlerList = new();
        
        private List<ISceneChangeHandler> HandlerList
        {
            get
            {
                if (handlerList.IsNullOrEmpty()) GetComponents(handlerList);

                return handlerList;
            }
        }

        public void SceneChanging() => HandlerList.ForEach(handler => handler.OnChanging());
        public void SceneChanged() => HandlerList.ForEach(handler => handler.OnChanged());
        

        private void OnEnable() => SceneManager.AddListener(this);
        private void OnDisable() => SceneManager.RemoveListener(this);
    }
}
