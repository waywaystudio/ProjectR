using Common.Characters;
using UnityEngine;

namespace Character.Adventurers
{
    public class Adventurer : CharacterBehaviour
    {
        [SerializeField] protected AdventurerModChanger modChanger;
        
        public bool IsPlayer { get; set; }

        // TODO. Temp
        public override void ForceInitialize()
        {
            combatStatus.Initialize();
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
