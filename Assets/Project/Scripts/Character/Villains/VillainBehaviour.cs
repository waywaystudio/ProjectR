using Common;
using Common.Characters;
using UnityEngine;

namespace Character.Villains
{
    public class VillainBehaviour : CharacterBehaviour
    {
        [SerializeField] private VillainData data;
        [SerializeField] private VillainDifficultyTable difficultyTable;
        [SerializeField] private PhaseBehaviours phaseBehaviours;


        /*
         * Common Attribute
         */ 
        public override DataIndex DataIndex => data.DataIndex;
        public override CharacterMask CombatClass => data.CharacterMask;
        public override string Name => data.Name;
        public override CharacterData Data => data;


        /*
         * Phase
         */
        public PhaseSequencer CurrentPhase => phaseBehaviours.CurrentPhase;
        public void CheckPhaseBehaviour() => phaseBehaviours.CheckPhaseBehaviour();

        public void ForceInitialize(DifficultyType difficulty, int level)
        {
            StatTable.Add(difficultyTable.GetDifficultySpec(difficulty, level));
        }


        private void Update()
        {
            Animating.Flip(transform.forward);
        }
        
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            if (Finder.TryGetObject($"{name}Data", out data))
            {
                data.EditorSetUp();
            }

            difficultyTable ??= GetComponentInChildren<VillainDifficultyTable>();
            phaseBehaviours ??= GetComponentInChildren<PhaseBehaviours>();
            SkillTable.EditorSetUp();
        }
#endif
    }
}

