using System;
using MainGame;
using Skill = MainGame.Data.ContentData.SkillData.Skill;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Buff
 * Casting
 * CoolTime
 * Damage
 * Heal
 * Position
 * Projectile
 * Target
 */

namespace Common.Character.Skills.Core
{
    public abstract class SkillAttribution : MonoBehaviour
    {
        [SerializeField] protected CharacterCombat combat;
        [SerializeField] protected int id;
        [SerializeField] protected string skillName;
        
        private string animationKey;
        private Skill skillData;

        public string SkillName => skillName ??= GetType().Name;
        public Skill SkillData => skillData ??= MainData.GetSkillData(SkillName);
        public CharacterCombat Combat => combat ??= GetComponent<CharacterCombat>();
        public Dictionary<EntityType, EntityAttribution> EntityTable { get; } = new();

        public Action OnStarted { get; set; }
        public Action OnInterrupted { get; set; }
        public Action OnCompleted { get; set; }

        public string AnimationKey => "Attack"; // isTemporary, originally : animationKey;
        public bool IsSkillReady => EntityTable.All(x => x.Value.IsReady);
        public bool IsCoolTimeReady =>!EntityTable.ContainsKey(EntityType.CoolTime) || 
                                       EntityTable[EntityType.CoolTime].IsReady;

        public abstract void InvokeEvent();
        public void StartSkill() => OnStarted?.Invoke();
        public void InterruptedSkill() => OnInterrupted?.Invoke();
        public void CompleteSkill() => OnCompleted?.Invoke();

#if UNITY_EDITOR
        #region EditorOnly
        [Sirenix.OdinInspector.OnInspectorInit]
        protected virtual void Initialize()
        {
            id = SkillData.ID;
            animationKey = SkillData.AnimationKey;
        }
        #endregion
#endif
    }
}
