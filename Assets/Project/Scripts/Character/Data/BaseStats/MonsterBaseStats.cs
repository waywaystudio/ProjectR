using MainGame;

namespace Common.Character.Data.BaseStats
{
    public class MonsterBaseStats : ClassBaseStats
    {
#if UNITY_EDITOR
        protected override void SetUp()
        {
            var monsterBehaviour = GetComponentInParent<MonsterBehaviour>();
            
            baseStatCode = monsterBehaviour.ID;
            var bossData = MainData.GetBoss(baseStatCode);

            maxHp = bossData.MaxHp;
            moveSpeed = bossData.MoveSpeed;
            maxResource = bossData.MaxResource;
            critical = bossData.Critical;
            haste = bossData.Haste;
            hit = bossData.Hit;
            evade = bossData.Evade;
            armor = bossData.Armor;
        }
#endif
    }
}
