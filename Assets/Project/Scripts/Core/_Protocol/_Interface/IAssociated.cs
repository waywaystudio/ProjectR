namespace Core
{
    public interface IAssociated<T>
    {
        T Host { get; set; }

        void AssignHost();
    }

    // public interface IHost
    // {
    //     void SetAssociation();
    // }
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
