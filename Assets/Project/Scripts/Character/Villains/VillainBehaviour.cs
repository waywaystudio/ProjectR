using Common;
using Common.Animation;
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


        /*
         * Phase
         */
        public PhaseSequencer CurrentPhase => phaseBehaviours.CurrentPhase;
        public void CheckPhaseBehaviour() => phaseBehaviours.CheckPhaseBehaviour();

        public void ForceInitialize(DifficultyType difficulty, int level)
        {
            StatTable.Clear();
            StatTable.RegisterTable(data.StaticStatTable);
            StatTable.Add(difficultyTable.GetDifficultySpec(difficulty, level));
            
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

            if (Finder.TryGetObject($"{name}Data", out data))
            {
                data.EditorSetUp();
            }

            difficultyTable ??= GetComponentInChildren<VillainDifficultyTable>();
            phaseBehaviours ??= GetComponentInChildren<PhaseBehaviours>();
            SkillBehaviour.EditorSetUp();
        }
#endif
    }
}

