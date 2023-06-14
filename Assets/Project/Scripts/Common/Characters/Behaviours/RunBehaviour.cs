using Cysharp.Threading.Tasks;
using Sequences;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class RunBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private Sequencer<Vector3> sequencer;
        
        private CharacterBehaviour cb;
        
        public CharacterActionMask BehaviourMask => CharacterActionMask.Run;

        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private bool CanOverrideToCurrent 
            => (CharacterActionMask.RunIgnoreMask | Cb.BehaviourMask) == CharacterActionMask.RunIgnoreMask;
        

        public void Run(Vector3 destination)
        {
            if (!sequencer.IsAbleToActive) return;
            
            sequencer.Activate(destination);
        }

        public void Cancel() 
            => sequencer.Cancel();

        public void RunRegisterActive()
        {
            if (cb.CurrentBehaviour is not null && cb.BehaviourMask != BehaviourMask)
            {
                cb.CurrentBehaviour.Cancel();
            }

            cb.CurrentBehaviour = this;
        }

        public void RunAnimationActive()
        {
            Cb.Animating.Run();
        }

        public void RunCancel() => Cb.Stop();
        public void RunComplete() => Cb.Stop();
        
        
        private async UniTask AwaitMove(Vector3 destination)
        {
            await Cb.Pathfinding.Move(destination);
        }

        private void Awake()
        {
            sequencer.Condition.Add("CanMove", () => Cb.Pathfinding.CanMove);
            sequencer.Condition.Add("AbleToBehaviourOverride", () => CanOverrideToCurrent);
            sequencer.ActiveParamSection.AddAwait("RunMoveActive", AwaitMove);
        }

        private void OnDestroy()
        {
            sequencer.Clear();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            sequencer.AssignPersistantEvents();
        }
#endif
    }
}
