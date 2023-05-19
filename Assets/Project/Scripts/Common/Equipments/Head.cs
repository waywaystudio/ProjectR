namespace Common.Equipments
{
    public class Head : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var headData = Database.HeadData(ActionCode);

            (headData.Power  != 0.0f).OnTrue(() => Spec.Add(StatType.Power, StatApplyType.Plus, headData.Power));
            (headData.Health != 0.0f).OnTrue(() => Spec.Add(StatType.Health, StatApplyType.Plus, headData.Health));
            (headData.Armor  != 0.0f).OnTrue(() => Spec.Add(StatType.Armor, StatApplyType.Plus, headData.Armor));
        }
#endif
    }
}
