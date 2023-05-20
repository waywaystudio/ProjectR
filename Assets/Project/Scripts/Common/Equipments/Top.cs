namespace Common.Equipments
{
    public class Top : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var topData = Database.TopData(DataIndex);

            (topData.Power  != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Power, EquipmentKey, topData.Power));
            (topData.Health != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Health, EquipmentKey, topData.Health));
            (topData.Armor  != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Armor, EquipmentKey, topData.Armor));
        }
#endif
    }
}
