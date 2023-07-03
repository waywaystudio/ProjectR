using Common;
using Common.Characters;
using UnityEngine;

namespace Character.Venturers
{
    public class VenturerBehaviour : CharacterBehaviour
    {
        [SerializeField] private VenturerData data;
        [SerializeField] protected VenturerModChanger modChanger;

        /*
         * Common Attribute
         */
        public VenturerType Type => data.VenturerType;
        public override DataIndex DataIndex => data.DataIndex;
        public override CharacterMask CombatClass => data.CharacterMask;
        public override string Name => data.Name;
        public override CharacterData Data => data;
        
        public bool IsPlayer { get; set; }

        private void Update()
        {
            Animating.Flip(transform.forward);
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            modChanger = GetComponent<VenturerModChanger>();
        }
#endif
    }
}
