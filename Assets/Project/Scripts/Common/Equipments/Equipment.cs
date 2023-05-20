using UnityEngine;

namespace Common.Equipments
{
    public class Equipment : MonoBehaviour, IDataIndexer, IEditable
    {
        [SerializeField] protected EquipmentConstEntity constEntity;

        public DataIndex DataIndex => constEntity.DataIndex;
        public EquipType EquipType => constEntity.EquipType;
        public CombatClassType AvailableClass => constEntity.AvailableClassType;
        public string Title => constEntity.ItemName;
        public Sprite Icon => constEntity.Icon;
        public Spec ConstSpec => constEntity.ConstSpec;
        public Spec DynamicSpec { get; set; }
        public int UpgradeLevel { get; set; }
        // Vices
        public Spec EquipmentSpec { get; set; }

        protected string EquipmentKey => EquipType.ToString();

        public void Initialize()
        {
            EquipmentSpec = ConstSpec + DynamicSpec;
        }
        
        // public void Upgrade()
        // {
        //     
        // }
        //
        // public void EnchantSpec()
        // {
        //     
        // }
        //
        // public void EnchantVices()
        // {
        //     
        // }


#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            var targetDataIndex = DataIndex;
            
            if (DataIndex == DataIndex.None)
            {
                targetDataIndex = name.ConvertDataIndexStyle().TryFindDataIndex();
            }
            
            constEntity.ConstSpec.Clear();
            constEntity.SetEntity(targetDataIndex);
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        
        // [SerializeField] protected EquipmentInfo info;
        // public EquipmentInfo Info => info;
#endif
    }
}
