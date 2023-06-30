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
            var resourceReference = vb.DynamicStatEntry.Resource;
            var maxResource = vb.StatTable.MaxResource;

            fillGlobe.Register(resourceReference, maxResource);
            voidGlobe.RegisterReverse(resourceReference, maxResource);
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
