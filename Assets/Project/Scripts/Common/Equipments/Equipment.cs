using UnityEngine;

namespace Common.Equipments
{
    public class Equipment : MonoBehaviour, IDataIndexer, IEditable
    {
        [SerializeField] protected EquipmentInfo info;
        [SerializeField] protected string title;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected CombatClassType availableClass;
        [SerializeField] protected Spec spec;

        // Effect
        public EquipmentInfo Info => info;
        public DataIndex ActionCode => info.ActionCode;
        public EquipType EquipType => info.EquipType;
        public string Title => title;
        public Sprite Icon => icon;
        public CombatClassType AvailableClass => availableClass;
        public Spec Spec => spec;

        // TODO. After Enchant Design done,
        public void Enchant(int level)
        {
            info.EnchantLevel = level;
        }
        

#if UNITY_EDITOR
        public virtual void EditorSetUp()
        {
            if (ActionCode == DataIndex.None)
            {
                ActionCode.TryFindDataIndex(name, out var dataCode)
                          .OnTrue(() => info.SetDataCode(dataCode));
            }
            
            title          = ActionCode.ToString().DivideWords();
            availableClass = CombatClassType.All; // TEMP
            // image = Database.GetSprite(ActionCode)
            
            spec.Clear();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
