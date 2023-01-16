using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Character.Combat
{
    using Skill;
    
    // ISkillTable
    public class SkillTable : MonoBehaviour, IEditorSetUp
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private List<SkillObject> skillList = new(4);

        public SkillObject CurrentSkill { get; set; }

        public void SwitchSkill(DataIndex existSkill, DataIndex newSkill) { }
        public void RemoveSkill(DataIndex existSkill) { }
        public void AddSkill(DataIndex newSkill) { }

        public void StartSkill(ISkill skill)
        {
            skill.Active();
        }

        public void CompleteSkill(ISkill skill)
        {
            skill.Complete();
        }

        public void HitSkill(ISkill skill)
        {
            skill.Hit();
        }


#if UNITY_EDITOR
        public void SetUp()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>();
            // TODO. 나중에는 인게임에서 캐릭터의 스킬을 변경할 수 있게 될 것이다.
            skillList = GetComponentsInChildren<SkillObject>().Where(x => x.enabled).ToList();
        }
#endif
    }
}
