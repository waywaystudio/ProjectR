using Common.Character;
using UnityEngine;

namespace Common
{
    public interface ICombatStatContainer
    {
        string Name { get; }
        GameObject Object { get; }
        public Status Status { get; }
        public StatTable StatTable { get; }
    }

    public interface IActionSender
    {
        IDCode ActionCode { get; }
        ICombatProvider Sender { get; }
    }
    
    public interface ICombatProvider : ICombatStatContainer, IActionSender
    {
        void ReportActive(CombatLog log);
    }
}