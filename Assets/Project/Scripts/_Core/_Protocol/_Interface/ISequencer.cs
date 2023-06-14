using Sequences;

public interface ISequencer
{
    ConditionTable Condition { get; }
    Section Activation { get; }
    Section Cancellation { get; }
    Section Complete { get; }
    Section End { get; }
}

public interface ISequencer<T> : ISequencer
{
    Section<T> ActiveParamSection { get; }
    
    // + ConditionTable Condition { get; }
    // + Section Activation { get; }
    // + Section Cancellation { get; }
    // + Section Complete { get; }
    // + Section End { get; }
}
