using Common.Character;
using UnityEngine;

namespace Common
{
    public interface ICombatProvider
    {
        string ActionName { get; }
        string Name { get; }
        GameObject Object { get; }
        CombatValueEntity CombatValue { get; }
        ICombatProvider Predecessor { get; }

        void CombatReport(CombatLog log);
    }
}