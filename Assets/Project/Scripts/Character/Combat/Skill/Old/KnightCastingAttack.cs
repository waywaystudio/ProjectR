using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Combat.Skill
{
    public class KnightCastingAttack : SkillComponent
    {
        // private void OnKnightCastingAttackCompleted()
        // {
        //     if (!moduleController.CollidingModule.TryGetTargetList(out var takerList)) return;
        //     
        //     takerList.ForEach(x =>
        //     {
        //         x.TakeDamage(moduleController.DamageModule);
        //         Debug.Log($"{x.Name} Hit by {ActionCode.ToString()}");
        //     });
        // }
        //
        // protected override void Init()
        // {
        //     OnCompleted.Register("KnightCommonAttack", OnKnightCastingAttackCompleted);
        // }
        //
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
