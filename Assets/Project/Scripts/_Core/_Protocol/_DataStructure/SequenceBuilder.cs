using System;

public class SequenceBuilder
{
    protected readonly Sequencer Sequencer;
    
    public SequenceBuilder(Sequencer holder) => Sequencer = holder;
    
    public SequenceBuilder AddCondition(string key, Func<bool> condition) { Sequencer.Condition.Add(key, condition); return this; }
    public SequenceBuilder Add(Section type, string key, Action action)
    {
        Sequencer[type].Add(key, action);
        
        return this;
    }
    
    public SequenceBuilder RemoveCondition(string key) { Sequencer.Condition.Remove(key); return this; }
    public SequenceBuilder Remove(Section type, string key)
    {
        Sequencer[type].Remove(key);
        
        return this;
    }
    
    /// <summary>
    /// mainSequencer에 Sequencer를 등록안한다.
    /// </summary>
    public SequenceBuilder Register(string key, Sequencer mainSequencer)
    {
        mainSequencer[Section.Active].Add(key, () => Sequencer[Section.Active].Invoke());
        mainSequencer[Section.Cancel].Add(key, () => Sequencer[Section.Cancel].Invoke());
        mainSequencer[Section.Execute].Add(key, () => Sequencer[Section.Execute].Invoke());
        mainSequencer[Section.End].Add(key, () => Sequencer[Section.End].Invoke());

        return this;
    }

    /// <summary>
    /// mainSequencer로 부터 Sequencer를 해제한다.
    /// </summary>
    public SequenceBuilder Unregister(string key, Sequencer mainSequencer)
    {
        mainSequencer[Section.Active].Remove(key);
        mainSequencer[Section.Cancel].Remove(key);
        mainSequencer[Section.Execute].Remove(key);
        mainSequencer[Section.End].Remove(key);
        
        return this;
    }
}

public class SequenceBuilder<T>
{
    protected readonly Sequencer<T> Sequencer;

    public SequenceBuilder(Sequencer<T> holder) => Sequencer = holder;

    public SequenceBuilder<T> AddActiveParam(string key, Action<T> action) { Sequencer.ActiveParamAction.Add(key, action); return this; }
    public SequenceBuilder<T> AddCondition(string key, Func<bool> condition) { Sequencer.Condition.Add(key, condition); return this; }
    public SequenceBuilder<T> Add(Section type, string key, Action action)
    {
        Sequencer[type].Add(key, action);
        
        return this;
    }

    public SequenceBuilder<T> RemoveActiveParam(string key) { Sequencer.ActiveParamAction.Remove(key); return this; }
    public SequenceBuilder<T> RemoveCondition(string key) { Sequencer.Condition.Remove(key); return this; }
    public SequenceBuilder<T> Remove(Section type, string key)
    {
        Sequencer[type].Remove(key);
        
        return this;
    }
}
