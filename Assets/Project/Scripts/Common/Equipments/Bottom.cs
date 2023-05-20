namespace Common.Equipments
{
    public class Bottom : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var bottomData = Database.BottomData(DataIndex);

            (bottomData.Power  != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Power, EquipmentKey, bottomData.Power));
            (bottomData.Health != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Health, EquipmentKey, bottomData.Health));
            (bottomData.Armor  != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Armor, EquipmentKey, bottomData.Armor));
        }
#endif
    }
}
