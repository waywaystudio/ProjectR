using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InstantEvent
{
    public class InstantEvent : MonoBehaviour
    {
        [ShowInInspector]
        public readonly Dictionary<string, InstantEventListener> InstantEventTable = new();

        public void Invoke()
        {
            InstantEventTable.ForEach(x => x.Value.Invoke());
        }

        public void Register(InstantEventListener listener)
        {
            InstantEventTable.TryAdd(listener.Key, listener);
        }

        public void UnRegister(InstantEventListener listener)
        {
            InstantEventTable.ContainsKey(listener.Key)
                .OnTrue(() => InstantEventTable.Remove(listener.Key));
        }
    }
}
