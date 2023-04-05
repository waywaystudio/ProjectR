using UnityEngine;

namespace Common.Equipments
{
    public class Equipment : MonoBehaviour, IDataIndexer, IEditable
    {
        [SerializeField] protected DataIndex dataCode;
        [SerializeField] protected string title;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected EquipType equipType;
        [SerializeField] protected AvailableClass availableClass;
        [SerializeField] protected float combatValue;
        [SerializeField] protected Spec spec;

        // effect
        public DataIndex ActionCode => dataCode;
        public Spec Spec => spec;
        public EquipType EquipType => equipType;

        
#if UNITY_EDITOR
        public virtual void EditorSetUp() {}
#endif
    }
}
