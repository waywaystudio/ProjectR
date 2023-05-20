using UnityEngine;

namespace Common.Characters
{
    public class CharacterCombat : MonoBehaviour
    {
        [SerializeField] private CharacterConstEntity constEntity;
        [SerializeField] private CharacterEquipmentEntity equipmentEntity;

        public StatTable CharacterStatTable { get; set; }

        public void InstantiateCombatValues()
        {
            // Assemble Spec.
            // Const Spec + 
            // Player Camp Spec + 
            // Equipments Spec +
        }
    }
}
