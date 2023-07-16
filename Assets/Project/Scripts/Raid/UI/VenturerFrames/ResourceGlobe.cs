using Character.Venturers;
using Common.UI;
using UnityEngine;

namespace Raid.UI.VenturerFrames
{
    public class ResourceGlobe : MonoBehaviour, IEditable
    {
        [SerializeField] private ImageFiller fillGlobe;
        [SerializeField] private ImageFiller voidGlobe;


        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            if (vb == null)
            {
                UnregisterGlobe();
                return;
            }
            
            var resourceReference = vb.Resource;
            var maxResource = vb.StatTable.MaxResource;

            fillGlobe.RegisterEvent(resourceReference, maxResource);
            voidGlobe.RegisterEventReverse(resourceReference, maxResource);
        }
        
        public void OnCommandMode()
        {
            UnregisterGlobe();
        }
        
        
        private void UnregisterGlobe()
        {
            fillGlobe.UnregisterFloatEvent();
            voidGlobe.UnregisterFloatEvent();
        }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            fillGlobe = transform.Find("Fill").GetComponent<ImageFiller>();
            voidGlobe = transform.Find("Void").GetComponent<ImageFiller>();
        }
#endif
    }
}
