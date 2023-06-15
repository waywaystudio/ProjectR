using System;

[Serializable]
public class Sequencer : ISequencer
{
    public bool IsAbleToActive => Condition == null || Condition.IsAllTrue;

    public ConditionTable Condition { get; } = new();
    public ActionTable ActiveAction { get; } = new();
    public ActionTable CancelAction { get; } = new();
    public ActionTable CompleteAction { get; } = new();
    public ActionTable EndAction { get; } = new();
    
    private WaitTrigger CompleteTrigger { get; set; }
    

    public void Active()
    {
        ActiveAction.Invoke();
        CompleteTrigger?.Pull();
    }

    public void AddCompleteTrigger(Func<bool> condition)
    {
        CompleteTrigger = new WaitTrigger(Complete, condition);
    }

    public void Cancel()
    {
        CancelAction.Invoke();
        CompleteTrigger?.Cancel();
        End();
    }
        
    public void Complete()
    {
        CompleteAction.Invoke();
        End();
    }

    public void End()
    {
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
public class Sequencer<T> : Sequencer, ISequencer<T>
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
    }
}