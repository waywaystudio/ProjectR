using MainGame;

namespace Character.Data.BaseStats
{
    public class MonsterConstStats : CharacterConstStats
    {
#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();

            var mb = GetComponentInParent<MonsterBehaviour>();
            
            baseStatCode = mb.ActionCode;
            var bossData = MainData.BossSheetData(baseStatCode);
            
            maxHp.Value       = bossData.MaxHp;
            moveSpeed.Value   = bossData.MoveSpeed;
            maxResource.Value = bossData.MaxResource;
            critical.Value    = bossData.Critical;
            haste.Value       = bossData.Haste;
            armor.Value       = bossData.Armor;
        }
#endif
    }
}
