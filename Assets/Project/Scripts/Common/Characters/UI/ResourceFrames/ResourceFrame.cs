using Common.UI;
using UnityEngine;

namespace Common.Characters.UI.ResourceFrames
{
    public class ResourceFrame : MonoBehaviour
    {
        [SerializeField] private ImageFiller resourceBar;

        private CharacterBehaviour cb;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        private void OnEnable()
        {
            resourceBar.Register(Cb.DynamicStatEntry.Resource, cb.StatTable.MaxResource);
        }

        private void OnDisable()
        {
            resourceBar.Unregister();
        }
    }
}
