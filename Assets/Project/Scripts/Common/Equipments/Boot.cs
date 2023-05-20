namespace Common.Equipments
{
    public class Boot : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            var bootData = Database.BootData(DataIndex);
            
            (bootData.Power  != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Power, EquipmentKey, bootData.Power));
            (bootData.Health != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Health, EquipmentKey, bootData.Health));
            (bootData.Armor  != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Armor, EquipmentKey, bootData.Armor));
        }
#endif
    }
}
