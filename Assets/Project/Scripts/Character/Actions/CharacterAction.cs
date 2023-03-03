using System.Collections.Generic;
using Character.Actions.Commons;
using Core;
using UnityEngine;

namespace Character.Actions
{
    public class CharacterAction : MonoBehaviour, IEditable
    {
        [Sirenix.OdinInspector.Button]
        public void Stun3() => Stun(3);
        
        [Sirenix.OdinInspector.Button]
        public void KnockBack3() => KnockBack(Vector3.zero, 10f);
        
        [SerializeField] private SkillAction skillAction;
        [SerializeField] private RunAction runAction;
        [SerializeField] private RotateAction rotateAction;
        [SerializeField] private StopAction stopAction;
        [SerializeField] private StunAction stunAction;
        [SerializeField] private KnockBackAction knockBackAction;
        [SerializeField] private DeadAction deadAction;

        public bool IsSkillEnded => skillAction.IsSkillEnded;
        public bool IsGlobalCooling => skillAction.IsCooling;
        public SkillAction SkillAction => skillAction;
        
        public List<SkillComponent> SkillList => skillAction.SkillList;
        public SkillComponent FirstSkill => SkillList[0];
        public SkillComponent SecondSkill => SkillList[1];
        public SkillComponent ThirdSkill => SkillList[2];
        public SkillComponent FourthSkill => SkillList[3];


        public void Run(Vector3 destination) => runAction.Active(destination);
        public void Rotate(Vector3 lookTargetPosition) => rotateAction.Active(lookTargetPosition);
        public void Stop() => stopAction.Active();
        public void Stun(float duration) => stunAction.Active(duration);
        public void KnockBack(Vector3 source, float distance) => knockBackAction.Active(source, distance);
        public void Dead() => deadAction.Active();

        // public void Dash(Vector3 position, float distance)
        // {
        //     CancelSkill();
        //
        //     commonAction.Dash(position, distance);
        // }
        //
        // public void Teleport(Vector3 direction, float distance)
        // {
        //     CancelSkill();
        //     
        //     commonAction.Teleport(direction, distance);
        // }

        public void ActiveSkill(SkillComponent skill, Vector3 targetPosition)
        {
            if (!skillAction.IsAble(skill)) return;

            Rotate(targetPosition);
            skillAction.ActiveSkill(skill);

            if (skill.IsRigid)
            {
                var runAble = runAction.Conditions;
                
                runAble.Register("OnRigidSkill", () => false);
                skill.OnEnded.Register("ReleaseRunAction", () => runAble.Unregister("OnRigidSkill"));
            }
        }

        public void CancelSkill() => skillAction.CancelSkill();
        public void ReleaseSkill() => skillAction.ReleaseSkill();

        public bool TryGetMostPrioritySkill(out SkillComponent skill)
        {
            SkillComponent result = null;
            
            foreach (var skillComponent in SkillList)
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
        
        public void EnableActions(CharacterActionMask actionMask, string disabledKey)
        {
            GetActionList(actionMask).ForEach(action => action.Conditions.Unregister(disabledKey));
        }
        
        public void DisableActions(CharacterActionMask actionMask, string disableKey)
        {
            GetActionList(actionMask).ForEach(action => action.Conditions.Register(disableKey, () => false));
        }
        
        
        private List<ICharacterAction> GetActionList(CharacterActionMask actionMask)
        {
            List<ICharacterAction> result = new();
            
            if (HasFlag(actionMask, CharacterActionMask.Skill)) result.Add(skillAction);
            if (HasFlag(actionMask, CharacterActionMask.Run)) result.Add(runAction);
            if (HasFlag(actionMask, CharacterActionMask.Rotate)) result.Add(rotateAction);
            if (HasFlag(actionMask, CharacterActionMask.Stop)) result.Add(stopAction);
            if (HasFlag(actionMask, CharacterActionMask.Stun)) result.Add(stunAction);
            if (HasFlag(actionMask, CharacterActionMask.KnockBack)) result.Add(knockBackAction);
            if (HasFlag(actionMask, CharacterActionMask.Dead)) result.Add(deadAction);

            return result;
        }

        private static bool HasFlag(CharacterActionMask actionMask, CharacterActionMask action) 
            => (actionMask & action) == actionMask;

        private void Awake()
        {
            skillAction     ??= GetComponent<SkillAction>();
            runAction       ??= GetComponent<RunAction>();
            rotateAction    ??= GetComponent<RotateAction>();
            stopAction      ??= GetComponent<StopAction>();
            stunAction      ??= GetComponent<StunAction>();
            knockBackAction ??= GetComponent<KnockBackAction>();
            deadAction      ??= GetComponent<DeadAction>();
        }

        private void OnEnable()
        {
            runAction.OnActivated.Register("CancelSkill", CancelSkill);
            
            stunAction.OnActivated.Register("CancelSkill", CancelSkill);
            stunAction.OnActivated.Register("DisableActions", () => DisableActions(stunAction.DisableActionMask, "byStun"));
            stunAction.OnCompleted.Register("EnableActions", () => EnableActions(stunAction.DisableActionMask, "byStun"));
            
            knockBackAction.OnActivated.Register("CancelSkill", CancelSkill);
            knockBackAction.OnActivated.Register("DisableActions", () => DisableActions(knockBackAction.DisableActionMask, "byKnockBack"));
            knockBackAction.OnCompleted.Register("EnableActions", () => EnableActions(knockBackAction.DisableActionMask, "byKnockBack"));
            
            deadAction.OnActivated.Register("CancelSkill", CancelSkill);
            deadAction.OnActivated.Register("DisableActions", () => DisableActions(deadAction.DisableActionMask, "byDead"));
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            TryGetComponent(out skillAction);
            TryGetComponent(out runAction);
            TryGetComponent(out rotateAction);
            TryGetComponent(out stopAction);
            TryGetComponent(out stunAction);
            TryGetComponent(out knockBackAction);
            TryGetComponent(out deadAction);
        }
#endif
    }
}
