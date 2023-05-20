namespace Common.Equipments
{
    public class Head : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var headData = Database.HeadData(DataIndex);

            (headData.Power  != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Power, EquipmentKey, headData.Power));
            (headData.Health != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Health, EquipmentKey, headData.Health));
            (headData.Armor  != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Armor, EquipmentKey, headData.Armor));
        }
#endif
    }
}
