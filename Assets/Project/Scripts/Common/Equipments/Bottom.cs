namespace Common.Equipments
{
    public class Bottom : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var bottomData = Database.BottomData(ActionCode);

            title    = bottomData.Name;

            (bottomData.Power != 0.0f).OnTrue(() => spec.Add(StatType.Power, StatApplyType.Plus, bottomData.Power));
            (bottomData.Health != 0.0f).OnTrue(() => spec.Add(StatType.Health, StatApplyType.Plus, bottomData.Health));
            (bottomData.CriticalChance != 0.0f).OnTrue(() => spec.Add(StatType.CriticalChance, StatApplyType.Plus, bottomData.CriticalChance));
            (bottomData.CriticalDamage != 0.0f).OnTrue(() => spec.Add(StatType.CriticalDamage, StatApplyType.Plus, bottomData.CriticalDamage));
            (bottomData.Haste != 0.0f).OnTrue(() => spec.Add(StatType.Haste, StatApplyType.Plus, bottomData.Haste));
            (bottomData.Armor != 0.0f).OnTrue(() => spec.Add(StatType.Armor, StatApplyType.Plus, bottomData.Armor));
        }
#endif
    }
}
