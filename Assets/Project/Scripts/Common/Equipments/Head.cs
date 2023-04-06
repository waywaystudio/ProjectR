namespace Common.Equipments
{
    public class Head : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var headData = Database.HeadData(ActionCode);

            (headData.Power != 0.0f).OnTrue(() => spec.Add(StatType.Power, StatApplyType.Plus, headData.Power));
            (headData.Health != 0.0f).OnTrue(() => spec.Add(StatType.Health, StatApplyType.Plus, headData.Health));
            (headData.CriticalChance != 0.0f).OnTrue(() => spec.Add(StatType.CriticalChance, StatApplyType.Plus, headData.CriticalChance));
            (headData.CriticalDamage != 0.0f).OnTrue(() => spec.Add(StatType.CriticalDamage, StatApplyType.Plus, headData.CriticalDamage));
            (headData.Haste != 0.0f).OnTrue(() => spec.Add(StatType.Haste, StatApplyType.Plus, headData.Haste));
            (headData.Armor != 0.0f).OnTrue(() => spec.Add(StatType.Armor, StatApplyType.Plus, headData.Armor));
        }
#endif
    }
}
