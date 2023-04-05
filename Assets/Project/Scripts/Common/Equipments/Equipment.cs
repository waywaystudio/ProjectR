using UnityEngine;

namespace Common.Equipments
{
    public class Equipment : MonoBehaviour, IDataIndexer, IEditable
    {
        [SerializeField] protected DataIndex dataCode;
        [SerializeField] protected string title;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected EquipType equipType;
        [SerializeField] protected CombatClassType availableClass;
        [SerializeField] protected Spec spec;

        // effect
        public DataIndex ActionCode => dataCode;
        public string Title => title;
        public EquipType EquipType => equipType;
        public CombatClassType AvailableClass => availableClass;
        public Spec Spec => spec;

        
#if UNITY_EDITOR
        public virtual void EditorSetUp() {}
#endif
    }
}
