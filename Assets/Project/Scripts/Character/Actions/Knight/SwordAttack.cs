using MainGame;
using UnityEngine;

namespace Character.Actions.Knight
{
    public class SwordAttack : SkillComponent
    {
        [SerializeField] private ValueCompletion power;
        
        
        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }
        
        protected void OnAttack()
        {
            if (!CharacterSystem.Colliding.TryGetTakersInSphere(this, out var takerList)) return;

            takerList.ForEach(taker =>
            {
                power.Damage(taker);
                // armorCrash.Effect(taker);
            });
        }
        
        private void RegisterHitEvent()
        {
            CharacterSystem.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        }
        
        protected void OnEnable()
        {
            power.Initialize(Provider, ActionCode);

            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            
            OnHit.Register("SwordAttack", OnAttack);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => CharacterSystem.Animating.OnHit.Unregister("SkillHit"));
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = Database.SkillSheetData(actionCode);

            if (!TryGetComponent(out power))
            {
                power = gameObject.AddComponent<ValueCompletion>();
            }

            power.SetPower(skillData.CompletionValueList[0]);
        }
#endif
    }
}
