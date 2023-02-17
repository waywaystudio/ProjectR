using System;
using System.Collections.Generic;
using Character.Graphic;
using Character.Move;
using Core;
using MainGame;
using UnityEngine;

namespace Character.Skill
{
    public class ActionBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform rootTransform;
        [SerializeField] private GlobalCoolDown gcd;
        [SerializeField] private MoveBehaviour move;
        [SerializeField] private AnimationModel model;
        
        [SerializeField] private List<SkillComponent> skillList = new();
        [SerializeField] private SkillComponent firstSkill;
        [SerializeField] private SkillComponent secondSkill;
        [SerializeField] private SkillComponent thirdSkill;
        [SerializeField] private SkillComponent fourthSkill;

        private Camera mainCamera;

        public List<SkillComponent> SkillList => skillList;
        public SkillComponent FirstSkill => firstSkill;
        public SkillComponent SecondSkill => secondSkill;
        public SkillComponent ThirdSkill => thirdSkill;
        public SkillComponent FourthSkill => fourthSkill;
        
        public ConditionTable Conditions { get; } = new();
        public Vector3 RootPosition => rootTransform.position;
        public bool IsSkillEnded
        {
            get
            {
                if (Current.IsNullOrEmpty() || Current.IsEnded) return true;
                
                Debug.LogWarning($"Current Skill is not ended. Skill:{Current.ActionCode.ToString()}");
                return false;
            }
        }
        
        private SkillComponent Current { get; set; }
        private Dictionary<DataIndex, SkillComponent> SkillTable { get; } = new();

        public void Run(Vector3 destination)
        {
            if (!move.IsMovable) return;
            
            InterruptCurrentSkill();
            move.Move(destination, model.Idle);
            model.Run();
        }

        public void Rotate(Vector3 direction)
        {
            move.Rotate(direction);
            model.Flip(move.Direction);
        }

        public void Stop()
        {
            InterruptCurrentSkill();
            move.Stop();
            model.Idle();
        }
        
        public void Dash()
        {
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;

            Dash(mousePosition - RootPosition);
        }

        public void Dash(Vector3 direction)
        {
            InterruptCurrentSkill();
            
            Conditions.Register("OnDashing", () => false);
            move.Rotate(direction);
            model.PlayOnce("jump");
            move.Dash(direction,
                () =>
                {
                    model.Idle();
                    Conditions.Unregister("OnDashing");
                });
        }
        
        public void Teleport()
        {
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;
            
            Teleport(mousePosition - RootPosition);
        }

        public void Teleport(Vector3 direction)
        {
            InterruptCurrentSkill();
            move.Rotate(direction);
            model.Idle();
            move.Teleport(direction);
        }

        public void KnockBack(Vector3 from, Action callback)
        {
            InterruptCurrentSkill();
            move.KnockBack(from, callback);
            model.PlayOnce("fall");
        }
        
        // + Stun, KnockDown, Fear, etc.

        public void ActiveSkill(DataIndex actionCode)
        {
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;
            if (SkillTable.TryGetValue(actionCode, out var skill))
            {
                ActiveSkill(skill, mousePosition);
            }
            
            Debug.LogWarning($"Not Exist Skill!. Input:{actionCode}");
        }
        
        public void ActiveSkill(SkillComponent skill)
        {
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;
            
            ActiveSkill(skill, mousePosition);
        }
        
        public void ActiveSkill(SkillComponent skill, Vector3 targetPosition)
        {
            if (Conditions.HasFalse) return;
            
            Rotate(targetPosition);
            Stop();
            Current = skill;
            gcd.StartCooling();
                
            skill.Active();
        }

        public void ReleaseSkill(SkillComponent skill) => skill.Release();

        public void InterruptCurrentSkill()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Interrupt();
        }

        public bool TryGetMostPrioritySkill(out SkillComponent skill)
        {
            SkillComponent result = null;
            
            foreach (var skillComponent in skillList)
            {
                if (skillComponent.ConditionTable.HasFalse) continue;
                if (result is null || result.Priority < skillComponent.Priority)
                {
                    result = skillComponent;
                }
            }

            skill = result;
            return skill is not null;
        }


        private void Awake()
        {
            gcd ??= GetComponent<GlobalCoolDown>();
            
            skillList.Clear();
            GetComponentsInChildren(false, skillList);
            skillList.ForEach(x =>
            {
                x.OnEnded.Register("BehaviourUnregister", () => Current = null);
                SkillTable.Add(x.ActionCode, x);
            });

            Conditions.Register("GlobalCoolTime", () => !gcd.IsCooling);
            Conditions.Register("CurrentSkillCompleted", () => IsSkillEnded);
        }

        private void Update()
        {
            model.Flip(rootTransform.forward);
        }

        public void SetUp()
        {
            TryGetComponent(out gcd);
            
            skillList.Clear();
            GetComponentsInChildren(false, skillList);
        }


        // TODO. 개별 테스트에서 넘어 옴. 완료 되면 UI로 넘김
        // #region TESTFIELD
        //
        // [SerializeField] private InputAction mouseClick;
        // [SerializeField] private InputAction dashInput;
        // [SerializeField] private InputAction teleportInput;
        //
        // private Vector3 mousedDestination = Vector3.zero;
        //
        // public InputAction shortCutQ;
        // public InputAction shortCutW;
        // public InputAction shortCutE;
        // public InputAction shortCutR;
        //
        // public InputAction shortCutZ;
        //
        // public SkillComponent GeneralSkill;
        // public SkillComponent CastingSkill;
        // public SkillComponent ChargingSkill;
        // public SkillComponent HoldingSkill;
        //
        // public void DoGeneralSkill(InputAction.CallbackContext context) => ActiveSkill(GeneralSkill);
        // public void DoCastingSkill(InputAction.CallbackContext context) => ActiveSkill(CastingSkill);
        // public void DoChargingSkill(InputAction.CallbackContext context) => ActiveSkill(ChargingSkill);
        // public void DoHoldingSkill(InputAction.CallbackContext context) => ActiveSkill(HoldingSkill);
        // public void InterruptSkill(InputAction.CallbackContext context) => InterruptCurrentSkill();
        //
        // public void ReleaseCharging(InputAction.CallbackContext context)
        // {
        //     if (!ChargingSkill.OnProgress) return;
        //     
        //     ChargingSkill.OnCompleted.Invoke();
        // }
        //
        // public void ReleaseHolding(InputAction.CallbackContext context)
        // {
        //     if (!HoldingSkill.OnProgress) return;
        //     
        //     HoldingSkill.OnEnded.Invoke();
        // }
        //
        // public void RegisterInputAction()
        // {
        //     mouseClick.Enable();
        //     dashInput.Enable();
        //     teleportInput.Enable();
        //     
        //     shortCutQ.Enable();
        //     shortCutW.Enable();
        //     shortCutE.Enable();
        //     shortCutR.Enable();
        //     shortCutZ.Enable();
        //     
        //     mainCamera              =  Camera.main;
        //     mouseClick.performed    += OnMove;
        //     dashInput.performed     += OnDash;
        //     teleportInput.performed += OnTeleport;
        //     shortCutQ.performed     += DoGeneralSkill;
        //     shortCutW.performed     += DoCastingSkill;
        //     shortCutZ.performed     += InterruptSkill;
        //     shortCutE.started       += DoChargingSkill;
        //     shortCutR.started       += DoHoldingSkill;
        //     shortCutE.canceled      += ReleaseCharging;
        //     shortCutR.canceled      += ReleaseHolding;
        // }
        //
        // public void UnregisterInputAction()
        // {
        //     mouseClick.performed    -= OnMove;
        //     dashInput.performed     -= OnDash;
        //     teleportInput.performed -= OnTeleport;
        //     shortCutQ.performed     -= DoGeneralSkill;
        //     shortCutW.performed     -= DoCastingSkill;
        //     shortCutZ.performed     -= InterruptSkill;
        //     shortCutE.started       -= DoChargingSkill;
        //     shortCutR.started       -= DoHoldingSkill;
        //     shortCutE.canceled      -= ReleaseCharging;
        //     shortCutR.canceled      -= ReleaseHolding;
        //     
        //     mouseClick.Disable();
        //     dashInput.Disable();
        //     teleportInput.Disable();
        //     shortCutQ.Disable();
        //     shortCutW.Disable();
        //     shortCutE.Disable();
        //     shortCutR.Disable();
        //     shortCutZ.Disable();
        // }
        //
        // private void OnDisable() => UnregisterInputAction();
        //
        // public void OnMove(InputAction.CallbackContext context)
        // {
        //     var ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        //
        //     if (Physics.Raycast(ray: ray, hitInfo: out var hit) && hit.collider)
        //     {
        //         mousedDestination.x = hit.point.x;
        //         mousedDestination.z = hit.point.z;
        //
        //         Run(mousedDestination);
        //     }
        // }
        //
        // public void OnDash(InputAction.CallbackContext context) => Dash();
        // public void OnTeleport(InputAction.CallbackContext context) => Teleport();
        // #endregion
    }
}
