using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Combat.Skill
{
    public class KnightCommonAttack : SkillComponent
    {
        private void OnKnightCommonAttackHit()
        {
            if (!moduleController.CollidingModule.TryGetTargetList(out var takerList)) return;
            
            takerList.ForEach(x =>
            {
                x.TakeDamage(moduleController.DamageModule);
                Debug.Log($"{x.Name} Hit!");
            });
        }
        
        protected override void Init()
        {
            OnHit.Register("KnightCommonAttack", OnKnightCommonAttackHit);
        }
        
        #region TEMP ShortCut Test
        [SerializeField] private InputAction shortCut;
        
        // TODO. Test 완료되면 UI로 이동
        protected override void OnEnable()
        {
            base.OnEnable();
            
            shortCut.Enable();
            shortCut.performed += DoSkill;
        }
        
        protected void OnDisable()
        {
            shortCut.performed -= DoSkill;
            shortCut.Disable();
        }
        
        public void DoSkill(InputAction.CallbackContext context)
        {
            UseSkill();
        }
        #endregion
    }
}
