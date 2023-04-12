using Common.Characters;
using UnityEngine;

namespace Character.Adventurers
{
    public class Adventurer : CharacterBehaviour
    {
        [SerializeField] protected AdventurerModChanger modChanger;
        
        public bool IsPlayer { get; set; }
        
        public override void Initialize()
        {
            if (IsInitialized) return;
            
            stats.Initialize();
            equipment.Initialize();

            IsInitialized = true;
        }


        private void Update()
        {
            Animating.Flip(transform.forward);
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            modChanger ??= GetComponent<AdventurerModChanger>();
        }
#endif
    }
}
