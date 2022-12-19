using System;
using System.Collections.Generic;
using Common.Character;
using Common.Character.Operation.Combating;
using Common.Character.Operation.Combating.StatusEffects;
using Core;
using UnityEngine;


[Serializable]
public class StatusEffects : MonoBehaviour
{
    public CharacterBehaviour Cb => GetComponentInParent<CharacterBehaviour>();
    
    public Dictionary<string, StatusEffect> StatusEffectTable = new ();

    public void TryAddStatusEffect(string key, ICombatProvider provider)
    {
        if (StatusEffectTable.ContainsKey(key)) return;

        StatusEffect statusEffect = null;
        
        switch (key)
        {
            case "Corruption":
            {
                statusEffect = AddCorruption(provider);
                break;
            }
        }

        StatusEffectTable.TryAdd(key, statusEffect);
    }

    public Corruption AddCorruption(ICombatProvider provider)
    {
        var corruption = new Corruption(Cb, provider);
        corruption.InvokeRoutine = StartCoroutine(corruption.Invoke());

        return corruption;
    }

    public bool TryDispel(string key)
    {
        if (!StatusEffectTable.TryGetValue(key, out var statusEffect)) return false;

        // TODO. 이렇게 하는거 아니다.
        // TODO. 수정했다...근데 맞나? 테스트해봐야 한다.
        if (statusEffect.InvokeRoutine != null)
        {
            StopCoroutine(statusEffect.InvokeRoutine);
        }

        StatusEffectTable.TryRemove(key);

        return true;
    }

    private void Awake()
    {
        Cb.OnTakeStatusEffect.Register(GetInstanceID(), TryAddStatusEffect);
    }
}
