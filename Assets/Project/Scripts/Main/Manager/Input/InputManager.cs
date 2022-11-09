using System.Collections.Generic;
using UnityEngine;
using Wayway.Engine;

namespace Main.Input
{
    public class InputManager : MonoBehaviour
    {
        private IControlModel controlModel;
        private readonly List<IEventModel> eventModelList = new ();

        public void Register(IControlModel model) => controlModel = model;
        public void Register(IEventModel model) => eventModelList.AddUniquely(model);
        public void Unregister() => controlModel = null;
        public void Unregister(IEventModel model) => eventModelList.RemoveSafely(model);
        
        public bool GetPermission(IControlModel model) => controlModel == model;
        
        public void InvokeEvent()
        {
            if (eventModelList.IsNullOrEmpty())
            {
                Debug.LogWarning("EventModelList is Empty");
                return;
            }
            
            eventModelList.ForEach(x => x.Invoke());
        }

        private void FixedUpdate() => controlModel?.UpdateState();
    }
}