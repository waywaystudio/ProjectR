using Character.Graphic;
using Character.Systems;
using UnityEngine;

namespace Character.Actions
{
    public class CommonAction : MonoBehaviour
    {
        private PathfindingSystem pathfinding;
        private AnimationModel animating;
        
        public void Run(Vector3 destination)
        {
            if (!pathfinding.CanMove) return;
            
            pathfinding.Move(destination, animating.Idle);
            animating.Run();
        }

        public void Rotate(Vector3 lookTargetPosition)
        {
            pathfinding.RigidRotate(lookTargetPosition);
            animating.Flip(pathfinding.Direction);
        }
        
        public void Stop()
        {
            pathfinding.Stop();
            animating.Idle();
        }

        public void Dead()
        {
            pathfinding.Quit();
            animating.Dead();
        }
        
        public void Dash(Vector3 direction, float distance)
        {
            pathfinding.Dash(direction, distance, animating.Idle);
            animating.Flip(pathfinding.Direction);
        }
        
        public void Teleport(Vector3 direction, float distance)
        {
            pathfinding.Teleport(direction, distance, animating.Idle);
            animating.Flip(pathfinding.Direction);
        }
        
        // + Stun, Fear, etc.
        // public void KnockBack(Vector3 from, Action callback)
        // {
        //     pathfinding.KnockBack(from, callback);
        //     Animating.PlayOnce("hit");
        // }
        

        private void Awake()
        {
            var characterSystem = GetComponentInParent<ICharacterSystem>();

            animating   = characterSystem.Animating;
            pathfinding = characterSystem.Pathfinding;
        }
    }
}
