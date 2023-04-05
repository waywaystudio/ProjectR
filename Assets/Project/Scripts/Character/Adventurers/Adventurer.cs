using Adventurers;
using Common.Characters;
using UnityEngine;

namespace Character.Adventurers
{
    public class Adventurer : CharacterBehaviour
    {
        [SerializeField] protected AdventurerModChanger modChanger;


        protected void Awake()
        {
            stats.Initialize();
            modChanger.Initialize(this);
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
