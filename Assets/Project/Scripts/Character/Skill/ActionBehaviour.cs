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
        public ActionTable<SkillComponent> OnCommonActivated { get; } = new();
        public ActionTable<SkillComponent> OnCommonCanceled { get; } = new();
        public ActionTable<SkillComponent> OnCommonReleased { get; } = new();
        public SkillComponent Current { get; set; }
        public Vector3 RootPosition => rootTransform.position;
        public bool IsSkillEnded => Current.IsNullOrEmpty() || Current.IsEnded;

        public void Run(Vector3 destination)
        {
            if (!move.IsMovable) return;
            
            CancelSkill();
            
            move.Move(destination, model.Idle);
            model.Run();
        }

        public void Rotate(Vector3 position)
        {
            move.RotateTo(position);
            model.Flip(move.Direction);
        }

        public void Stop()
        {
            move.Stop();
            model.Idle();
        }

        public void Dash(Vector3 position)
        {
            CancelSkill();

            move.RotateTo(position);
            Stop();
            Current = DashSkill;
            gcd.StartCooling();
            
            DashSkill.Activate();
            OnCommonActivated.Invoke(DashSkill);
        }
        
        public void Teleport()
        {
            if (!MainManager.Input.TryGetMousePosition(out var mousePosition)) return;
            
            Teleport(mousePosition - RootPosition);
        }

        public void Teleport(Vector3 position)
        {
            CancelSkill();
            
            move.RotateTo(position);
            model.Idle();
            move.Teleport(position);
        }

        // + Stun, Fear, etc.
        public void KnockBack(Vector3 from, Action callback)
        {
            CancelSkill();
            move.KnockBack(from, callback);
            model.PlayOnce("fall");
        }
        
        
        public void ActiveSkill(SkillComponent skill, Vector3 targetPosition)
        {
            if (Conditions.HasFalse) return;

            Stop();
            Rotate(targetPosition);
            Current = skill;
            gcd.StartCooling();
                
            skill.Activate();
            
            OnCommonActivated.Invoke(skill);
        }

        public void ReleaseSkill(SkillComponent skill)
        {
            skill.Release();
            
            OnCommonReleased.Invoke(skill);
        }

        public void CancelSkill()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Cancel();
            OnCommonCanceled.Invoke(Current);
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
            
            skillList.ForEach(x => x.OnEnded.Register("BehaviourUnregister", () => Current = null));

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
