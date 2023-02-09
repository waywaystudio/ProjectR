using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Combat
{
    using Skill;
    
    public class SkillBehaviour : MonoBehaviour
    {
        // [SerializeField] private List<SkillComponent> skillList = new();
        //
        // private Dictionary<DataIndex, SkillComponent> SkillTable { get; } = new();
        // private SkillComponent CurrentSkill { get; set; }
        //
        //
        // public void UseSkill(DataIndex skillID)
        // {
        //     if (!SkillTable.TryGetValue(skillID, out var skill))
        //     {
        //         Debug.LogWarning($"{skillID.ToString()} is not registered.");
        //         return;
        //     }
        //
        //     if (CurrentSkill != null && !CurrentSkill.IsFinished)
        //     {
        //         // Cancel or Disable to use
        //         // Try Cancel first
        //         CurrentSkill.OnCanceled.Invoke();
        //     }
        //
        //     Unregister();
        //     Register(skill);
        //     // GCD here?
        //
        //     skill.UseSkill();
        // }
        //
        // private void Register(SkillComponent skill)
        // {
        //     CurrentSkill = skill;
        // }
        //
        // private void Unregister()
        // {
        //     CurrentSkill = null;
        // }
        //
        // private void Awake()
        // {
        //     skillList.ForEach(x => SkillTable.Add(x.ActionCode, x));
        // }
        //
        //
        // public void SetUp()
        // {
        //     skillList.Clear();
        //     
        //     GetComponents<SkillComponent>().ForEach(x =>
        //     {
        //         if (x.gameObject.activeSelf)
        //         {
        //             skillList.Add(x);
        //         }
        //     });
        // }
    }
}
