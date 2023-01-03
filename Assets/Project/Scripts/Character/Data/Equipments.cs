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

        private int instanceID;
        private CharacterBehaviour cb;

        public List<EquipmentItem> EquipmentItemList => equipmentItemList;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public float MaxHp => EquipmentItemList.Sum(x => x.MaxHp);
        public float Critical => EquipmentItemList.Sum(x => x.Critical);
        public float Haste => EquipmentItemList.Sum(x => x.Haste);
        public float Hit => EquipmentItemList.Sum(x => x.Hit);
        public float Evade => EquipmentItemList.Sum(x => x.Evade);


        private void AddValueTable()
        {
            Cb.StatTable.Register(StatCode.AddPower, instanceID, () => 1f, true);
            Cb.StatTable.Register(StatCode.AddMaxHp, instanceID, () => MaxHp, true);
            Cb.StatTable.Register(StatCode.AddCritical, instanceID, () => Critical, true);
            Cb.StatTable.Register(StatCode.AddHaste, instanceID, () => Haste, true);
            Cb.StatTable.Register(StatCode.AddHit, instanceID, () => Hit, true);
            Cb.StatTable.Register(StatCode.AddEvade, instanceID, () => Evade, true);
        }

        private void Awake()
        {
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
            AddValueTable();
        }
    }
}
