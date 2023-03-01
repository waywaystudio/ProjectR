using Character.Graphic;
using Character.Systems;
using Core;
using UnityEngine;

namespace Character.Actions
{
    public class CommonMove : MonoBehaviour
    {
        private PathfindingSystem pathfinding;
        private AnimationModel animating;
        private Transform root;
        
        public void Run(Vector3 destination)
        {
            if (!pathfinding.CanMove) return;
            
            pathfinding.Move(destination, animating.Idle);
            animating.Run();
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
            var characterSystem = GetComponentInParent<ICharacterSystem>();
            var provider        = GetComponentInParent<ICombatProvider>();

            animating   = characterSystem.Animating;
            pathfinding = characterSystem.Pathfinding;
            root        = provider.Object.transform;
        }

        private void Update()
        {
            animating.Flip(root.forward);
        }
    }
}
