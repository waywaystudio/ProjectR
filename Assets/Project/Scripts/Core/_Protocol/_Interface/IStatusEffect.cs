using Character;
using UnityEngine;

namespace Core
{
    public interface IStatusEffect : IActionSender
    {
        // + ICombatProvider Provider { get; }
        // + DataIndex ActionCode { get; }
        
        Sprite Icon { get; }
        StatusEffectType StatusEffectType { get; }
        float Duration { get; }
        StatusEffectTable TargetTable { get; set; }

        void Active(ICombatTaker taker);
        void DeActive();
    }
}
