using UnityEngine;

namespace Character.Actions.Assassin
{
    public class PowerBeat : SkillComponent
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
            });
        }
        
        private void RegisterHitEvent()
        {
            CharacterSystem.Animating.OnHit.Register("SkillHit", OnHit.Invoke);
        }
        
        protected void OnEnable()
        {
            power.Initialize(Provider, ActionCode);
            // armorCrash.Initialize(Provider);

            OnActivated.Register("RegisterHitEvent", RegisterHitEvent);
            OnActivated.Register("StartCooling", StartCooling);
            
            OnHit.Register("PowerBeat", OnAttack);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => CharacterSystem.Animating.OnHit.Unregister("SkillHit"));
        }
        
        
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = MainGame.MainData.SkillSheetData(actionCode);

            if (!TryGetComponent(out power)) power = gameObject.AddComponent<ValueCompletion>();

            power.SetPower(skillData.CompletionValueList[0]);
        }
#endif
    }
}
