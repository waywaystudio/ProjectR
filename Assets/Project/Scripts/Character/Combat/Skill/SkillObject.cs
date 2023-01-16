using Core;
using UnityEngine;
// ReSharper disable MemberCanBeProtected.Global

namespace Character.Combat.Skill
{
    public abstract class SkillObject : CombatObject, ISkill
    {
        [SerializeField] protected int priority;
        [SerializeField] protected Sprite icon;

        protected CharacterBehaviour Cb;

        public override ICombatProvider Provider => Cb ??= GetComponentInParent<CharacterBehaviour>();
        public Sprite Icon => icon;
        public bool HasCastingModule => ModuleTable.ContainsKey(ModuleType.Casting);
        public float CastingTime => CastingModule.CastingTime;
        public float CastingProgress => CastingModule.CastingProgress;
        public bool HasCoolTimeModule => ModuleTable.ContainsKey(ModuleType.CoolTime);
        public float CoolTime => CoolTimeModule.OriginalCoolTime;
        public Observable<float> RemainTime => CoolTimeModule.RemainTime;

        public int Priority => priority;
        public bool IsSkillFinished { get; set; }
        public bool IsCoolTimeReady => CoolTimeModule is null || CoolTimeModule.IsReady;
        public bool IsSkillReady 
        {
            get
            {
                // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
                foreach (var item in ReadyCheckList) if (!item.IsReady) return false;
                
                return true;
            }
        }

        public virtual void ActiveSkill()
        {
            Cb.SkillInfo = this;
            Cb.OnSkill.Register(InstanceID, Active);
            Cb.OnSkillHit.Register(InstanceID, Hit);
            
            Cb.Skill(ActionCode, Complete);
        }

        public void DeActiveSkill()
        {
            Cb.SkillInfo = null;
            Cb.OnSkill.Unregister(InstanceID);
            Cb.OnSkillHit.Unregister(InstanceID);
        }


        protected virtual void Awake()
        {
            Cb       = GetComponentInParent<CharacterBehaviour>();
            Provider = Cb;
            OnActivated.Register(InstanceID, () => IsSkillFinished = false);
            OnCompleted.Register(InstanceID, () => IsSkillFinished = true);
        }

        protected void OnDisable() => DeActiveSkill();


#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();

            var skillData = MainGame.MainData.GetSkill(actionCode);
            priority = skillData.Priority;

            GetComponents<CombatModule>().ForEach(x => ModuleUtility.SetSkillModule(skillData, x));
            UnityEditor.EditorUtility.SetDirty(this);
        }
        private void ShowDB()
        {
            UnityEditor.EditorUtility.OpenPropertyEditor
                (MainGame.MainData.DataList.Find(x => x.Index == 13));
        }
        private void Reset() => actionCode = GetType().Name.ToEnum<DataIndex>();
#endif
    }
}
