using System;
using UnityEngine.Events;

public static class EventExtension
{
    public static void AddPersistantListenerInEditor(this UnityEvent unityEvent, object targetObject, string methodName)
    {
        var activeAction = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), targetObject, methodName);
        
        UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent, activeAction);
    }

    public static void ClearUnityEventInEditor(this UnityEvent unityEvent)
    {
        for (var i = unityEvent.GetPersistentEventCount() - 1; i >= 0; i--) 
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent, i);
    }
}
