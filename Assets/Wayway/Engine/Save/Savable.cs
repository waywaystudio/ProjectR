using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Wayway.Engine.Save
{
    public class Savable : MonoBehaviour
    {
        [SerializeField] private UnityEvent saveEvent;
        [SerializeField] private UnityEvent loadEvent;

        [Button]
        public void Save() => saveEvent?.Invoke();
        
        [Button]
        public void Load() => loadEvent?.Invoke();

        private void OnEnable()
        {
            GetComponents<ISavable>().ForEach(x =>
            {
                saveEvent.AddListener(x.Save);
                loadEvent.AddListener(x.Load);
            });
            
            // SaveManager.Instance.Register(this);
        }
    }
}
