using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.TargetSystem
{
    [Serializable]
    public class CombatTakerDetector
    {
        private enum TargetingType { None = 0, Circle = 1, Self = 2,}

        [SerializeField] private TargetingType targetType = TargetingType.Circle;
        [SerializeField] private SortingType sortingType;
        [SerializeField] private int maxBufferCount = 32;
        [SerializeField] private Vector2 sizeVector;
        [SerializeField] private LayerMask targetLayer;

        private ICombatTaker self;
        private ISearchable searchEngine;
        private Collider[] buffers;

        public float Range => sizeVector.x;
        public float Angle => sizeVector.y;
        public float Width => sizeVector.x;
        public float Height => sizeVector.y;
        
        
        public void Initialize(ISearchable searchEngine)
        {
            if (!searchEngine.gameObject.TryGetComponent(out self))
            {
                Debug.LogWarning($"Not Exist ICombatTaker in {searchEngine.gameObject.name}");
            }

            this.searchEngine = searchEngine;
            buffers           = new Collider[maxBufferCount];
        }

        public List<ICombatTaker> GetTakers()
        {
            return targetType switch
            {
                TargetingType.Circle => GetTakersInCircleRange(sizeVector.x),
                TargetingType.Self   => GetSelf(),
                _                    => throw new ArgumentOutOfRangeException()
            };
        }

        public ICombatTaker GetMainTarget()
        {
            return targetType switch
            {
                TargetingType.Self => self,
                _                  => GetMainTarget(targetLayer),
            };
        }

        public List<ICombatTaker> GetTakersInCircleRange(float radius) => GetTakersInCircleRange(radius, Angle);
        public List<ICombatTaker> GetTakersInCircleRange(float radius, float angle) => GetTakersInCircleRange(searchEngine.gameObject.transform.position, radius, angle);
        public List<ICombatTaker> GetTakersInCircleRange(Vector3 center, float radius, float angle)
        {
            var takerList = GetTakersInAngle(
                center,
                radius,
                angle);

            return takerList;
        }
        
        public ICombatTaker GetNearestTarget(Vector3 position, float radius)
        {
            var takersInSphere = GetTakersInSphere(position, radius);
            
            takersInSphere.SortingFilter(position, SortingType.DistanceAscending);

            return takersInSphere?[0];
        }
        

        private List<ICombatTaker> GetSelf() => new() { self };
        private List<ICombatTaker> GetTakersInSphere(Vector3 center, float radius)
        {
            return TargetUtility.GetTargetsInSphere<ICombatTaker>(center, targetLayer, radius, buffers);
        }
        
        private List<ICombatTaker> GetTakersInAngle(Vector3 center, float radius, float angle)
        {
            var forward = searchEngine.gameObject.transform.forward;

            return TargetUtility.GetTargetsInAngle<ICombatTaker>(center, forward, targetLayer, radius, angle, buffers);
        }

        private ICombatTaker GetMainTarget(LayerMask targetLayerMask)
        {
            var searchTable = searchEngine.SearchedTable;
            var layerIndex = targetLayerMask.ToIndex();
            
            var combatTakerList = TargetUtility.ConvertTargets<ICombatTaker>(searchTable[layerIndex]);
        
            if (combatTakerList.IsNullOrEmpty()) return null;

            var pivot = searchEngine.gameObject.transform.position;

            combatTakerList.SortingFilter(pivot, sortingType);
            
            foreach (var taker in combatTakerList)
            {
                if (!taker.DynamicStatEntry.Alive.Value) continue;
        
                return taker;
            }
        
            return null;
        }


#if UNITY_EDITOR
        public void SetUpAsSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);

            targetType  = skillData.DetectorType.ToEnum<TargetingType>();
            sizeVector  = skillData.TargetParam;
            sortingType = skillData.SortingType.ToEnum<SortingType>();
            targetLayer = LayerMask.GetMask(skillData.TargetLayer);
        }
#endif
    }
}
