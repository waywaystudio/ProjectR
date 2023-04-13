namespace Serialization
{
    
    /* 
     * Exactly Same as SaveListener, but Script Execution Order is negative.
     * if serializable Data require preload, Use PreLoadListener Instead SaveListener
     */
    public class PreLoadListener : SaveListener { }
}
