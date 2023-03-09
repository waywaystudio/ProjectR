using UnityEngine;

namespace Character.Actions.Soldier
{
    public class AimShot : SkillComponent
    {
        [SerializeField] private ValueCompletion power;
        
        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey);
        }
        
        private void OnAimShotAttack()
        {
            // TODO. 현재 Test상 HitScan 방식이어서 이렇고, Projectile로 바뀌면 교체해야 함.
            var providerTransform = Provider.Object.transform;
            
            if (!CharacterSystem.Colliding.TryGetTakersByRaycast(
                    providerTransform.position,
                    providerTransform.forward,
                    range,
                    16,
                    targetLayer,
                    out var takerList)) return;

            takerList.ForEach(power.Damage);
        }
        
        private void PlayEndChargingAnimation()
        {
            CharacterSystem.Animating.PlayOnce("heavyAttack", 0f, OnEnded.Invoke);
        }
        
        protected void OnEnable()
        {
            power.Initialize(Provider, ActionCode);
            
            OnCompleted.Register("StartCooling", StartCooling);
            OnCompleted.Register("OnAimShotAttack", OnAimShotAttack);
            OnCompleted.Register("PlayEndChargingAnimation", PlayEndChargingAnimation);
            OnCompleted.Register("StopProcess", StopProgression);
            OnCompleted.Register("ProgressEnd", () => IsProgress = false);
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
