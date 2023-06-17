using System;

[Serializable]
public class Sequencer : ISections
{
    public bool IsAbleToActive => Condition == null || Condition.IsAllTrue;
    public bool IsActive { get; private set; }
    public bool IsEnd { get; private set; } = true;

    public ConditionTable Condition { get; } = new();
    public WaitTrigger CompleteTrigger { get; set; }
    
    public ActionTable ActiveAction { get; } = new();
    public ActionTable CancelAction { get; } = new();
    public ActionTable CompleteAction { get; } = new();
    public ActionTable EndAction { get; } = new();
    

    public void Active()
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

[Serializable]
public class Sequencer<T> : Sequencer, IParamSection<T>
{
    public ActionTable<T> ActiveParamAction { get; } = new();
    
    public void Active(T value)
    {
        ActiveParamAction.Invoke(value);
        Active();
    }

    public new void Clear()
    {
        ActiveParamAction.Clear();
        
        Condition.Clear();
        ActiveAction.Clear();
        CancelAction.Clear();
        CompleteAction.Clear();
        EndAction.Clear();
        CompleteTrigger?.Dispose();
    }
}
