using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using MainGame.Manager.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combating
{
    public class Combat : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private CombatPosition combatPosition;

        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        public CombatPosition CombatPosition => combatPosition ??= GetComponent<CombatPosition>();
        
        [ShowInInspector]
        public Dictionary<int, BaseSkill> SkillTable { get; } = new();
        public BaseSkill CurrentSkill { get; set; }
        public float GlobalCoolTime => 1.2f * CombatManager.GetHasteValue(Cb.HasteTable.Result);
        public bool IsGlobalCooling { get; set; }
        public bool IsCurrentSkillFinished => CurrentSkill == null || CurrentSkill.IsSkillFinished;
        
        // SharedBool :: CombatBehaviorDesigner
        public bool IsCoolOnAnySkill => SkillTable.Any(x => x.Value.IsCoolTimeReady);

        // ShardInt :: CombatBehaviorDesigner
        public int MostPrioritySkillID => GetMostPrioritySkillID();

        public bool TryGetMostPrioritySkill(out BaseSkill skill)
        {
            var coolOnSkill = SkillTable.Where(x => x.Value.IsCoolTimeReady)
                .ToList();
            
            if (!coolOnSkill.IsNullOrEmpty())
            {
                skill = coolOnSkill.MaxBy(x => x.Value.Priority).Value;
                return true;
            }

            skill = null;
            return skill is not null;
        }

        public void UseSkill(BaseSkill skill)
        {
            if (CurrentSkill != null)
            {
                CurrentSkill.DeActiveSkill();
            }

            CurrentSkill = skill;
            CurrentSkill.ActiveSkill();
            
            GlobalCoolDownOn();
        }


        private int GetMostPrioritySkillID()
        {
            /*
             * Require Priority Algorithm
             */
            var mostPrioritySkillID = SkillTable
                .Where(x => x.Value.IsCoolTimeReady)
                .MaxBy(x => x.Value.Priority).Value.ID;

            return mostPrioritySkillID;
        }

        private void GlobalCoolDownOn() => StartCoroutine(GlobalCoolDownRoutine(GlobalCoolTime));
        private IEnumerator GlobalCoolDownRoutine(float coolTime)
        {
            var coolTimer = coolTime;
            
            IsGlobalCooling = true;

            while (coolTimer > 0)
            {
                coolTimer -= Time.deltaTime;
                yield return null;
            }

            IsGlobalCooling = false;
        }
    }
}
