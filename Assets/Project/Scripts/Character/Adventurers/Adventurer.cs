using Common.Characters;
using UnityEngine;

namespace Adventurers
{
    public class Adventurer : CharacterBehaviour
    {
        [SerializeField] protected AdventurerModChanger modChanger;


        protected void Awake()
        {
            statEntry.Initialize();
            modChanger.Initialize(this);
        }

        private void Update() { Animating.Flip(transform.forward); }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            modChanger ??= GetComponent<AdventurerModChanger>();
        }
#endif
    }
}
