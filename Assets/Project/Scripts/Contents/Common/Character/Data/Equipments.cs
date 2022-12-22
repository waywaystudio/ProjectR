using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Common.Character.Data
{
    public class Equipments : MonoBehaviour
    {
        [SerializeField] private List<EquipmentItem> equipmentItemList = new(8);
        
        // EquipmentID?
        // [SerializeField] private int weaponID;
        // [SerializeField] private int subWeaponID;
        // [SerializeField] private int headID;
        // [SerializeField] private int shoulderID;
        // [SerializeField] private int chestID;
        // [SerializeField] private int legID;
        // [SerializeField] private int ringID;
        // [SerializeField] private int trinketID;
        
        [SerializeField] private EquipmentItem weapon;
        [SerializeField] private EquipmentItem subWeapon;
        [SerializeField] private EquipmentItem head;
        [SerializeField] private EquipmentItem shoulder;
        [SerializeField] private EquipmentItem chest;
        [SerializeField] private EquipmentItem leg;
        [SerializeField] private EquipmentItem ring;
        [SerializeField] private EquipmentItem trinket;

        private const string EquipmentCode = "Equipment";
        private int instanceID;
        private CharacterBehaviour cb;

        public List<EquipmentItem> EquipmentItemList => equipmentItemList;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public float MaxHp => EquipmentItemList.Sum(x => x.MaxHp);
        public float Critical => EquipmentItemList.Sum(x => x.Critical);
        public float Haste => EquipmentItemList.Sum(x => x.Haste);
        public float Hit => EquipmentItemList.Sum(x => x.Hit);
        public float Evade => EquipmentItemList.Sum(x => x.Evade);


        private void Initialize()
        {
            OnEquipmentChanged();
        }

        private void OnEquipmentChanged()
        {
            AddValueTable();
        }
        
        private void AddValueTable()
        {
            // TEMP
            Cb.CombatPowerTable.Register(EquipmentCode, 1, true);
            
            Cb.MaxHpTable.Register(EquipmentCode, () => MaxHp, true);
            Cb.CriticalTable.Register(EquipmentCode, () => Critical, true);
            Cb.HasteTable.Register(EquipmentCode, () => Haste, true);
            Cb.HitTable.Register(EquipmentCode, () => Hit, true);
            Cb.EvadeTable.Register(EquipmentCode, () => Evade, true);
            
            Cb.StatTable.Register(StatCode.AddMaxHp, instanceID, () => MaxHp, true);
            Cb.StatTable.Register(StatCode.AddCritical, instanceID, () => Critical, true);
            Cb.StatTable.Register(StatCode.AddHaste, instanceID, () => Haste, true);
            Cb.StatTable.Register(StatCode.AddHit, instanceID, () => Hit, true);
            Cb.StatTable.Register(StatCode.AddEvade, instanceID, () => Evade, true);
        }

        private void Awake()
        {
            // weapon.SetStats(weaponID);
            // subWeapon.SetStats(subWeaponID);
            // head.SetStats(headID);
            // shoulder.SetStats(shoulderID);
            // chest.SetStats(chestID);
            // leg.SetStats(legID);
            // ring.SetStats(ringID);
            // trinket.SetStats(trinketID);

            EquipmentItemList.Clear();
            EquipmentItemList.AddUniquely(weapon);
            EquipmentItemList.AddUniquely(subWeapon);
            EquipmentItemList.AddUniquely(head);
            EquipmentItemList.AddUniquely(shoulder);
            EquipmentItemList.AddUniquely(chest);
            EquipmentItemList.AddUniquely(leg);
            EquipmentItemList.AddUniquely(ring);
            EquipmentItemList.AddUniquely(trinket);

            instanceID = GetInstanceID();
            Cb.OnStart.Register(instanceID, Initialize);
        }
    }
}
