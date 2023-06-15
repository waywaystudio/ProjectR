using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class RunBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private Sequencer<Vector3> sequencer;

        public CharacterActionMask BehaviourMask => CharacterActionMask.Run;
        public Sequencer<Vector3> Sequencer => sequencer;

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private bool CanOverrideToCurrent 
            => (CharacterActionMask.RunIgnoreMask | Cb.BehaviourMask) == CharacterActionMask.RunIgnoreMask;
        

        public void Run(Vector3 destination)
        {
            if (!sequencer.IsAbleToActive) return;
            
            sequencer.Active(destination);
        }

        public void Cancel() => sequencer.Cancel();
        

        private void OnEnable()
        {
            sequencer.Condition.Add("AbleToBehaviourOverride", () => CanOverrideToCurrent);
            sequencer.Condition.Add("CanMove", () => Cb.Pathfinding.CanMove);
            
            sequencer.ActiveParamAction.Add("CommonRunAction", destination => Cb.Pathfinding.Move(destination, sequencer.Complete));
            sequencer.ActiveAction.Add("CommonRunAction", () =>
            {
                if (cb.CurrentBehaviour is not null && cb.BehaviourMask != BehaviourMask)
                    cb.CurrentBehaviour.Cancel();

                cb.CurrentBehaviour = this;
                Cb.Animating.Run();
            });
            
            sequencer.EndAction.Add("Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }
    }
}
