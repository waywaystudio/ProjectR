namespace Common.Equipments
{
    public class Top : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var topData = Database.TopData(ActionCode);

            (topData.Power != 0.0f).OnTrue(() => Spec.Add(StatType.Power, StatApplyType.Plus, topData.Power));
            (topData.Health != 0.0f).OnTrue(() => Spec.Add(StatType.Health, StatApplyType.Plus, topData.Health));
            (topData.CriticalChance != 0.0f).OnTrue(() => Spec.Add(StatType.CriticalChance, StatApplyType.Plus, topData.CriticalChance));
            (topData.CriticalDamage != 0.0f).OnTrue(() => Spec.Add(StatType.CriticalDamage, StatApplyType.Plus, topData.CriticalDamage));
            (topData.Haste != 0.0f).OnTrue(() => Spec.Add(StatType.Haste, StatApplyType.Plus, topData.Haste));
            (topData.Armor != 0.0f).OnTrue(() => Spec.Add(StatType.Armor, StatApplyType.Plus, topData.Armor));
        }
#endif
    }
}
