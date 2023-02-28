using Character.Graphic;
using Character.Systems;
using UnityEngine;

namespace Character.Actions
{
    public class CommonMove : MonoBehaviour
    {
        private PathfindingSystem pathfinding;
        private AnimationModel animating;
        
        public void Run(Vector3 destination)
        {
            if (!pathfinding.IsMovable) return;
            
            pathfinding.Move(destination, animating.Idle);
            animating.Run();
        }

        public void Rotate(Vector3 lookTargetPosition)
        {
            pathfinding.RotateTo(lookTargetPosition);
            animating.Flip(pathfinding.Direction);
        }
        
        public void Stop()
        {
            pathfinding.Stop();
            animating.Idle();
        }
        
        // + Stun, Fear, etc.
        // public void KnockBack(Vector3 from, Action callback)
        // {
        //     pathfinding.KnockBack(from, callback);
        //     Animating.PlayOnce("hit");
        // }
        

        private void Awake()
        {
            var characterSystem =   GetComponentInParent<ICharacterSystem>();

            animating   = characterSystem.Animating;
            pathfinding = characterSystem.Pathfinding;
        }
    }
}
