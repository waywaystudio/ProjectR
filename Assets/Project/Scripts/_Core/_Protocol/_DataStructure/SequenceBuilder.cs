using System;

public class SequenceBuilder
{
    private Sequencer sequencer;

    public SequenceBuilder Initialize(Sequencer sequencer)
    {
        this.sequencer = sequencer;
        return this;
    }
    
    public Sequencer Build() => sequencer;

    public SequenceBuilder AddCondition(string key, Func<bool> condition)
    {
        sequencer.Condition.Add(key, condition);
        return this;
    }
    
    public SequenceBuilder RemoveCondition(string key)
    {
        sequencer.Condition.Remove(key);
        return this;
    }
    
    public SequenceBuilder AddActive(string key, Action action) => Add(SectionType.Active, key, action);
    public SequenceBuilder AddCancel(string key, Action action) => Add(SectionType.Cancel, key, action);
    public SequenceBuilder AddComplete(string key, Action action) => Add(SectionType.Complete, key, action);
    public SequenceBuilder AddEnd(string key, Action action) => Add(SectionType.End, key, action);
    public SequenceBuilder RemoveActive(string key) => Remove(SectionType.Active, key);
    public SequenceBuilder RemoveCancel(string key) => Remove(SectionType.Cancel, key);
    public SequenceBuilder RemoveComplete(string key) => Remove(SectionType.Complete, key);
    public SequenceBuilder RemoveEnd(string key) => Remove(SectionType.End, key);
    
    private SequenceBuilder Add(SectionType type, string key, Action action)
    {
        GetActionTable(type).Add(key, action);
        return this;
    }
    
    private SequenceBuilder Remove(SectionType type, string key)
    {
        GetActionTable(type).Remove(key);
        return this;
    }

    private ActionTable GetActionTable(SectionType type) => type switch
    {

        SectionType.Active => sequencer.ActiveAction,
        SectionType.Cancel     => sequencer.CancelAction,
        SectionType.Complete   => sequencer.CompleteAction,
        SectionType.End        => sequencer.EndAction,
        _                      => null,
    };
}

public class SequenceBuilder<T>
{
    private Sequencer<T> sequencer;

    public SequenceBuilder<T> Initialize(Sequencer<T> sequencer)
    {
        this.sequencer = sequencer;
        return this;
    }
    
    public Sequencer<T> Build() => sequencer;

    public SequenceBuilder<T> AddCondition(string key, Func<bool> condition)
    {
        sequencer.Condition.Add(key, condition);
        return this;
    }
    
    public SequenceBuilder<T> RemoveCondition(string key)
    {
        sequencer.Condition.Remove(key);
        return this;
    }

    public SequenceBuilder<T> AddActiveParam(string key, Action action)
    {
        sequencer.ActiveParamAction.Add(key, action);
        return this;
    }

    public SequenceBuilder<T> AddActiveParam(string key, Action<T> action)
    {
        sequencer.ActiveParamAction.Add(key, action);
        return this;
    }
    
    public SequenceBuilder<T> RemoveActiveParam(string key)
    {
        sequencer.ActiveParamAction.Remove(key);
        return this;
    }
    
    public SequenceBuilder<T> AddActive(string key, Action action) => Add(SectionType.Active, key, action);
    public SequenceBuilder<T> AddCancel(string key, Action action) => Add(SectionType.Cancel, key, action);
    public SequenceBuilder<T> AddComplete(string key, Action action) => Add(SectionType.Complete, key, action);
    public SequenceBuilder<T> AddEnd(string key, Action action) => Add(SectionType.End, key, action);
    public SequenceBuilder<T> RemoveActive(string key) => Remove(SectionType.Active, key);
    public SequenceBuilder<T> RemoveCancel(string key) => Remove(SectionType.Cancel, key);
    public SequenceBuilder<T> RemoveComplete(string key) => Remove(SectionType.Complete, key);
    public SequenceBuilder<T> RemoveEnd(string key) => Remove(SectionType.End, key);
    
    
    private SequenceBuilder<T> Add(SectionType type, string key, Action action)
    {
        GetActionTable(type).Add(key, action);
        return this;
    }
    private SequenceBuilder<T> Remove(SectionType type, string key)
    {
        GetActionTable(type).Remove(key);
        return this;
    }

    private ActionTable GetActionTable(SectionType type) => type switch
    {
        SectionType.Active   => sequencer.ActiveAction,
        SectionType.Cancel   => sequencer.CancelAction,
        SectionType.Complete => sequencer.CompleteAction,
        SectionType.End      => sequencer.EndAction,
        _                    => null,
    };
}