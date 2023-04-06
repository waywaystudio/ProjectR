namespace Common.Equipments
{
    public class Trinket : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var trinketData = Database.TrinketData(ActionCode);

            (trinketData.Power != 0.0f).OnTrue(() => spec.Add(StatType.Power, StatApplyType.Plus, trinketData.Power));
            (trinketData.Health != 0.0f).OnTrue(() => spec.Add(StatType.Health, StatApplyType.Plus, trinketData.Health));
            (trinketData.CriticalChance != 0.0f).OnTrue(() => spec.Add(StatType.CriticalChance, StatApplyType.Plus, trinketData.CriticalChance));
            (trinketData.CriticalDamage != 0.0f).OnTrue(() => spec.Add(StatType.CriticalDamage, StatApplyType.Plus, trinketData.CriticalDamage));
            (trinketData.Haste != 0.0f).OnTrue(() => spec.Add(StatType.Haste, StatApplyType.Plus, trinketData.Haste));
        }
#endif
    }
}
