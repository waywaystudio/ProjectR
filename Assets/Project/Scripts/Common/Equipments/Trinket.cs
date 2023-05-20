namespace Common.Equipments
{
    public class Trinket : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var trinketData = Database.TrinketData(DataIndex);

            (trinketData.Power != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Power, EquipmentKey, trinketData.Power));
            (trinketData.Health != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Health, EquipmentKey, trinketData.Health));
            (trinketData.CriticalChance != 0.0f).OnTrue(() => ConstSpec.Add(StatType.CriticalChance, EquipmentKey, trinketData.CriticalChance));
            (trinketData.CriticalDamage != 0.0f).OnTrue(() => ConstSpec.Add(StatType.CriticalDamage, EquipmentKey, trinketData.CriticalDamage));
            (trinketData.Haste != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Haste, EquipmentKey, trinketData.Haste));
        }
#endif
    }
}
