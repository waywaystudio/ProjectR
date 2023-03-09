using MainGame;
using UnityEngine;

namespace Character.Data.BaseStats
{
    public class AdventurerConstStats : CharacterConstStats
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var ab = GetComponentInParent<Adventurer>();
            
            baseStatCode = ab.ActionCode;
            var classData = DB.CombatClassSheetData(baseStatCode);
            
            maxHp.Value       = classData.MaxHp;
            moveSpeed.Value   = classData.MoveSpeed;
            maxResource.Value = classData.MaxResource;
            critical.Value    = classData.Critical;
            haste.Value       = classData.Haste;
            armor.Value      = classData.Armor;
            
            Debug.Log($"Adventurer Const Stat:{ab.ActionCode} Load Complete");
        }
#endif
    }
}
