using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Skill
{
    public class SkillBehaviour : MonoBehaviour
    {
        [SerializeField] private GlobalCoolDown gcd;
        [SerializeField] private List<SkillComponent> skillList = new();

        public BoolTable Conditions { get; } = new();
        public SkillComponent Current { get; private set; }
        

        public void Active(SkillComponent skill)
        {
            if (Conditions.HasFalse) return;

            Current = skill;
            gcd.StartCooling();
            skill.Active();
        }

        public void InterruptCurrentSkill()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Interrupt();
        }


        private void Release()
        {
            Current = null;
        }

        private bool IsSkillEnded
        {
            get
            {
                if (Current.IsNullOrEmpty() || Current.IsEnded) return true;
                
                Debug.LogWarning($"Current Skill is not ended. Skill:{Current.ActionCode.ToString()}");
                return false;
            }
        }
        
        private void Awake()
        {
            gcd       ??= GetComponent<GlobalCoolDown>();
            skillList ??= GetComponentsInChildren<SkillComponent>().ToList();
            skillList.ForEach(x => x.OnCompleted.Register("Release", Release));
            
            Conditions.Register("GlobalCoolTime", () => !gcd.IsCooling);
            Conditions.Register("CurrentSkillCompleted", () => IsSkillEnded);
        }

        public void SetUp()
        {
            TryGetComponent(out gcd);
            
            skillList.Clear();
            GetComponentsInChildren(false, skillList);
        }


        // TODO. 개별 테스트에서 넘어 옴. 완료 되면 UI로 넘김
        #region TESTFIELD

        public InputAction shortCutQ;
        public InputAction shortCutW;
        public InputAction shortCutE;
        public InputAction shortCutR;
        
        public InputAction shortCutZ;
        
        public SkillComponent GeneralSkill;
        public SkillComponent CastingSkill;
        public SkillComponent ChargingSkill;
        public SkillComponent HoldingSkill;

        public void DoGeneralSkill(InputAction.CallbackContext context) => Active(GeneralSkill);
        public void DoCastingSkill(InputAction.CallbackContext context) => Active(CastingSkill);
        public void DoChargingSkill(InputAction.CallbackContext context) => Active(ChargingSkill);
        public void DoHoldingSkill(InputAction.CallbackContext context) => Active(HoldingSkill);
        public void InterruptSkill(InputAction.CallbackContext context) => InterruptCurrentSkill();
        
        public void ReleaseCharging(InputAction.CallbackContext context)
        {
            if (!ChargingSkill.OnProgress) return;
            
            ChargingSkill.OnCompleted.Invoke();
        }
        
        public void ReleaseHolding(InputAction.CallbackContext context)
        {
            if (!HoldingSkill.OnProgress) return;
            
            HoldingSkill.OnEnded.Invoke();
        }

        private void OnEnable()
        {
            shortCutQ.Enable();
            shortCutW.Enable();
            shortCutE.Enable();
            shortCutR.Enable();
            shortCutZ.Enable();
            
            shortCutQ.performed += DoGeneralSkill;
            shortCutW.performed += DoCastingSkill;
            shortCutZ.performed += InterruptSkill;
            shortCutE.started   += DoChargingSkill;
            shortCutR.started   += DoHoldingSkill;
            shortCutE.canceled  += ReleaseCharging;
            shortCutR.canceled  += ReleaseHolding;
        }

        private void OnDisable()
        {
            shortCutQ.performed -= DoGeneralSkill;
            shortCutW.performed -= DoCastingSkill;
            shortCutZ.performed -= InterruptSkill;
            shortCutE.started   -= DoChargingSkill;
            shortCutR.started   -= DoHoldingSkill;
            shortCutE.canceled  -= ReleaseCharging;
            shortCutR.canceled  -= ReleaseHolding;
            
            shortCutQ.Disable();
            shortCutW.Disable();
            shortCutE.Disable();
            shortCutR.Disable();
            shortCutZ.Disable();
        }

        #endregion
    }
}
