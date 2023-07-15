using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.TargetSystem
{
    [Serializable]
    public class TakerDetector
    {
        private enum TargetingType { None = 0, Circle = 1, Self = 2, Cone = 3, }

        [SerializeField] private SortingType sortingType;
        [SerializeField] private int maxBufferCount = 32;
        [SerializeField] private Vector3 sizeVector;
        [SerializeField] private LayerMask targetLayer;

        private ICombatTaker self;
        private ISearchable searchEngine;
        private TargetingType TargetType { get; set; } = TargetingType.None;
        private Collider[] buffers;

        public Vector3 SizeVector => sizeVector;
        public float Distance => sizeVector.x;
        public float Range => sizeVector.y;
        public float Angle => sizeVector.z;
        public float Width => sizeVector.y;
        public float Height => sizeVector.z;
        
        
        public void Initialize(ISearchable searchEngine)
        {
            if (!searchEngine.gameObject.TryGetComponent(out self))
            {
                Debug.LogWarning($"Not Exist ICombatTaker in {searchEngine.gameObject.name}");
            }

            this.searchEngine = searchEngine;
            buffers           = new Collider[maxBufferCount];

            SetTargetType();
        }

        public List<ICombatTaker> GetTakers(bool isAlive = true)
        {
            var result = TargetType switch
            {
                TargetingType.Circle => GetTakersInCircleRange(sizeVector.y),
                TargetingType.Self   => GetSelf(),
                TargetingType.Cone   => GetTakersInAngle(sizeVector.y, sizeVector.z),
                _                    => throw new ArgumentOutOfRangeException()
            };

            if (isAlive)
            {
                result?.RemoveAll(taker => !taker.Alive.Value);
            }

            return result;
        }

        public ICombatTaker GetMainTarget()
        {
            return TargetType switch
            {
                TargetingType.Self => self,
                _                  => GetMainTarget(targetLayer),
            };
        }
        
        public bool HasTarget()
        {
            var takers = GetTakers();

            return !takers.IsNullOrEmpty() 
                   && takers[0].Alive.Value;
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
            
            takersInSphere?.SortingFilter(position, SortingType.DistanceAscending);

            return takersInSphere?[0];
        }
        
        
        private void SetTargetType()
        {
            if (sizeVector is { x: 0.0f, y: 0.0f, z:0.0f })
            {
                TargetType = TargetingType.Self;
            }
            else if (sizeVector.y != 0.0f && Math.Abs(sizeVector.z - 360.0f) > 0.000001f)
            {
                TargetType = TargetingType.Cone;
            }
            else if (sizeVector.y != 0.0f && Math.Abs(sizeVector.z - 360.0f) <= 0.000001f)
            {
                TargetType = TargetingType.Circle;
            }
            else
            {
                TargetType = TargetingType.None;
            }
        }

        private List<ICombatTaker> GetSelf() => new() { self };
        private List<ICombatTaker> GetTakersInSphere(Vector3 center, float radius)
        {
            return TargetUtility.GetTargetsInSphere<ICombatTaker>(center, targetLayer, radius, buffers);
        }

        private List<ICombatTaker> GetTakersInAngle(float radius, float angle) => GetTakersInAngle(searchEngine.gameObject.transform.position, radius, angle);
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
                if (!taker.Alive.Value) continue;
        
                return taker;
            }
        
            return null;
        }


#if UNITY_EDITOR
        public void SetUpAsSkill(DataIndex dataIndex)
        {
            var skillData = Database.SkillSheetData(dataIndex);

            sizeVector  = skillData.TargetParam;
            sortingType = skillData.SortingType.ToEnum<SortingType>();
            targetLayer = LayerMask.GetMask(skillData.TargetLayer);
        }
#endif
    }
}
