using Core;
using MainGame;
using Sirenix.OdinInspector;
using UnityEngine;
using CombatClassData = MainGame.Data.ContentData.CombatClassData.CombatClass;

namespace Common.Character.Data
{
    public class ClassBaseStats : MonoBehaviour
    {
        private const string BaseStatsKey = "BaseStats";
        
        [SerializeField] private string combatClass;
        [SerializeField] private float maxHp;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float critical;
        [SerializeField] private float haste;
        [SerializeField] private float hit;
        [SerializeField] private float evade;
        
        private CharacterBehaviour cb;
        private CombatClassData classData;
        private string characterName;

        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private CombatClassData ClassData => classData ??= MainData.GetCombatClassData(combatClass);

        [Button]
        public void Initialize()
        {
            combatClass.IsNullOrEmpty().OnTrue(() => combatClass = Cb.CombatClass);
            classData = MainData.GetCombatClassData(combatClass);

            maxHp = ClassData.MaxHp;
            moveSpeed = ClassData.MoveSpeed;
            critical = ClassData.Critical;
            haste = ClassData.Haste;
            hit = ClassData.Hit;
            evade = ClassData.Evade;
            
            AddValueTable();
        }

        public void AddValueTable()
        {
            Cb.MaxHp.RegisterSumType(BaseStatsKey, maxHp);
            Cb.MoveSpeed.RegisterSumType(BaseStatsKey, moveSpeed);
            Cb.Critical.RegisterSumType(BaseStatsKey, critical);
            Cb.Haste.RegisterSumType(BaseStatsKey, haste);
            Cb.Hit.RegisterSumType(BaseStatsKey, hit);
            Cb.Evade.RegisterSumType(BaseStatsKey, evade);
        }
        
        private void Awake()
        {
            Cb.OnStart.Register(GetInstanceID(), Initialize);
        }
    }
}
