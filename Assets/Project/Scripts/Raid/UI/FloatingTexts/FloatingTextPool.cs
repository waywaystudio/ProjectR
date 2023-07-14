using System.Collections.Generic;
using Character.Venturers;
using Common;
using Common.UI.FloatingTexts;
using UnityEngine;

namespace Raid.UI.FloatingTexts
{
    public class FloatingTextPool : MonoBehaviour
    {
        [Sirenix.OdinInspector.ListDrawerSettings]
        [SerializeField] private List<FloatingTextEntity> textEntities;
        [SerializeField] private Pool<FloatingText> pool;
        
        private VenturerBehaviour currentVenturer;
        private Table<CombatEntityType, FloatingTextEntity> EntityTable { get; } = new();


        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            if (currentVenturer != null)
            {
                currentVenturer.OnCombatTaken.Remove("SpawnCombatText");
                currentVenturer.OnCombatProvided.Remove("SpawnCombatText");
            }

            currentVenturer = vb;
            currentVenturer.OnCombatProvided.Add("SpawnCombatText", SpawningOnProvide);
            currentVenturer.OnCombatTaken.Add("SpawnCombatText", Spawn);
        }


        /// <summary>
        /// 동일 대상에게 Heal을 주고 받을 때 어색한 것을 방지
        /// </summary>
        private void Create(FloatingText ft)
        {
            ft.OnEnd.Add("ReleasePool", () => pool.Release(ft));
        }

        private void SpawningOnProvide(CombatEntity combatEntity)
        {
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

        private void Awake()
        {
            pool.Initialize(Create, transform);
            EntityTable.CreateTable(textEntities, entity => entity.CombatEntityType);
        }
    }
}
