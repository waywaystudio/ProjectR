using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Common.Character.Operation.Combat
{
    public class Combating : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private CombatPosition combatPosition;
        private Coroutine globalCoolDownRoutine;

        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        public CombatPosition CombatPosition => combatPosition ??= GetComponent<CombatPosition>();

        public Dictionary<int, BaseSkill> SkillTable { get; } = new();
        public BaseSkill CurrentSkill { get; set; }
        public bool IsGlobalCooling { get; set; }
        public bool IsCurrentSkillFinished => CurrentSkill == null || CurrentSkill.IsSkillFinished;
        public float GlobalCoolTime => 2.0f * CharacterUtility.GetHasteValue(Cb.StatTable.Haste);
        
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
                CurrentSkill.DeActiveSkill();

            CurrentSkill = skill;
            CurrentSkill.ActiveSkill();

            if (globalCoolDownRoutine != null) StopCoroutine(globalCoolDownRoutine);
            globalCoolDownRoutine = StartCoroutine(GlobalCoolDownRoutine());
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

        private IEnumerator GlobalCoolDownRoutine()
        {
            var coolTimer = GlobalCoolTime;
            
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
