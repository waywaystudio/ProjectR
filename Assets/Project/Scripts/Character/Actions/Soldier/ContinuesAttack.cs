using Core;
using UnityEngine;

namespace Character.Actions.Soldier
{
    public class ContinuesAttack : SkillComponent
    {
        [SerializeField] private ValueCompletion power;
        
        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayLoop(animationKey);
        }
        
        private void RegisterHitEvent()
        {
            CharacterSystem.Animating.OnHit.Unregister("HoldingHit");
            CharacterSystem.Animating.OnHit.Register("HoldingHit", OnHit.Invoke);
        }
        
        private void OnHoldingAttack()
        {
            if (!CharacterSystem.Colliding.TryGetTakersInSphere(transform.position, range, angle, targetLayer, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                power.Damage(taker);
            });
        }
        
        protected void OnEnable()
        {
            power.Initialize(Provider, ActionCode);
            
            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);

            OnHit.Register("OnHoldingAttack", OnHoldingAttack);
            
            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("StartCooling", StartCooling);
            OnEnded.Register("ReleaseHit", () => CharacterSystem.Animating.OnHit.Unregister("HoldingHit"));
        }
        
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            if (!TryGetComponent(out power))
            {
                power = gameObject.AddComponent<ValueCompletion>();
            }

            power.SetPower(skillData.CompletionValueList[0]);
        }
#endif
    }
}
