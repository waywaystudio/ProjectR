using System.Collections.Generic;
using System.Linq;
using Wayway.Engine.Events.Core;

namespace Wayway.Engine.Events
{
    public class GameEvent : GameEventCore
    { 
        public List<GameEventListener> EventListenerList 
            => listenerList.Cast<GameEventListener>().ToList();

        public void Invoke() => EventListenerList.ReverseForEach(x => x.Invoke());
    }

    public class GameEvent<T0> : GameEventCore
    {
        public List<GameEventListener<T0>> EventListenerList 
            => listenerList.Cast<GameEventListener<T0>>().ToList();

        public void Invoke(T0 value) => EventListenerList.ReverseForEach(x => x.Invoke(value));
    }
    
    public class GameEvent<T0, T1> : GameEventCore
    {
        public List<GameEventListener<T0, T1>> EventListenerList 
            => listenerList.Cast<GameEventListener<T0, T1>>().ToList();

        public void Invoke(T0 value1, T1 value2) => EventListenerList.ReverseForEach(x => x.Invoke(value1, value2));
    }
}
