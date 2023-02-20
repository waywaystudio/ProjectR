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
        [SerializeField] private SkillComponent dashSkill;
        [SerializeField] private SkillComponent firstSkill;
        [SerializeField] private SkillComponent secondSkill;
        [SerializeField] private SkillComponent thirdSkill;
        [SerializeField] private SkillComponent fourthSkill;

        private Camera mainCamera;

        public List<SkillComponent> SkillList => skillList;
        public SkillComponent DashSkill => dashSkill;
        public SkillComponent FirstSkill => firstSkill;
        public SkillComponent SecondSkill => secondSkill;
        public SkillComponent ThirdSkill => thirdSkill;
        public SkillComponent FourthSkill => fourthSkill;
        
        public ConditionTable Conditions { get; } = new();
        public ActionTable<SkillComponent> OnActivatedSkill { get; } = new();
        public ActionTable<SkillComponent> OnInterruptedSkill { get; } = new();
        public ActionTable<SkillComponent> OnReleasedSkill { get; } = new();
        public SkillComponent Current { get; set; }
        public Vector3 RootPosition => rootTransform.position;
        public bool IsSkillEnded
        {
            get
            {
                if (Current.IsNullOrEmpty() || !Current.OnProgress) return true;
                
                // Debug.LogWarning($"Current Skill is not ended. Skill:{Current.ActionCode.ToString()}");
                return false;
            }
        }
        
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
            
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;

            move.Rotate(mousePosition - RootPosition);
            Stop();
            Current = DashSkill;
            gcd.StartCooling();
            
            DashSkill.Activate(mousePosition);
            OnActivatedSkill.Invoke(DashSkill);
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
        
        public void ActiveSkill(SkillComponent skill, Vector3 targetPosition)
        {
            if (Conditions.HasFalse) return;

            Rotate(targetPosition);
            Stop();
            Current = skill;
            gcd.StartCooling();
                
            skill.Activate(targetPosition);
            OnActivatedSkill.Invoke(skill);
        }

        public void ReleaseSkill(SkillComponent skill)
        {
            skill.Release();
            OnReleasedSkill.Invoke(skill);
        }

        public void InterruptCurrentSkill()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Interrupted();
            OnInterruptedSkill.Invoke(Current);
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
    }
}
