using System;
using System.Collections.Generic;
using Common.Characters;
using UnityEngine;

namespace Common.Systems
{
    [Serializable]
    public class Detector
    {
        private enum TargetingType { None = 0, Circle = 1, Self = 2,}

        [SerializeField] private TargetingType targetType = TargetingType.Circle;
        [SerializeField] private SortingType sortingType;
        [SerializeField] private int maxBufferCount = 32;
        [SerializeField] private Vector2 sizeVector;
        [SerializeField] private LayerMask targetLayer;

        private Collider[] colliderBuffers = new Collider[32];
        private RaycastHit[] rayBuffers = new RaycastHit[32];
        private CharacterBehaviour character;

        public float Range => sizeVector.x;
        public float Angle => sizeVector.y;
        public float Width => sizeVector.x;
        public float Height => sizeVector.y;
        

        public void Initialize(CharacterBehaviour character)
        {
            this.character  = character;
            
            colliderBuffers = new Collider[maxBufferCount];
            rayBuffers      = new RaycastHit[maxBufferCount];
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
                TargetingType.Self => character,
                _                  => character.Searching.GetMainTarget(targetLayer, character.transform.position, sortingType),
            };
        }

        public List<ICombatTaker> GetTakersInCircleRange(float radius) => GetTakersInCircleRange(radius, Angle);
        public List<ICombatTaker> GetTakersInCircleRange(float radius, float angle) => GetTakersInCircleRange(character.transform.position, radius, angle);
        public List<ICombatTaker> GetTakersInCircleRange(Vector3 center, float radius, float angle)
        {
            var takerList = GetTakersInSphereType(
                center,
                radius,
                angle,
                targetLayer);

            return takerList;
        }
        

        private List<ICombatTaker> GetSelf() => new() { character };
        private List<ICombatTaker> GetTakersInSphereType(Vector3 center, float radius, float angle, LayerMask layer)
        {
            if (Physics.OverlapSphereNonAlloc(center, radius, colliderBuffers, layer) == 0) return null;
        
            var result = new List<ICombatTaker>();
            
            colliderBuffers.ForEach(collider =>
            {
                if (collider.IsNullOrEmpty() || !collider.TryGetComponent(out ICombatTaker taker)) return;
                
                if (Mathf.Abs(angle - 360.0f) > 0.000001f)
                {
                    var direction = (collider.transform.position - center).normalized;
        
                    if (Vector3.Angle(character.transform.forward, direction) > angle * 0.5f) return;
                }
                
                result.Add(taker);
            });
        
            return result;
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

        // protected bool TryGetTakersByRayCast(out List<ICombatTaker> takerList)
        // {
        //     var providerTransform = Cb.transform;
        //
        //     return Cb.Colliding.TryGetTakersByRaycast(
        //         providerTransform.position, 
        //         providerTransform.forward, range, 16,
        //         targetLayer, out takerList);
        // }
    }
}
