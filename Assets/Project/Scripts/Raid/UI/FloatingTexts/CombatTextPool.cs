using System.Collections.Generic;
using Character.Venturers;
using Common;
using UnityEngine;

namespace Raid.UI.FloatingTexts
{
    public class CombatTextPool : MonoBehaviour
    {
        [Sirenix.OdinInspector.ListDrawerSettings]
        [SerializeField] private List<FloatingTextEntity> textEntities;
        [SerializeField] private Pool<FloatingText> pool;
        
        private VenturerBehaviour currentVenturer;
        private Table<CombatEntityType, FloatingTextEntity> EntityTable { get; } = new();


        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            RemoveText();

            if (vb == null) return;

            currentVenturer = vb;
            currentVenturer.OnCombatProvided.Add("SpawnCombatText", SpawningOnProvide);
            currentVenturer.OnCombatTaken.Add("SpawnCombatText", Spawn);
        }

        public void OnCommandMode()
        {
            RemoveText();
        }


        private void Create(FloatingText ft)
        {
            ft.OnEnd.Add("ReleasePool", () => pool.Release(ft));
        }

        /// <summary>
        /// 동일 대상에게 Heal을 주고 받을 때 어색한 것을 방지
        /// (ICombatTaker)Downcast 필요.
        /// </summary>
        private void SpawningOnProvide(CombatEntity combatEntity)
        {
            // ReSharper disable once RedundantCast
            if (combatEntity.Taker != (ICombatTaker)currentVenturer) Spawn(combatEntity);
        }
        
        private void Spawn(CombatEntity combatEntity)
        {
            var ft = pool.Get();
            var textDesignEntity = EntityTable[combatEntity.Type];
            var text = combatEntity.Type switch
            {
                CombatEntityType.Damage         => combatEntity.Value.ToString("0"),
                CombatEntityType.CriticalDamage => combatEntity.Value.ToString("0"),
                CombatEntityType.Heal           => combatEntity.Value.ToString("0"),
                CombatEntityType.CriticalHeal   => combatEntity.Value.ToString("0"),
                CombatEntityType.Evade          => "Evade",
                CombatEntityType.Block          => "Block",
                CombatEntityType.Absorb         => "Absorb",
                CombatEntityType.Immune         => "Immune",
                _                               => "Miss"
            };

            ft.ImportProperty(textDesignEntity, text);
            ft.ShowValue(combatEntity);
        }

        private void RemoveText()
        {
            if (currentVenturer == null) return;
            
            currentVenturer.OnCombatTaken.Remove("SpawnCombatText");
            currentVenturer.OnCombatProvided.Remove("SpawnCombatText");
        }

        private void Awake()
        {
            pool.Initialize(Create, transform);
            EntityTable.CreateTable(textEntities, entity => entity.CombatEntityType);
        }
    }
}
