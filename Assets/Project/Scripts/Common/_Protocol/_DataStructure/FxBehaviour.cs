using UnityEngine;

namespace Common
{
    public abstract class FxBehaviour<T> : MonoBehaviour, IEditable where T : class
    {
        [SerializeField] protected T master;


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            master = GetComponentInParent<T>();
            
            Verify.IsNotNull(master, "Missing Projectile Class in InstantShotProjectileFx");
        }
#endif
    }
}
