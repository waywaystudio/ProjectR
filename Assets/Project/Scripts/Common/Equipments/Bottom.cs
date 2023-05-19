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

            (bottomData.Power  != 0.0f).OnTrue(() => Spec.Add(StatType.Power, StatApplyType.Plus, bottomData.Power));
            (bottomData.Health != 0.0f).OnTrue(() => Spec.Add(StatType.Health, StatApplyType.Plus, bottomData.Health));
            (bottomData.Armor  != 0.0f).OnTrue(() => Spec.Add(StatType.Armor, StatApplyType.Plus, bottomData.Armor));
        }
#endif
    }
}
