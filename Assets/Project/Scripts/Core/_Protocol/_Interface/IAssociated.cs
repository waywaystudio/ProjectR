namespace Core
{
    public interface IAssociated<T>
    {
        T Assignor { get; set; }

        void GetAssignor(T assignor);
    }

    public interface IIntermediary<T> : IAssociated<T>, IAssignor where T : IAssignor
    {
        // T Assignor { get; set; }

        // void GetAssignor(T assignor);
        // void SetAssociation();
    } 

    public interface IAssignor
    {
        void SetAssociation();
    }
    
    
    // public class HostBehaviour : MonoBehaviour, IHost
    // {
    //     public void SetAssociation()
    //     {
    //         GetComponentsInChildren<IAssociated<HostBehaviour>>()
    //             .ForEach(x => x.AssignHost(this));
    //     }
    //
    //     private void Awake()
    //     {
    //         SetAssociation();
    //     }
    // }
    //
    // public class BridgeBehaviour : MonoBehaviour, IHost, IAssociated<HostBehaviour>
    // {
    //     public HostBehaviour Host { get; set; }
    //     
    //     public void AssignHost(HostBehaviour host)
    //     {
    //         Host = GetComponentInParent<HostBehaviour>();
    //         
    //         SetAssociation();
    //     }
    //     
    //     public void SetAssociation()
    //     {
    //         GetComponentsInChildren<IAssociated<BridgeBehaviour>>()
    //             .ForEach(x => x.AssignHost(this));
    //     }
    // }
    //
    // public class AssociatedBehaviour : MonoBehaviour, IAssociated<BridgeBehaviour>
    // {
    //     public BridgeBehaviour Host { get; set; }
    //
    //     public void AssignHost(BridgeBehaviour host)
    //     {
    //         Host = GetComponentInParent<BridgeBehaviour>();
    //     }
    // }
}
