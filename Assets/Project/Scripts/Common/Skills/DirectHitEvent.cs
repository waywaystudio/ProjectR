using System.Collections.Generic;
using Common.Characters;
using Common.Completion;
using UnityEngine;

namespace Common.Skills
{
    [RequireComponent(typeof(SkillSequence))]
    public class DirectHitEvent : MonoBehaviour, IEditable
    {
        [SerializeField] private SkillSequence skillSequence;
        [SerializeField] private List<CollidingCompletion> completionList;

        private CharacterBehaviour Cb => skillSequence.Cb;

        public ActionTable OnActivated { get; } = new();
        public ActionTable OnHit { get; } = new();
        public ActionTable OnEnded { get; } = new();
        
        
        public void Initialize()
        {
            completionList.ForEach(completion => completion.Initialize(Cb));
            
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            OnHit.Register("Bash", skillSequence.OnAttack);
            OnEnded.Register("ReleaseHit", UnregisterHitEvent);
            
            CombineTo(skillSequence);
        }

        public void Completion(ICombatTaker taker)
        {
            completionList.ForEach(completion => completion.Completion(taker));
        }

        public void Dispose()
        {
            OnActivated.Clear();
            OnHit.Clear();
            OnEnded.Clear();
        }


        private void CombineTo(ISequence sequence)
        {
            sequence.OnActivated.Register("SkillAnimationHitEvent", OnActivated);
            sequence.OnEnded.Register("SkillAnimationHitEvent", OnEnded);
        }

        private void RegisterHitEvent() => Cb.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        private void UnregisterHitEvent() => Cb.Animating.OnHit.Unregister("SkillHit");
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            TryGetComponent(out skillSequence);
            GetComponents(completionList);
        }
#endif
    }
}
