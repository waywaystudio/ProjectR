using Character;
using Character.Actions;
using Character.StatusEffect;
using MainGame;
using UnityEngine;

namespace __Refactoring
{
    public class KnightCommonAttack : SkillComponent
    {
        [SerializeField] protected ValueCompletion power;
        [SerializeField] private StatusEffectCompletion armorCrash;

        // public override void Release() { }
        
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
                armorCrash.DeBuff(taker);
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
            
            OnHit.Register("GeneralAttack", OnAttack);

            OnCompleted.Register("EndCallback", OnEnded.Invoke);

            OnEnded.Register("ReleaseHit", () => CharacterSystem.Animating.OnHit.Unregister("SkillHit"));
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var skillData = DB.SkillSheetData(actionCode);

            power.SetPower(skillData.CompletionValueList[0]);
        }
#endif
    }
}
