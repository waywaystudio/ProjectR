public class SequenceInvoker
{
    protected readonly Sequencer Sequencer;
    
    public SequenceInvoker(Sequencer sequencer) => Sequencer = sequencer;
    
    public bool IsAbleToActive => Sequencer.Condition == null || Sequencer.Condition.IsAllTrue;
    public bool IsActive { get; private set; }
    public bool IsEnd { get; private set; } = true;


    public void Active()
    {
        IsEnd    = false;
        IsActive = true;
        
        Sequencer[Section.Active].Invoke();

        // Handle Active Just once and than disappear Action.
        if (Sequencer.Table.Remove(Section.ActiveOnce, out var onceAction))
        {
            onceAction.Invoke();
        }

        Sequencer.CompleteTrigger?.Pull();
    }

    public void Cancel()
    {
        IsActive = false;
        
        Sequencer[Section.Cancel].Invoke();
        Sequencer.CompleteTrigger?.Cancel();
        End();
    }
        
    public void Complete()
    {
        IsActive = false;
        
        Sequencer[Section.Complete].Invoke();
        End();
    }

    public void End()
    {
        IsEnd = true;
        
        Sequencer[Section.End].Invoke();
        Sequencer.CompleteTrigger?.Dispose();
    }
    
    public void ExtraAction() => Sequencer[Section.Extra].Invoke();
}

public class SequenceInvoker<T>
{
    protected readonly Sequencer<T> Sequencer;
    
    public SequenceInvoker(Sequencer<T> sequencer) => Sequencer = sequencer;
    
    public bool IsAbleToActive => Sequencer.Condition == null || Sequencer.Condition.IsAllTrue;
    public bool IsActive { get; set; }
    public bool IsEnd { get; private set; } = true;


    public void Active(T value)
    {
        IsEnd    = false;
        IsActive = true;
        
        // Active 가 ActiveParam보다 우선되게 설정. RunBehaviour 참조.
        Sequencer[Section.Active].Invoke();
        Sequencer.ActiveParamAction.Invoke(value);
        
        // Handle Active Just once and than disappear Action.
        if (Sequencer.Table.Remove(Section.ActiveOnce, out var onceAction))
        {
            onceAction.Invoke();
        }
        
        Sequencer.CompleteTrigger?.Pull();
    }

    public void Cancel()
    {
        IsActive = false;
        
        Sequencer[Section.Cancel].Invoke();
        Sequencer.CompleteTrigger?.Cancel();
        End();
    }
        
    public void Complete()
    {
        IsActive = false;
        
        Sequencer[Section.Complete].Invoke();
        End();
    }

    public void End()
    {
        IsEnd = true;
        
        Sequencer[Section.End].Invoke();
        Sequencer.CompleteTrigger?.Dispose();
    }
    
    public void ExtraAction() => Sequencer[Section.Extra].Invoke();
}
