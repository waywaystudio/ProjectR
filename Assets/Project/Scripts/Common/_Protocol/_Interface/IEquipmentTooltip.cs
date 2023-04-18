using Common.Equipments;
using UnityEngine;

namespace Common
{
    public interface IEquipmentTooltip
    {
        public Transform transform { get; }
        public Equipment Equipment { get; }
    }
}