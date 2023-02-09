using UnityEngine;

namespace Character.Combat.Skill.Knight
{
    public class CommonAttack : SkillComponent
    {
        public override void UseSkill()
        {
            if (!IsReady.IsAllTrue()) return;

            model.OnHit.Unregister("SkillHit");
            model.OnHit.Register("SkillHit", OnHit.Invoke);
            
            OnActivated.Invoke();
        }
        
        protected override void PlayAnimation()
        {
            model.Play(animationKey, 0, false, progressTime, OnCompleted.Invoke);
        }

        private void OnCommonAttack()
        {
            if (!targeting.TryGetTargetList(transform.position, out var takerList)) return;
            
            takerList.ForEach(taker =>
            {
                taker.TakeDamage(this);
                Debug.Log($"{taker.Name} Hit by {ActionCode.ToString()}");
            });
        }
        
        protected void OnEnable()
        {
            OnActivated.Register("PlayAnimation", PlayAnimation);
            OnActivated.Register("UpdatePowerValue", UpdatePowerValue);
            OnActivated.Register("Complete", () => IsCompleted = false);
            
            OnHit.Register("CommonAttack", OnCommonAttack);

            OnCompleted.Register("IdleAnimation", model.Idle);
            OnCompleted.Register("Complete", () => IsCompleted = true);
        }

        // #region TEMP ShortCut Test
        // [SerializeField] private InputAction shortCut;
        //
        // // TODO. Test 완료되면 UI로 이동
        // protected override void OnEnable()
        // {
        //     base.OnEnable();
        //     
        //     shortCut.Enable();
        //     shortCut.performed += DoSkill;
        // }
        //
        // protected void OnDisable()
        // {
        //     shortCut.performed -= DoSkill;
        //     shortCut.Disable();
        // }
        //
        // public void DoSkill(InputAction.CallbackContext context)
        // {
        //     UseSkill();
        // }
        // #endregion
    }
}
