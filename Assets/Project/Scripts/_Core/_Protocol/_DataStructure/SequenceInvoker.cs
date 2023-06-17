using System;

public class SequenceInvoker
{
    private Sequencer sequencer;
    
    public bool IsAbleToActive => sequencer.Condition == null || sequencer.Condition.IsAllTrue;
    public bool IsActive { get; private set; }
    public bool IsEnd { get; private set; } = true;

    public void Initialize(Sequencer sequencer) => this.sequencer = sequencer;

    public void Active()
    {
        IsEnd    = false;
        IsActive = true;
        
        sequencer.ActiveAction.Invoke();
        sequencer.CompleteTrigger?.Pull();
    }

    public void AddCompleteTrigger(Func<bool> condition)
    {
        sequencer.CompleteTrigger = new WaitTrigger(Complete, condition);
    }

    public void Cancel()
    {
        IsActive = false;
        
        sequencer.CancelAction.Invoke();
        sequencer.CompleteTrigger?.Cancel();
        End();
    }
        
    public void Complete()
    {
        IsActive = false;
        
        sequencer.CompleteAction.Invoke();
        End();
    }

    public void End()
    {
        IsEnd = true;
        
        sequencer.EndAction.Invoke();
        sequencer.CompleteTrigger?.Dispose();
    }
}

public class SequenceInvoker<T>
{
    private Sequencer<T> sequencer;
    
    public bool IsAbleToActive => sequencer.Condition == null || sequencer.Condition.IsAllTrue;
    public bool IsActive { get; private set; }
    public bool IsEnd { get; private set; } = true;
    
    public void Initialize(Sequencer<T> sequencer) => this.sequencer = sequencer;

    public void Active(T value)
    {
        IsEnd    = false;
        IsActive = true;
        
        // Active 가 ActiveParam보다 우선되게 설정. RunBehaviour 참조.
        sequencer.ActiveAction.Invoke();
        sequencer.ActiveParamAction.Invoke(value);
        sequencer.CompleteTrigger?.Pull();
    }

    public void AddCompleteTrigger(Func<bool> condition)
    {
        sequencer.CompleteTrigger = new WaitTrigger(Complete, condition);
    }

    public void Cancel()
    {
        IsActive = false;
        
        sequencer.CancelAction.Invoke();
        sequencer.CompleteTrigger?.Cancel();
        End();
    }
        
    public void Complete()
    {
        IsActive = false;
        
        sequencer.CompleteAction.Invoke();
        End();
    }

    public void End()
    {
        IsEnd = true;
        
        sequencer.EndAction.Invoke();
        sequencer.CompleteTrigger?.Dispose();
    }
}
