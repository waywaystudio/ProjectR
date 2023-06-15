using System;
using Common.Characters;
using UnityEngine;

namespace Common.Skills
{
    [RequireComponent(typeof(SkillComponent))]
    public class SkillAnimationHit : MonoBehaviour
    {
#if UNITY_EDITOR
        private string scriptDescription = string.Empty;
#endif
        
        private CharacterBehaviour cb;
        
        protected CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        private void RegisterHitEvent(Action mainAction) => Cb.Animating.OnHit.Register("SkillHit", mainAction);
        private void UnregisterHitEvent() => Cb.Animating.OnHit.Unregister("SkillHit");

        private void Awake()
        {
            if (!TryGetComponent(out SkillComponent skill)) return;

            skill.Sequencer.ActiveAction.Add("RegisterHitEvent", () => RegisterHitEvent(skill.Execution));
            skill.Sequencer.EndAction.Add("ReleaseHit", UnregisterHitEvent);
        }
    }
}
