namespace Common.Equipments
{
    public class Weapon : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            var weaponData = Database.WeaponData(dataCode);

            title          = weaponData.Name;
            equipType      = EquipType.Weapon;

            spec.Clear();
            
            (weaponData.MinDamage != 0.0f).OnTrue(() => spec.Add(StatType.MinDamage, StatApplyType.Plus, weaponData.MinDamage));
            (weaponData.MaxDamage != 0.0f).OnTrue(() => spec.Add(StatType.MaxDamage, StatApplyType.Plus, weaponData.MaxDamage));
            (weaponData.Power != 0.0f).OnTrue(() => spec.Add(StatType.Power, StatApplyType.Plus, weaponData.Power));
            (weaponData.Health != 0.0f).OnTrue(() => spec.Add(StatType.Health, StatApplyType.Plus, weaponData.Health));
            (weaponData.CriticalChance != 0.0f).OnTrue(() => spec.Add(StatType.CriticalChance, StatApplyType.Plus, weaponData.CriticalChance));
            (weaponData.CriticalDamage != 0.0f).OnTrue(() => spec.Add(StatType.CriticalDamage, StatApplyType.Plus, weaponData.CriticalDamage));
            (weaponData.Haste != 0.0f).OnTrue(() => spec.Add(StatType.Haste, StatApplyType.Plus, weaponData.Haste));
        }
#endif
    }
}
