using System;
using Common.Characters;
using UnityEngine;

namespace Common.Skills
{
    [RequireComponent(typeof(SkillComponent))]
    public class DirectHitEvent : MonoBehaviour
    {
        // [SerializeField] private List<CollidingCompletion> completionList;

        private CharacterBehaviour cb;
        
        protected CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        // private void Completion(ICombatTaker taker)
        // {
        //     completionList.ForEach(completion => completion.Completion(taker));
        // }

        private void RegisterHitEvent(Action mainAction) => Cb.Animating.OnHit.Register("SkillHit", mainAction.Invoke);
        private void UnregisterHitEvent() => Cb.Animating.OnHit.Unregister("SkillHit");

        private void Awake()
        {
            if (!TryGetComponent(out SkillComponent skill)) return;

            skill.OnActivated.Register("RegisterHitEvent", () => RegisterHitEvent(skill.MainAttack));
            skill.OnEnded.Register("ReleaseHit", UnregisterHitEvent);

            // skill.OnCompletion.Register("Completion", Completion);
            // completionList.ForEach(completion => completion.Initialize(Cb));
        }
    }
}
