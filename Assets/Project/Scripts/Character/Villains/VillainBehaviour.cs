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
        // public override IAnimator Animating => animationTable;
        


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
            if (Animating is AnimationModel animationModel)
            {
                animationModel.Flip(transform.forward);
            }
            else
            {
                if (searching.AdventurerList.Count > 0 &&
                    searching.AdventurerList[0].DynamicStatEntry.Alive.Value &&
                    BehaviourMask is ActionMask.Stop or ActionMask.Skill)
                {
                    Debug.Log("Rotate!!!");
                    Rotate(searching.AdventurerList[0].gameObject.transform.position);
                }
            }
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

