using System;
using DG.Tweening;
using Singleton;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoSingleton<PostProcessingManager>
{
    [SerializeField] private Volume globalVolume;
    [SerializeField] private float defaultVignetteIntensity = 0.13f;
    [SerializeField] private float vignetteResetDuration = 0.15f;
    
    private static Vignette vignette;
    private static Tween vignetteTween;
    private static float VignetteResetDuration => Instance.vignetteResetDuration;

    /*
     * Vignette
     */
    public static void Vignetting(float intensity, float duration)
    {
        vignetteTween = DOTween.To(() => vignette.intensity.value, 
                                x => vignette.intensity.value = x, 
                                intensity, 
                                duration);
    }

    public static void ResetVignetting()
    {
        if (vignetteTween != null)
        {
            vignetteTween.Kill();
            vignetteTween = null;
        }

        if (Math.Abs(vignette.intensity.value - VignetteResetDuration) < 0.0001f) return;

        vignetteTween = DOTween.To(() => vignette.intensity.value, 
                                   x => vignette.intensity.value = x, 
                                   Instance.defaultVignetteIntensity, 
                                   VignetteResetDuration);
    }
    
    /*
     * Bloom
     */
    public static void Blooming(float intensity, float duration)
    {
        
    }

    public static void ResetBlooming()
    {
        
    }

    protected override void Awake()
    {
        base.Awake();

        globalVolume.profile.TryGet(out vignette);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (vignetteTween != null)
        {
            vignetteTween.Kill();
            vignetteTween = null;
        }
    }
    
    

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ResetSingleton()
    {
        if (!Instance.IsNullOrDestroyed())
            Instance.SetInstanceNull();
    }
}
