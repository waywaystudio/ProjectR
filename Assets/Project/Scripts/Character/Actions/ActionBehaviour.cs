using System.Collections.Generic;
using Character.Graphic;
using Character.Skill;
using Character.Systems;
using Core;
using MainGame;
using UnityEngine;

namespace Character.Actions
{
    public class ActionBehaviour : MonoBehaviour, IEditable
    {
        [SerializeField] private Transform rootTransform;
        [SerializeField] private GlobalCoolDown gcd;
        [SerializeField] private CommonMove commonMove;
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
        public bool IsSkillEnded => Current.IsNullOrEmpty() || !Current.IsEnded;

        private AnimationModel Animating { get; set; }
        private PathfindingSystem pathfinding { get; set; }

        public void Run(Vector3 destination)
        {
            commonMove.Run(destination);
            CancelSkill();
        }

        public void Rotate(Vector3 lookTargetPosition)
        {
            commonMove.Rotate(lookTargetPosition);
        }

        public void Stop()
        {
            commonMove.Stop();
        }

        public void Dash(Vector3 position)
        {
            CancelSkill();

            pathfinding.RotateTo(position);
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
            
            pathfinding.RotateTo(position);
            Animating.Idle();
            pathfinding.Teleport(position);
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
            // characterSystem = GetComponentInParent<ICharacterSystem>();
            var characterSystem =   GetComponentInParent<ICharacterSystem>();

            Animating   =   characterSystem.Animating;
            pathfinding =   characterSystem.Pathfinding;
            gcd         ??= GetComponent<GlobalCoolDown>();
            commonMove  ??= GetComponent<CommonMove>();
            
            skillList.Clear();
            GetComponentsInChildren(false, skillList);
            
            skillList.ForEach(x => x.OnEnded.Register("BehaviourUnregister", () => Current = null));

            Conditions.Register("GlobalCoolTime", () => !gcd.IsCooling);
            Conditions.Register("CurrentSkillCompleted", () => IsSkillEnded);
        }

        private void Update()
        {
            Animating.Flip(rootTransform.forward);
        }
        

        public void EditorSetUp()
        {
            TryGetComponent(out gcd);
            TryGetComponent(out commonMove);
            
            skillList.Clear();
            GetComponentsInChildren(false, skillList);
        }
    }
}
