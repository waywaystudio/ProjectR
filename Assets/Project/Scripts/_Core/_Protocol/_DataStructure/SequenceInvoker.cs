using System;

public class SequenceInvoker
{
    protected Sequencer Sequencer;
    
    public bool IsAbleToActive => Sequencer.Condition == null || Sequencer.Condition.IsAllTrue;
    public bool IsActive { get; private set; }
    public bool IsEnd { get; private set; } = true;

    public void Initialize(Sequencer sequencer) => this.Sequencer = sequencer;

    public void Active()
    {
        IsEnd    = false;
        IsActive = true;
        
        Sequencer.ActiveAction.Invoke();
        Sequencer.CompleteTrigger?.Pull();
    }

    public void AddCompleteTrigger(Func<bool> condition)
    {
        Sequencer.CompleteTrigger = new WaitTrigger(Complete, condition);
    }

    public void Cancel()
    {
        IsActive = false;
        
        Sequencer.CancelAction.Invoke();
        Sequencer.CompleteTrigger?.Cancel();
        End();
    }
        
    public void Complete()
    {
        IsActive = false;
        
        Sequencer.CompleteAction.Invoke();
        End();
    }

    public void End()
    {
        IsEnd = true;
        
        Sequencer.EndAction.Invoke();
        Sequencer.CompleteTrigger?.Dispose();
    }
}

public class SequenceInvoker<T>
{
    public bool IsInitialized;
    protected Sequencer<T> Sequencer;
    
    public bool IsAbleToActive => Sequencer.Condition == null || Sequencer.Condition.IsAllTrue;
    public bool IsActive { get; private set; }
    public bool IsEnd { get; private set; } = true;

    public void Initialize(Sequencer<T> sequencer)
    {
        IsInitialized = true;
        Sequencer     = sequencer;
    }

    public void Active(T value)
    {
        IsEnd    = false;
        IsActive = true;
        
        // Active 가 ActiveParam보다 우선되게 설정. RunBehaviour 참조.
        Sequencer.ActiveAction.Invoke();
        Sequencer.ActiveParamAction.Invoke(value);
        Sequencer.CompleteTrigger?.Pull();
    }

    public void AddCompleteTrigger(Func<bool> condition)
    {
        Sequencer.CompleteTrigger = new WaitTrigger(Complete, condition);
    }

    public void Cancel()
    {
        IsActive = false;
        
        Sequencer.CancelAction.Invoke();
        Sequencer.CompleteTrigger?.Cancel();
        End();
    }
        
    public void Complete()
    {
        IsActive = false;
        
        Sequencer.CompleteAction.Invoke();
        End();
    }

    public void End()
    {
        IsEnd = true;
        
        Sequencer.EndAction.Invoke();
        Sequencer.CompleteTrigger?.Dispose();
    }
}
