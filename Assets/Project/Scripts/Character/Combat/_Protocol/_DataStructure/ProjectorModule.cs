using Core;
using UnityEngine;

namespace Character.Combat
{
    using Projector;
    
    public class ProjectorModule : CombatModule
    {
        [SerializeField] private DataIndex projectorID;
        [SerializeField] private GameObject projectorPrefab;
        
        public ICombatProvider Provider => CombatObject.Provider;


        public void Projection(ICombatTaker taker)
        {
            // Pooling by projectorID
            var cbPosition = Provider.Object.transform.position;
            var tkPosition = taker.Object.transform.position;
            var lookAt = Quaternion.LookRotation(tkPosition - cbPosition);
            var newProjection = Instantiate(projectorPrefab, tkPosition, lookAt);
            //
            
            newProjection.TryGetComponent(out ProjectorObject po);
            po.Projection(Provider, taker);
        }

        public void Projection(Vector3 position)
        {
            // Pooling by projectorID
            var cbPosition = Provider.Object.transform.position;
            var lookAt = Quaternion.LookRotation(position - cbPosition);
            var newProjection = Instantiate(projectorPrefab, position, lookAt);
            //
            
            newProjection.TryGetComponent(out ProjectorObject po);
            po.Projection(Provider, position);
        }


#if UNITY_EDITOR
        public void SetUpValue(int projectorID)
        {
            Flag             = ModuleType.Projector;
            this.projectorID = (DataIndex)projectorID;
        }
#endif
    }
}
