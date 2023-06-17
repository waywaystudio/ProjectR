using System;

public class TrySequencer
{
    public bool IsAbleToActive => Condition == null || Condition.IsAllTrue;
    public bool IsActive { get; protected set; }
    public bool IsEnd { get; protected set; } = true;

    public ConditionTable Condition { get; } = new();
    public ActionTable ActiveAction { get; } = new();
    public ActionTable CancelAction { get; } = new();
    public ActionTable CompleteAction { get; } = new();
    public ActionTable EndAction { get; } = new();
    
    protected WaitTrigger CompleteTrigger { get; set; }
    

    public virtual void Active()
    {
        IsEnd    = false;
        IsActive = true;
        
        ActiveAction.Invoke();
        CompleteTrigger?.Pull();
    }

    public void AddCompleteTrigger(Func<bool> condition)
    {
        CompleteTrigger = new WaitTrigger(Complete, condition);
    }

    public void Cancel()
    {
        IsActive = false;
        
        CancelAction.Invoke();
        CompleteTrigger?.Cancel();
        End();
    }
        
    public void Complete()
    {
        IsActive = false;
        
        CompleteAction.Invoke();
        End();
    }

    public void End()
    {
        IsEnd = true;
        
        EndAction.Invoke();
        CompleteTrigger?.Dispose();
    }
        
    public void Clear()
    {
        Condition.Clear();
        ActiveAction.Clear();
        CancelAction.Clear();
        CompleteAction.Clear();
        EndAction.Clear();
        CompleteTrigger?.Dispose();
    }
}

public class TrySequencer<T> : TrySequencer
{
    public T Value { get; set; }
    public ActionTable<T> ActiveParamAction { get; } = new();

    public void Active(T value)
    {
        Value = value;
        
        Active();
    }

    public override void Active()
    {
        IsEnd    = false;
        IsActive = true;
        
        ActiveParamAction.Invoke(Value);
        ActiveAction.Invoke();
        CompleteTrigger?.Pull();
    }
}
