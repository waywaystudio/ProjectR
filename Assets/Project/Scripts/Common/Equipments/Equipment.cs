using UnityEngine;

namespace Common.Equipments
{
    public class Equipment : MonoBehaviour, IDataIndexer, IEditable
    {
        [SerializeField] protected EquipmentInfo info;
        [SerializeField] protected string title;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected CombatClassType availableClass;

        // Effect
        public EquipmentInfo Info => info;
        public DataIndex ActionCode => info.ActionCode;
        public EquipType EquipType => info.EquipType;
        public CombatClassType AvailableClass => availableClass;
        public string Title => title;
        public Sprite Icon => icon;
        public Spec Spec => info.Spec;

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
            
            Spec.Clear();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
