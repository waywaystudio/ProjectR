using UnityEngine;

namespace Common.Projectors
{
    public class Projection : MonoBehaviour
    {
        [SerializeField] protected TargetingType targetingType;

        public string InstanceKey { get; set; }
        public IProjection Provider { get; protected set; }
        public TargetingType TargetingType => targetingType;
        public Sequencer Sequencer { get; } = new();
        public SequenceBuilder Builder { get; protected set; }


        public void Initialize(IProjection provider)
        {
            Provider    = provider;
            InstanceKey = GetInstanceID().ToString();

            Builder = new SequenceBuilder(Sequencer);
            Builder.Register($"{InstanceKey}.Projection", Provider.Sequence);

            var associates = GetComponentsInChildren<IAssociate<Projection>>();
            
            associates?.ForEach(associate => associate.Initialize(this));
        }
    }
}
