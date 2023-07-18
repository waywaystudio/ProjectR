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
            if (vb == null)
            {
                UnregisterGlobe();
                return;
            }
            
            var hpReference = vb.Hp;
            var maxHp = vb.StatTable.MaxHp;

            fillGlobe.RegisterEvent(hpReference, maxHp);
            voidGlobe.RegisterEventReverse(hpReference, maxHp);
        }

        public void OnCommandModeEnter()
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
