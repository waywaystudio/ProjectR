using System;
using Common.Equipments;
using Serialization;
using UnityEngine;

namespace Common.Characters
{
    [Serializable]
    public class CharacterEquipmentEntity
    {
        [SerializeField] private DataIndex initialWeapon;
        [SerializeField] private DataIndex initialArmor;

        [Sirenix.OdinInspector.ShowInInspector] public WeaponEntity WeaponEntity { get; set; }//  = new();
        [Sirenix.OdinInspector.ShowInInspector] public ArmorEntity ArmorEntity { get; set; }//  = new();
        
        public StatTable StatTable { get; set; } = new();
        public EthosTable EthosTable { get; set; } = new();
        public IEquipment GetWeapon() => WeaponEntity;
        public IEquipment GetArmor() => ArmorEntity;

        public void Save(string providerName)
        {
            SaveManager.Save(SerializeKey(providerName, EquipmentSlotType.Weapon), WeaponEntity);
            SaveManager.Save(SerializeKey(providerName, EquipmentSlotType.Top), ArmorEntity);
        }

        public void Load(string providerName)
        {
            WeaponEntity = SaveManager.Load(SerializeKey(providerName, EquipmentSlotType.Weapon),InitialWeapon(initialWeapon));
            ArmorEntity    = SaveManager.Load(SerializeKey(providerName, EquipmentSlotType.Top),InitialArmor(initialArmor));
            
            WeaponEntity.Generate();
            ArmorEntity.Generate();
            
            StatTable.Add(WeaponEntity.StatSpec);
            StatTable.Add(ArmorEntity.StatSpec);
        }
        
        private string SerializeKey(string providerName, EquipmentSlotType slot) => $"{providerName}.{slot.ToString()}";
        private WeaponEntity InitialWeapon(DataIndex dataIndex) => new (dataIndex);
        private ArmorEntity InitialArmor(DataIndex dataIndex) => new (dataIndex);


#if UNITY_EDITOR
        public void EditorSetUpByDataIndex(DataIndex dataIndex)
        {
            switch (dataIndex.GetCategory())
            {
                case DataIndex.Venturer: LoadAdventurerData(dataIndex); break;
                case DataIndex.Villain:  LoadMonsterData(dataIndex); break;
                default:
                {
                    Debug.LogWarning($"DataIndex Error. Must be CombatClass or Boss. Input Category:{dataIndex.GetCategory()}");
                    return;
                } 
            }
        }
        
        private void LoadAdventurerData(DataIndex dataIndex)
        {
            var classData = Database.CombatClassSheetData(dataIndex);

            if (!Verify.IsNotNull(classData, $"Not Exist {dataIndex} in AdventurerData")) return;
            
            initialWeapon = (DataIndex)classData.InitialEquipments[0];
            initialArmor = (DataIndex)classData.InitialEquipments[2];
        }
        private void LoadMonsterData(DataIndex dataIndex)
        {
            // var monsterData = Database.BossSheetData(dataIndex);
            // TODO. 추후 기획에 Monster에 초기장비가 있다면 추가, 없다면 삭제.
            // monsterData.InitialEquipments.ForEach(equipment => initialEquipmentIndexList.Add((DataIndex)equipment));
        }
#endif
    }
}
