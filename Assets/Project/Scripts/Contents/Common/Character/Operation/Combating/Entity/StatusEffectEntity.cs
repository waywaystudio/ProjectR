using System;
using Core;
using MainGame;
using UnityEngine;

namespace Common.Character.Operation.Combating.Entity
{
    public abstract class StatusEffectEntity : BaseEntity, ICombatProvider
    {
        [SerializeField] private int id;
        [SerializeField] private string statusEffectName;
        
        public int ID => id;
        public string Name => statusEffectName;
        
        public GameObject Provider => Cb.gameObject;
        public string ProviderName => Cb.CharacterName;
        public float CombatPower => Cb.CombatPower.Result;
        public float Critical => Cb.Critical.Result;
        public float Haste => Cb.Haste.Result;
        public float Hit => Cb.Hit.Result;

        public override bool IsReady => true;
        public override void SetEntity()
        {
            id = SkillData.StatusEffect;
            statusEffectName = MainData.GetStatusEffectData(id).Name;
        }

        private void Reset()
        {
            flag = EntityType.StatusEffect;
            SetEntity();
        }
    }
}
