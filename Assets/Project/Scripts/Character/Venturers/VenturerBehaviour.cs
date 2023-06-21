using Common;
using Common.Animation;
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
        public override DataIndex DataIndex => data.DataIndex;
        public override CharacterMask CombatClass => data.CharacterMask;
        public override string Name => data.Name;
        
        public bool IsPlayer { get; set; }
        
        /*
         * Behaviours
         */
        public override void Rotate(Vector3 lookTarget)
        {
            Pathfinding.RotateToTarget(lookTarget);

            if (Animating is AnimationModel animationModel)
            {
                animationModel.Flip(transform.forward);
            }
        }

        // TODO. Temp
        public void ForceInitialize()
        {
            StatTable.Clear();
            StatTable.RegisterTable(data.StaticStatTable);
            
            combatStatus.Initialize();
        }


        private void Update()
        {
            if (Animating is AnimationModel animationModel)
            {
                animationModel.Flip(transform.forward);
            }
            
            // Animating.Flip(transform.forward);
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            modChanger ??= GetComponent<VenturerModChanger>();
            data.EditorSetUp();
        }
#endif
    }
}
