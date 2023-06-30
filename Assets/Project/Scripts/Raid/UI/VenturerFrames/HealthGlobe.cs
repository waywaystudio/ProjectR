using Character.Venturers;
using Common.UI;
using UnityEngine;

namespace Raid.UI.VenturerFrames
{
    public class HealthGlobe : MonoBehaviour, IEditable
    {
        [SerializeField] private ImageFiller fillGlobe;
        [SerializeField] private ImageFiller voidGlobe;


        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            var hpReference = vb.DynamicStatEntry.Hp;
            var maxHp = vb.StatTable.MaxHp;

            fillGlobe.Register(hpReference, maxHp);
            voidGlobe.RegisterReverse(hpReference, maxHp);
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
