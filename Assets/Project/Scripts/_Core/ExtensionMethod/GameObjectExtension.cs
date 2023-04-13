using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
// ReSharper disable UnusedMember.Global

public static class GameObjectExtension
{
    public static void DestroyAllChildren(this GameObject gameObject, bool includeInactive = false)
    {
        foreach (Transform child in gameObject.GetComponentInChildren<Transform>(includeInactive))
        {
            Object.Destroy(child.gameObject);
        }
    }
    public static void DestroyImmediateAllChildren(this GameObject gameObject, bool includeInactive = false)
    {
        foreach (Transform child in gameObject.GetComponentInChildren<Transform>(includeInactive))
        {
            Object.DestroyImmediate(child.gameObject);
        }
    }

    public static T GetComponentOnlyChildren<T>(this GameObject gameObject) where T : MonoBehaviour
    {
        return gameObject.GetComponentsInChildren<T>().First(x => x.gameObject != gameObject);
    }

    public static List<T> GetComponentsInOnlyChildren<T>(this GameObject gameObject)
    {
        var selfBehaviour = gameObject.GetComponent<T>();
        var result        = new List<T>();

        gameObject.GetComponentsInChildren(result);

        result.Remove(selfBehaviour);

        return result;
    }
        
    public static IEnumerator Moving(this GameObject gameObject, Vector3 a, Vector3 b, Action postMove = null)
    {
        var elapsedTime  = .0f;
        var expectedTime = (b - a).magnitude * 2f / Mathf.Sqrt(5f);

        while (elapsedTime < expectedTime)
        {
            elapsedTime += Time.deltaTime;
            var t = elapsedTime / expectedTime;

            gameObject.transform.position = Vector3.Lerp(a, b, t);

            yield return null;
        }

        postMove?.Invoke();
    }

    public static bool IsNullOrEmpty(this GameObject gameObject)
    {
        return gameObject == null || !gameObject.activeSelf;
    }
        
    public static bool IsNullOrDestroyed(this object obj)
    {
        return obj switch
        {
            null     => true,
            Object o => o == null,
            _        => false
        };
    }

    public static bool HasObject(this GameObject gameObject)
    {
        return gameObject is not null && gameObject.activeSelf;
    }

    public static bool IsNullOrEmpty(this Component component)
    {
        return Equals(component, null) || component.gameObject.activeSelf == false;
    }

    public static bool TryGetComponent<T>(this GameObject gameObject, out T result, bool showMessageOnFailed) where T : Component
    {
        if (gameObject.TryGetComponent(out result)) return true;
        if (!showMessageOnFailed) return true;
        
        Debug.LogWarning($"Can't Find TypeOf:{typeof(T)} in {gameObject.name}.");
        return false;
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : MonoBehaviour
    {
        var component = gameObject.GetComponent<T>();
        if (component == null) gameObject.AddComponent<T>();
        return component;
    }
        
    public static bool HasComponent<T>(this GameObject gameObject) where T : MonoBehaviour
    {
        return gameObject.GetComponent<T>() != null;
    }
    
    public static void TrimAndDestroy<T>(this List<T> list, int maxIndex) where T : MonoBehaviour
    {
        // Check if the specified index is within the bounds of the list
        if (maxIndex < 0 || maxIndex >= list.Count)
        {
            return;
        }

        // Destroy GameObjects with index larger than or equal to maxIndex
        for (var i = list.Count - 1; i >= maxIndex; i--)
        {
            var go = list[i].gameObject;
            list.RemoveAt(i);
            if (go != null)
            {
                Object.Destroy(go);
            }
        }
    }
}