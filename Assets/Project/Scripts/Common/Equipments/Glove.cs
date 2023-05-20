namespace Common.Equipments
{
    public class Glove : Equipment
    {
#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();
            
            var gloveData = Database.GloveData(DataIndex);

            (gloveData.Power  != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Power, EquipmentKey, gloveData.Power));
            (gloveData.Health != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Health, EquipmentKey, gloveData.Health));
            (gloveData.Armor  != 0.0f).OnTrue(() => ConstSpec.Add(StatType.Armor, EquipmentKey, gloveData.Armor));
        }
#endif
    }
}
