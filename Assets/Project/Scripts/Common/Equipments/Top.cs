namespace Common.Equipments
{
    public class Top : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            var topData = Database.TopData(dataCode);

            title    = topData.Name;
            equipType = EquipType.Top;
            
            spec.Clear();

            (topData.Power != 0.0f).OnTrue(() => spec.Add(StatType.Power, StatApplyType.Plus, topData.Power));
            (topData.Health != 0.0f).OnTrue(() => spec.Add(StatType.Health, StatApplyType.Plus, topData.Health));
            (topData.CriticalChance != 0.0f).OnTrue(() => spec.Add(StatType.CriticalChance, StatApplyType.Plus, topData.CriticalChance));
            (topData.CriticalDamage != 0.0f).OnTrue(() => spec.Add(StatType.CriticalDamage, StatApplyType.Plus, topData.CriticalDamage));
            (topData.Haste != 0.0f).OnTrue(() => spec.Add(StatType.Haste, StatApplyType.Plus, topData.Haste));
            (topData.Armor != 0.0f).OnTrue(() => spec.Add(StatType.Armor, StatApplyType.Plus, topData.Armor));
        }
#endif
    }
}
