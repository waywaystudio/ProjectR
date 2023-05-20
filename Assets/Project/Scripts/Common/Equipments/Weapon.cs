namespace Common.Equipments
{
    public class Weapon : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            var weaponData = Database.WeaponData(DataIndex);
            
            (weaponData.MinDamage != 0.0f).OnTrue(() => ConstSpec.Add(StatType.MinDamage, EquipmentKey, weaponData.MinDamage));
            (weaponData.MaxDamage != 0.0f).OnTrue(() => ConstSpec.Add(StatType.MaxDamage, EquipmentKey, weaponData.MaxDamage));
            (weaponData.Power != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Power, EquipmentKey, weaponData.Power));
        }
#endif
    }
}
