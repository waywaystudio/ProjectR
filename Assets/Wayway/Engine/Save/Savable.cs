using Sirenix.OdinInspector;
using UnityEngine;

namespace Wayway.Engine.Save
{
    public abstract class Savable : MonoBehaviour
    {
        [Button]
        public abstract void Save();
        
        [Button]
        public abstract void Load();
    }
}
