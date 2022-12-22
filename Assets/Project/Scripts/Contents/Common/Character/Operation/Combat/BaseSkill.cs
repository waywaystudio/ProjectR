using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
// ReSharper disable MemberCanBeProtected.Global

namespace Common.Character.Operation.Combat
{
    public abstract class BaseSkill : MonoBehaviour, ICombatProvider
    {
        [SerializeField] protected int id;
        [SerializeField] protected string actionName;
        [SerializeField] protected int priority;
        
        protected int InstanceID;
        protected CharacterBehaviour Cb;
        protected Combating Combat;

        private int isFinishHash;

        public int ID => id;
        public string ActionName => actionName;
        public string Name => Predecessor.Name;
        public GameObject Object => Predecessor.Object;
        public ICombatProvider Predecessor => Cb;
        public virtual CombatValueEntity CombatValue { get; }
        
        public void CombatReport(CombatLog log) => Predecessor.CombatReport(log);

        public int Priority => priority;

        public Dictionary<EntityType, BaseEntity> EntityTable { get; } = new();
        
        public bool IsSkillFinished { get; set; }
        public bool IsSkillReady => EntityTable.All(x => x.Value.IsReady);
        public bool IsCoolTimeReady =>!EntityTable.ContainsKey(EntityType.CoolTime) || 
                                       EntityTable[EntityType.CoolTime].IsReady;

        public ActionTable OnStarted { get; } = new();
        public ActionTable OnInterrupted { get; } = new();
        public ActionTable OnCompleted { get; } = new();

        public virtual void StartSkill() => OnStarted?.Invoke();
        public void InterruptedSkill() => OnInterrupted?.Invoke();
        public virtual void CompleteSkill() => OnCompleted?.Invoke();
        
        /// <summary>
        /// Register Animation Event.
        /// </summary>
        public virtual void InvokeEvent(){}

        public virtual void ActiveSkill()
        {
            Cb.OnSkill.Register(InstanceID, StartSkill);
            Cb.OnSkillHit.Register(InstanceID, InvokeEvent);
            Cb.Skill(ActionName, CompleteSkill);
        }

        public void DeActiveSkill()
        {
            Cb.OnSkill.Unregister(InstanceID);
            Cb.OnSkillHit.Unregister(InstanceID);
        }

        public bool TryGetEntity<T>(EntityType entityType, out T result) where T : BaseEntity
        {
            if (!EntityTable.TryGetValue(entityType, out var entity))
            {
                result = null;
                return false;
            }
            
            result = entity as T;
            return true;
        }
        

        protected void Awake()
        {
            InstanceID = GetInstanceID();
            Cb = GetComponentInParent<CharacterBehaviour>();
            Combat = GetComponentInParent<Combating>();
            isFinishHash = IsSkillFinished.GetHashCode();
            
            actionName ??= GetType().Name;
        }

        protected void OnEnable()
        {
            Combat.SkillTable.TryAdd(id, this);
            
            OnStarted.Register(isFinishHash, () => IsSkillFinished = false);
            OnCompleted.Register(isFinishHash, () => IsSkillFinished = true);
        }

        protected void OnDisable()
        {
            Combat.SkillTable.TryRemove(id);
            DeActiveSkill();
            
            OnStarted.Unregister(isFinishHash);
            OnCompleted.Unregister(isFinishHash);
        }

        protected virtual void Reset() => actionName = GetType().Name;

#if UNITY_EDITOR
        #region EditorOnly
        protected void GetDataFromDB()
        {
            var skillData = MainGame.MainData.GetSkillData(actionName);
            
            actionName.IsNullOrEmpty().OnTrue(() => actionName = GetType().Name);
            id = skillData.ID;
            priority = skillData.Priority;
            GetComponents<BaseEntity>().ForEach(x => x.SetEntity());
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        protected void ShowDB()
        {
            UnityEditor.EditorUtility.OpenPropertyEditor
                (MainGame.MainData.DataObjectList.Find(x => x.Category == MainGame.Data.DataCategory.Skill));
        }
        #endregion
#endif
    }
}
