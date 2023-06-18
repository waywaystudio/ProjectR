using System;

public class SequenceBuilder
{
    public bool IsInitialized;
    protected Sequencer Sequencer;

    public Sequencer Build() => Sequencer;
    public SequenceBuilder Initialize(Sequencer sequencer)
    {
        IsInitialized = true;
        Sequencer     = sequencer; 
        
        return this;
    }
    public SequenceBuilder AddCondition(string key, Func<bool> condition) { Sequencer.Condition.Add(key, condition); return this; }
    public SequenceBuilder AddTrigger(Func<bool> condition, Action callback) { Sequencer.CompleteTrigger = new WaitTrigger(condition, callback); return this; }
    public SequenceBuilder Add(SectionType type, string key, Action action)
    {
        Sequencer[type].Add(key, action);
        // GetActionTable(type).Add(key, action);
        
        return this;
    }
    
    public SequenceBuilder RemoveCondition(string key) { Sequencer.Condition.Remove(key); return this; }
    public SequenceBuilder RemoveTrigger() { Sequencer.CompleteTrigger = null; return this; }
    public SequenceBuilder Remove(SectionType type, string key)
    {
        Sequencer[type].Remove(key);
        // GetActionTable(type).Remove(key);
        
        return this;
    }


    // public SequenceBuilder AddActive(string key, Action action) => Add(SectionType.Active, key, action);
    // public SequenceBuilder AddCancel(string key, Action action) => Add(SectionType.Cancel, key, action);
    // public SequenceBuilder AddComplete(string key, Action action) => Add(SectionType.Complete, key, action);
    // public SequenceBuilder AddEnd(string key, Action action) => Add(SectionType.End, key, action);
    // public SequenceBuilder RemoveActive(string key) => Remove(SectionType.Active, key);
    // public SequenceBuilder RemoveCancel(string key) => Remove(SectionType.Cancel, key);
    // public SequenceBuilder RemoveComplete(string key) => Remove(SectionType.Complete, key);
    // public SequenceBuilder RemoveEnd(string key) => Remove(SectionType.End, key);
    //
    // protected ActionTable GetActionTable(SectionType type) => type switch
    // {
    //
    //     SectionType.Active => Sequencer.ActiveAction,
    //     SectionType.Cancel     => Sequencer.CancelAction,
    //     SectionType.Complete   => Sequencer.CompleteAction,
    //     SectionType.End        => Sequencer.EndAction,
    //     _                      => null,
    // };
}

public class SequenceBuilder<T>
{
    public bool IsInitialized;
    protected Sequencer<T> Sequencer;

    public Sequencer<T> Build() => Sequencer;
    public SequenceBuilder<T> Initialize(Sequencer<T> sequencer)
    {
        IsInitialized = true;
        Sequencer     = sequencer;
        
        return this;
    }

    public SequenceBuilder<T> AddCondition(string key, Func<bool> condition) { Sequencer.Condition.Add(key, condition); return this; }
    public SequenceBuilder<T> AddTrigger(Func<bool> condition, Action callback) { Sequencer.CompleteTrigger = new WaitTrigger(condition, callback); return this; }
    public SequenceBuilder<T> AddActiveParam(string key, Action action) { Sequencer.ActiveParamAction.Add(key, action); return this; }
    public SequenceBuilder<T> AddActiveParam(string key, Action<T> action) { Sequencer.ActiveParamAction.Add(key, action); return this; }
    public SequenceBuilder<T> Add(SectionType type, string key, Action action)
    {
        Sequencer[type].Add(key, action);
        // GetActionTable(type).Add(key, action);
        
        return this;
    }
    
    public SequenceBuilder<T> RemoveCondition(string key) { Sequencer.Condition.Remove(key); return this; }
    public SequenceBuilder<T> RemoveTrigger() { Sequencer.CompleteTrigger = null; return this; }
    public SequenceBuilder<T> RemoveActiveParam(string key) { Sequencer.ActiveParamAction.Remove(key); return this; }
    public SequenceBuilder<T> Remove(SectionType type, string key)
    {
        Sequencer[type].Remove(key);
        // GetActionTable(type).Remove(key);
        
        return this;
    }
    
    // public SequenceBuilder<T> AddActive(string key, Action action) => Add(SectionType.Active, key, action);
    // public SequenceBuilder<T> AddCancel(string key, Action action) => Add(SectionType.Cancel, key, action);
    // public SequenceBuilder<T> AddComplete(string key, Action action) => Add(SectionType.Complete, key, action);
    // public SequenceBuilder<T> AddEnd(string key, Action action) => Add(SectionType.End, key, action);
    // public SequenceBuilder<T> RemoveActive(string key) => Remove(SectionType.Active, key);
    // public SequenceBuilder<T> RemoveCancel(string key) => Remove(SectionType.Cancel, key);
    // public SequenceBuilder<T> RemoveComplete(string key) => Remove(SectionType.Complete, key);
    // public SequenceBuilder<T> RemoveEnd(string key) => Remove(SectionType.End, key);

    // protected virtual ActionTable GetActionTable(SectionType type) => type switch
    // {
    //     SectionType.Active   => Sequencer.ActiveAction,
    //     SectionType.Cancel   => Sequencer.CancelAction,
    //     SectionType.Complete => Sequencer.CompleteAction,
    //     SectionType.End      => Sequencer.EndAction,
    //     _                    => null,
    // };
}