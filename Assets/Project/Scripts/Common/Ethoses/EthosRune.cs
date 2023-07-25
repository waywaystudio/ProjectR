using UnityEngine;

namespace Common.Ethoses
{
    public class EthosRune : MonoBehaviour
    {
        [SerializeField] private int grade;
        [SerializeField] private Sprite icon;
        // Category // Currency, Skill, Character
        
        // EthosTask
        // EthosProgression
        // EthosReward
    }

    public class EthosCreator
    {
        // public EthosRune CreateInstance(Camp campProgress)
        public EthosRune CreateInstance()
        {
            // get Camp Progress
            // suppose 1 to 10
            
            return null;
        }

        public EthosRune CreateInstance(int campLevel)
        {
            // Set Random Task by campLevel
            // Set Random Reward by campLevel
            
            return null;
        }
    }
}
