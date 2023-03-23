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
            statEntry.Initialize();
            modChanger.Initialize(this);
        }

        private void Update()
        {
            // Debug.Log(BehaviourMask.ToString());
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
