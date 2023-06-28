public class SequenceInvoker
{
    public bool IsInitialized;
    protected Sequencer Sequencer;
    
    public SequenceInvoker(Sequencer sequencer) => Sequencer = sequencer;
    
    public bool IsAbleToActive => Sequencer.Condition == null || Sequencer.Condition.IsAllTrue;
    public bool IsActive { get; private set; }
    public bool IsEnd { get; private set; } = true;

    public void Initialize(Sequencer sequencer)
    {
        IsInitialized = true;
        Sequencer     = sequencer;
    }

    public void Active()
    {
        IsEnd    = false;
        IsActive = true;
        
        Sequencer[SectionType.Active].Invoke();
        Sequencer.CompleteTrigger?.Pull();
    }

    public void Cancel()
    {
        IsActive = false;
        
        Sequencer[SectionType.Cancel].Invoke();
        Sequencer.CompleteTrigger?.Cancel();
        End();
    }
        
    public void Complete()
    {
        IsActive = false;
        
        Sequencer[SectionType.Complete].Invoke();
        End();
    }

    public void End()
    {
        IsEnd = true;
        
        Sequencer[SectionType.End].Invoke();
        Sequencer.CompleteTrigger?.Dispose();
    }
}

public class SequenceInvoker<T>
{
    protected Sequencer<T> Sequencer;
    
    public SequenceInvoker(Sequencer<T> sequencer) => Sequencer = sequencer;
    
    public bool IsAbleToActive => Sequencer.Condition == null || Sequencer.Condition.IsAllTrue;
    public bool IsActive { get; set; }
    public bool IsEnd { get; private set; } = true;
    

    public void Initialize(Sequencer<T> sequencer)
    {
        Sequencer = sequencer;
    }

    public void Active(T value)
    {
        IsEnd    = false;
        IsActive = true;
        
        // Active 가 ActiveParam보다 우선되게 설정. RunBehaviour 참조.
        Sequencer[SectionType.Active].Invoke();
        Sequencer.ActiveParamAction.Invoke(value);
        Sequencer.CompleteTrigger?.Pull();
    }

    public void Cancel()
    {
        IsActive = false;
        
        Sequencer[SectionType.Cancel].Invoke();
        Sequencer.CompleteTrigger?.Cancel();
        End();
    }
        
    public void Complete()
    {
        IsActive = false;
        
        Sequencer[SectionType.Complete].Invoke();
        End();
    }

    public void End()
    {
        IsEnd = true;
        
        Sequencer[SectionType.End].Invoke();
        Sequencer.CompleteTrigger?.Dispose();
    }
}
