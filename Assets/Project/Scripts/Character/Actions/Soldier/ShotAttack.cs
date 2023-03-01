using Core;
using UnityEngine;

namespace Character.Actions.Soldier
{
    public class ShotAttack : SkillComponent
    {
        [SerializeField] private ValueCompletion power;
        
        public override void Release() { }
        
        protected override void PlayAnimation()
        {
            CharacterSystem.Animating.PlayOnce(animationKey, progressTime, OnCompleted.Invoke);
        }
        
        protected void OnAttack()
        {
            // TODO. Damage를 주기보다, Projectile을 발사해야 할 수 있다.
            // var providerTransform = Provider.Object.transform;
            // var tempTarget        = providerTransform.position + providerTransform.forward; 
            //     
            // if (!CharacterSystem.Colliding.TryGetTakerByRayCast(providerTransform.position, tempTarget,
            //         range, 10, targetLayer, out var taker))
            // {
            //     Debug.Log("No Taker");
            //     return;
            // }
            
            if (!CharacterSystem.Colliding.TryGetTakersInSphere(this, out var takerList)) return;
            
            takerList.ForEach(power.Damage);
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
