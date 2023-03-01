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
            if (!CharacterSystem.Colliding.TryGetTakersInSphere(this, out var takerList)) return;
            
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
