using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Combat.Skill
{
    public class KnightChargingAttack : SkillComponent
    {
        // #region TEMP ShortCut Test
        // [SerializeField] private InputAction shortCut;
        //
        // // TODO. Charging Input System. started : press, canceled : released
        // protected override void OnEnable()
        // {
        //     base.OnEnable();
        //     
        //     shortCut.Enable();
        //     shortCut.started  += DoSkill;
        //     shortCut.canceled += CancelSkill;
        // }
        //
        // protected void OnDisable()
        // {
        //     shortCut.started  -= DoSkill;
        //     shortCut.canceled -= CancelSkill;
        //     shortCut.Disable();
        // }
        //
        // public void DoSkill(InputAction.CallbackContext context)
        // {
        //     UseSkill();
        // }
        //
        // public void CancelSkill(InputAction.CallbackContext context)
        // {
        //     OnCompleted.Invoke();
        // }
        // #endregion
    }
}
