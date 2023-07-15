using System;
using UnityEngine;

namespace Common.Skills
{
    [Serializable]
    public class CombatCoolTimer : TimeTrigger
    {
        [SerializeField] private Section invokeSection;

        /// <summary>
        /// CombatTimer Constructor.
        /// </summary>
        /// <param name="hasteRetriever">가속 값의 주소를 입력하면, 계산 함수는 내부에서 실행</param>
        public void Initialize(ICombatObject combatObject)
        {
            if (invokeSection == Section.None) return;
        
            SetWeight(() => CombatFormula.HasteValue(combatObject.Haste));
        
            var builder = new CombatSequenceBuilder(combatObject.Sequence);
            builder
                .AddCondition("IsCoolTimeReady", () => !IsRunning)
                .Add(invokeSection, "ActiveCoolTime", Play);
        }


#if UNITY_EDITOR
        public void SetUpAsSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);
            
            duration      = skillData.CoolTime;
            invokeSection = skillData.CoolTimeInvoker.ToEnum<Section>();
        }
#endif
    }
}
