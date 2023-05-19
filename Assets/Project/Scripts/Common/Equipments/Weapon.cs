namespace Common.Equipments
{
    public class Weapon : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            var weaponData = Database.WeaponData(ActionCode);
            
            (weaponData.MinDamage != 0.0f).OnTrue(() => Spec.Add(StatType.MinDamage, StatApplyType.Plus, weaponData.MinDamage));
            (weaponData.MaxDamage != 0.0f).OnTrue(() => Spec.Add(StatType.MaxDamage, StatApplyType.Plus, weaponData.MaxDamage));
            (weaponData.Power != 0.0f).OnTrue(() => Spec.Add(StatType.Power, StatApplyType.Plus, weaponData.Power));
        }
#endif
    }
}
