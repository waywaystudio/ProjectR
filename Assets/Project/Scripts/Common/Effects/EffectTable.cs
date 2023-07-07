using System;
using System.Collections.Generic;
using Common.Particles;
using UnityEngine;

namespace Common.Effects
{
    [Serializable]
    public class EffectTable : Dictionary<IActionSender, ParticleComponent>
    {
        // private Table<IActionSender, ParticlePlayer> Table { get; set; } = new();
        // public void Add(IActionSender actionSender, ParticlePlayer particle) => Table.Add(actionSender, particle);
        // public void Remove(IActionSender actionSender) => Table.Remove(actionSender);

        public void Play(IActionSender actionSender, ParticleComponent particle, Transform transform)
        {
            if (!ContainsKey(actionSender))
            {
                Add(actionSender, particle);
            }
            
            // TODO.TEMP
            var position = transform.position;
                
            this[actionSender].Play(position, transform);
        }

        public void Stop(IActionSender actionSender)
        {
            if (ContainsKey(actionSender))
            {
                this[actionSender].Stop();
            }
        }
    }
}
