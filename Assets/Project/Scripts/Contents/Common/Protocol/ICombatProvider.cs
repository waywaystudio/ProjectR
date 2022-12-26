using Common.Character;
using Core;
using UnityEngine;

namespace Common
{
    public interface ICombatStatContainer
    {
        string Name { get; }
        GameObject Object { get; }
        public StatTable StatTable { get; }
    }

    public interface IActionSender
    {
        string ActionName { get; }
        ICombatProvider Sender { get; }
    }
    
    public interface ICombatProvider : ICombatStatContainer, IActionSender
    {
        void ReportActive(CombatLog log);
    }
}