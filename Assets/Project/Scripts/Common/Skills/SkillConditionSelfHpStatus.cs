using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Skills
{
    public class SkillConditionSelfHpStatus : MonoBehaviour
    {
        private enum RatioConditionType { None, More, Less,}

        [PropertyRange(0, 1f)]
        [SerializeField] private float availableRatio;
        [SerializeField] private RatioConditionType availableCondition;

        private ICombatProvider provider;
        private IConditionalSequence sequence;
        private float RemainHpRatio => provider.DynamicStatEntry.Hp.Value / provider.DynamicStatEntry.OldStatTable.MaxHp;


        private void OnEnable()
        {
            provider = GetComponentInParent<ICombatProvider>();

            TryGetComponent(out sequence);

            switch (availableCondition)
            {
                case RatioConditionType.More:
                    sequence.Conditions.Register("ConditionSelfHpStatus", () => RemainHpRatio >= availableRatio);
                    break;
                case RatioConditionType.Less:
                    sequence.Conditions.Register("ConditionSelfHpStatus", () => RemainHpRatio <= availableRatio);
                    break;
                case RatioConditionType.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
