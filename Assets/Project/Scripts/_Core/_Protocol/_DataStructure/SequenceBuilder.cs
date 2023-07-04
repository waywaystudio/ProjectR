using System;

public class SequenceBuilder
{
    protected readonly Sequencer Sequencer;

    public SequenceBuilder(IHasSequencer holder) => Sequencer = holder?.Sequencer;
    public SequenceBuilder(Sequencer holder) => Sequencer = holder;
    
    public SequenceBuilder AddCondition(string key, Func<bool> condition) { Sequencer.Condition.Add(key, condition); return this; }
    public SequenceBuilder AddTrigger(Func<bool> condition, Action callback) { Sequencer.CompleteTrigger = new WaitTrigger(condition, callback); return this; }
    public SequenceBuilder Add(SectionType type, string key, Action action)
    {
        Sequencer[type].Add(key, action);
        
        return this;
    }
    
    public SequenceBuilder RemoveCondition(string key) { Sequencer.Condition.Remove(key); return this; }
    public SequenceBuilder RemoveTrigger() { Sequencer.CompleteTrigger = null; return this; }
    public SequenceBuilder Remove(SectionType type, string key)
    {
        Sequencer[type].Remove(key);
        
        return this;
    }
    
    /// <summary>
    /// mainSequencer에 Sequencer를 등록한다.
    /// </summary>
    public SequenceBuilder Register(string key, Sequencer mainSequencer)
    {
        mainSequencer[SectionType.Active].Add(key, () => Sequencer[SectionType.Active].Invoke());
        mainSequencer[SectionType.Cancel].Add(key, () => Sequencer[SectionType.Cancel].Invoke());
        mainSequencer[SectionType.Complete].Add(key, () => Sequencer[SectionType.Complete].Invoke());
        mainSequencer[SectionType.End].Add(key, () => Sequencer[SectionType.End].Invoke());

        return this;
    }

    /// <summary>
    /// mainSequencer로 부터 Sequencer를 해제한다.
    /// </summary>
    public SequenceBuilder Unregister(string key, Sequencer mainSequencer)
    {
        mainSequencer[SectionType.Active].Remove(key);
        mainSequencer[SectionType.Cancel].Remove(key);
        mainSequencer[SectionType.Complete].Remove(key);
        mainSequencer[SectionType.End].Remove(key);
        
        return this;
    }
}

public class SequenceBuilder<T>
{
    protected readonly Sequencer<T> Sequencer;

    public SequenceBuilder(Sequencer<T> holder) => Sequencer = holder;

    public SequenceBuilder<T> AddActiveParam(string key, Action<T> action) { Sequencer.ActiveParamAction.Add(key, action); return this; }
    public SequenceBuilder<T> AddCondition(string key, Func<bool> condition) { Sequencer.Condition.Add(key, condition); return this; }
    public SequenceBuilder<T> AddTrigger(Func<bool> condition, Action callback) { Sequencer.CompleteTrigger = new WaitTrigger(condition, callback); return this; }
    public SequenceBuilder<T> Add(SectionType type, string key, Action action)
    {
        Sequencer[type].Add(key, action);
        
        return this;
    }
    
    public SequenceBuilder<T> AddExecution(string key, Action action)
    {
        Sequencer[SectionType.Execute].Add(key, action);
        return this;
    }
    
    public SequenceBuilder<T> RemoveActiveParam(string key) { Sequencer.ActiveParamAction.Remove(key); return this; }
    public SequenceBuilder<T> RemoveCondition(string key) { Sequencer.Condition.Remove(key); return this; }
    public SequenceBuilder<T> RemoveTrigger() { Sequencer.CompleteTrigger = null; return this; }
    public SequenceBuilder<T> Remove(SectionType type, string key)
    {
        Sequencer[type].Remove(key);
        
        return this;
    }
}
