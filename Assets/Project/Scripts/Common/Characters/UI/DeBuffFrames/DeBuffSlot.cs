using UnityEngine;

namespace Common.Characters.UI.DeBuffFrames
{
    public class DeBuffSlot : MonoBehaviour, IEditable
    {
        [SerializeField] private DeBuffSymbol symbol;

        public bool IsRegistered => gameObject.activeSelf;

        public void Register(IStatusEffect effect)
        {
            gameObject.SetActive(true);
            symbol.Register(effect);
        }

        public void Unregister()
        {
            gameObject.SetActive(false);
            symbol.Unregister();
        }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            symbol = GetComponentInChildren<DeBuffSymbol>();
        }
#endif
    }
}
