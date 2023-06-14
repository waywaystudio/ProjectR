using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Ranger.Skills
{
    public class Trap : SkillComponent
    {
        public override ICombatTaker MainTarget => Cb.Searching.GetSelf();
        
        public override void Execution()
        {
            ExecutionTable.Execute(MainTarget);
        }
        

        protected override void Initialize()
        {
            OnActivated.Register("Jump", Jump);
            OnActivated.Register("Execution", Execution);
            OnCompleted.Register("EndCallback", End);
        }

        private void Jump()
        {
            var direction = Cb.transform.forward * -1f;
            
            Cb.Pathfinding.Jump(direction, 10f, 2, 0.5f);
        }

        private Vector3 GetRangedPosition(Vector3 originalPosition)
        {
            var result = Vector3.Distance(originalPosition, Cb.Position) > range
                ? Cb.Position + (originalPosition - Cb.Position).normalized * range
                : originalPosition;

            return result;
        }
    }
}

/* Annotation */
// // On Player
// if (Cb.IsPlayer)
// {
//     // TODO. 플레이어의 경우 프로젝터를 투영해야 할 듯?
//     // MainManager.Input.ShowProjector(projectorRadius, bound)
//     // MainManager.Input.ReplaceClickActionOnce(()
//     // => ExecutionTable.Execute(GetRangedPosition(MainManager.Input.GetGroundPosition())));
//     if (!MainManager.Input.TryGetGroundPosition(out var groundPosition)) return;
//                 
//     ExecutionTable.Execute(groundPosition);
// }
// // On AI.
// else
// {
//     if (!TryGetTakersInSphere(this, out var takerList)) return;
//                 
//     ExecutionTable.Execute(takerList.FirstOrNull());
// }