using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Detects
{
    [Serializable]
    public class TakerDetector
    {
        [SerializeField] private TargetingType targetType;
        [SerializeField] private SortingType sortingType;
        [SerializeField] private SizeEntity sizeEntity;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private LayerMask damageLayer;

        private ICombatTaker self;
        private ISearchable searchEngine;
        private List<int> preAllocatedLayerIndex;
        private List<ICombatTaker> preAllocatedSelf;
        private List<ICombatTaker> mainTargetBuffer;

        public ITakerDetection TypeDetector { get; set; }
        public ICombatTaker ForceTaker { get; set; }
        public SizeEntity SizeEntity => sizeEntity;
        

        public void Initialize(ISearchable searchEngine)
        {
            if (!searchEngine.gameObject.TryGetComponent(out self))
            {
                Debug.LogWarning($"Not Exist ICombatTaker in {searchEngine.gameObject.name}");
            }

            this.searchEngine      = searchEngine;
            preAllocatedSelf       = new List<ICombatTaker> { self };
            mainTargetBuffer       = new List<ICombatTaker>();
            preAllocatedLayerIndex = targetLayer.ToIndexList();
            TypeDetector ??= targetType switch
            {
                TargetingType.Circle       => new CircleDetector(),
                TargetingType.Cone         => new AngleDetector(),
                TargetingType.Rect         => new RectDetector(),
                TargetingType.Raycast      => new RaycastDetector(),
                TargetingType.RaycastWidth => new BoxCastDetector(),
                TargetingType.Donut => new DonutDetector(),
                // TargetingType.Custom => gameObject.GetComponent
                // TargetingType.None         => expr,
                // TargetingType.Self         => expr,
                // TargetingType.Line         => expr,
                _ => null,
            };
        }


        /// <summary>
        /// Taker Detector에 미리 저장된 설정 값을 바탕으로 대상을 받는다.
        /// </summary>
        public bool TryGetTakers(out List<ICombatTaker> takers)
        {
            takers = GetTakers();

            return takers.HasElement();
        }
        
        
        /// <summary>
        /// 원거리 대상 지정형 스킬에서 사용
        /// </summary>
        /// <param name="center">일반적으로 마우스 포인터 위치</param>
        public bool TryGetTakersInCircle(Vector3 center, float radius, Collider[] buffers, out List<ICombatTaker> takers)
        {
            takers = GetTakersInCircleRange(center, radius, buffers);

            return takers.HasElement();
        }
        

        public ICombatTaker GetMainTarget()
        {
            return targetType == TargetingType.Self 
                ? self 
                : MainTargetFilter();
        }


        private List<ICombatTaker> GetTakers()
        {
            return targetType == TargetingType.Self 
                ? preAllocatedSelf 
                : TypeDetector?.GetTakers(searchEngine.Position, searchEngine.Forward, damageLayer, sizeEntity);
        }

        private List<ICombatTaker> GetTakersInCircleRange(Vector3 center, float radius, Collider[] buffers)
        {
            return TargetUtility.GetTargetsInSphere<ICombatTaker>(center, damageLayer, radius, buffers);
        }

        private ICombatTaker MainTargetFilter()
        {
            // Force 타겟팅 하는 과정에서 현재 스킬과 레이어가 다르면 무시하는 라인 필요.
            if (ForceTaker is not null && 
                ForceTaker.Alive.Value &&
                ForceTaker.gameObject.IsInLayerMask(targetLayer))
                return ForceTaker;
            
            var searchTable = searchEngine.SearchedTable;
            
            mainTargetBuffer.Clear();
            preAllocatedLayerIndex.ForEach(index =>
            {
                mainTargetBuffer.AddRange(TargetUtility.ConvertTargets<ICombatTaker>(searchTable[index]));
            });

            if (mainTargetBuffer.IsNullOrEmpty()) return null;

            var pivot = searchEngine.gameObject.transform.position;

            mainTargetBuffer.Sort(pivot, sortingType);
            
            foreach (var taker in mainTargetBuffer)
            {
                if (!taker.Alive.Value) continue;
        
                return taker;
            }
        
            return null;
        }


#if UNITY_EDITOR
        public void SetUpAsSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);

            targetType  = skillData.TargetType.ToEnum<TargetingType>();
            sortingType = skillData.SortingType.ToEnum<SortingType>();
            sizeEntity.Set(skillData.TargetParam);
            
            var layerNamesSplit = skillData.TargetLayer.Split(',');
            var layerNames = new string[layerNamesSplit.Length];

            for (var i = 0; i < layerNamesSplit.Length; i++) {
                layerNames[i] = layerNamesSplit[i].Trim();
            }

            targetLayer = LayerMask.GetMask(layerNames);
            
            layerNamesSplit = skillData.DamagetLayer.Split(',');
            layerNames = new string[layerNamesSplit.Length];

            for (var i = 0; i < layerNamesSplit.Length; i++) {
                layerNames[i] = layerNamesSplit[i].Trim();
            }

            damageLayer = LayerMask.GetMask(layerNames);
        }
#endif
    }
}
