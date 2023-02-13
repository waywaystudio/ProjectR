using MainGame;
using UnityEngine;

namespace Character.Data.BaseStats
{
    public class AdventurerConstStats : CharacterConstStats
    {
#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();
            
            var ab = GetComponentInParent<AdventurerBehaviour>();
            
            baseStatCode = ab.ActionCode;
            var classData = MainData.GetCombatClass(baseStatCode);
            
            maxHp.Value       = classData.MaxHp;
            moveSpeed.Value   = classData.MoveSpeed;
            maxResource.Value = classData.MaxResource;
            critical.Value    = classData.Critical;
            haste.Value       = classData.Haste;
            hit.Value         = classData.Hit;
            evade.Value       = classData.Evade;
            armor.Value      = classData.Armor;
            
            Debug.Log($"Adventurer Const Stat:{ab.ActionCode} Load Complete");
        }
#endif
    }
}
