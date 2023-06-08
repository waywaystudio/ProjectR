using Common;
using Common.Characters;
using UnityEngine;

namespace Character.Villains
{
    public class VillainBehaviour : CharacterBehaviour
    {
        [SerializeField] private VillainData data;
        [SerializeField] private VillainPhaseTable phaseTable;
        
        
        /*
         * Common Attribute
         */ 
        public override DataIndex DataIndex => data.DataIndex;
        public override CharacterMask CombatClass => data.CharacterMask;
        public override string Name => data.Name;

        public BossPhase CurrentPhase { get; set; }
        
        // TODO. Temp
        public override void ForceInitialize()
        {
            combatStatus.Initialize();
        }


        protected void Start()
        {
            CurrentPhase = phaseTable.GetStartPhase();
            CurrentPhase.Activate();
        }

        private void Update() { Animating.Flip(transform.forward); }
        
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            phaseTable ??= GetComponent<VillainPhaseTable>();
        }
#endif
    }
}

