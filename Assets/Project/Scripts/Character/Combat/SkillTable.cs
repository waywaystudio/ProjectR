using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Character.Combat
{
    using Skill;
    
    // 클래스 별로 나누어질 수 있다.
    // 1. 클래스별로 사용가능한 모든 스킬이 저장됨.
    // 2. 해당 모험가가 등록한 스킬이 저장됨.
    public class SkillTable : MonoBehaviour, IInspectorSetUp
    {
        [SerializeField] private OldSkillBehaviour combatBehaviour;
        [SerializeField] private List<SkillObject> selectSkillList = new(4);
        // [SerializeField] private List<SkillObject> entireSkillList = new();

        public ICombatProvider Provider => combatBehaviour.Provider;
        public List<SkillObject> SelectSkillList => selectSkillList;
        public List<ISkillInfo> SelectSkillInfo { get; } = new(4);
        // public List<ISkillInfo> EntireSkillInfo { get; } = new();

        // public void SwitchSkill(DataIndex existSkill, DataIndex newSkill) { }
        // public void RemoveSkill(DataIndex existSkill) { }
        // public void AddSkill(DataIndex newSkill) { }

        private void Awake()
        {
            SelectSkillInfo.AddRange(selectSkillList);
            // EntireSkillInfo.AddRange(entireSkillList);
        }

#if UNITY_EDITOR
        public void SetUp()
        {
            combatBehaviour ??= GetComponentInParent<OldSkillBehaviour>();
            
            // TODO. 나중에는 인게임에서 캐릭터의 스킬을 변경할 수 있게 될 것이다.
            // entireSkillList = GetComponentsInChildren<SkillObject>().ToList();
            selectSkillList = GetComponentsInChildren<SkillObject>().Where(x => x.enabled).ToList();
        }
#endif
        
    }
}
