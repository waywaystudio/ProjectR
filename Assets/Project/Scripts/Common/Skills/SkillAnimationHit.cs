using System;
using Common.Characters;
using UnityEngine;

namespace Common.Skills
{
    [RequireComponent(typeof(SkillComponent))]
    public class SkillAnimationHit : MonoBehaviour
    {
#if UNITY_EDITOR
        private string scriptDescription;
#endif
        
        private CharacterBehaviour cb;
        
        protected CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        private void RegisterHitEvent(Action mainAction) => Cb.Animating.OnHit.Register("SkillHit", mainAction.Invoke);
        private void UnregisterHitEvent() => Cb.Animating.OnHit.Unregister("SkillHit");

        private void Awake()
        {
            if (!TryGetComponent(out SkillComponent skill)) return;

            skill.OnActivated.Register("RegisterHitEvent", () => RegisterHitEvent(skill.MainAttack));
            skill.OnEnded.Register("ReleaseHit", UnregisterHitEvent);
        }
    }
}
