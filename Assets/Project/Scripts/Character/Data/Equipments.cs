using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Character.Data
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
        [SerializeField] private EquipmentItem head;
        [SerializeField] private EquipmentItem chest;
        [SerializeField] private EquipmentItem leg;
        [SerializeField] private EquipmentItem ring1;
        [SerializeField] private EquipmentItem ring2;

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
            // TODO. Equipment 필드 변경 필요
            // Cb.StatTable.Register(StatCode.AddPower, IDCode.None, 1f, true);
            // Cb.StatTable.Register(StatCode.AddMaxHp, IDCode.None, MaxHp, true);
            // Cb.StatTable.Register(StatCode.AddCritical, IDCode.None, Critical, true);
            // Cb.StatTable.Register(StatCode.AddHaste, IDCode.None, Haste, true);
            // Cb.StatTable.Register(StatCode.AddHit, IDCode.None, Hit, true);
            // Cb.StatTable.Register(StatCode.AddEvade, IDCode.None, Evade, true);
        }

        private void Awake()
        {
            EquipmentItemList.Clear();
            EquipmentItemList.AddUniquely(weapon);
            EquipmentItemList.AddUniquely(head);
            EquipmentItemList.AddUniquely(chest);
            EquipmentItemList.AddUniquely(leg);
            EquipmentItemList.AddUniquely(ring1);
            EquipmentItemList.AddUniquely(ring2);

            instanceID = GetInstanceID();
            AddValueTable();
        }
    }
}
