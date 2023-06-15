using System;
using UnityEngine;

namespace Common
{
    public interface IOldActiveSection { ActionTable OnActivated { get; }}
    public interface IOldCancelSection { ActionTable OnCanceled { get; }}
    public interface IOldCompleteSection { ActionTable OnCompleted { get; }}
    public interface IOldEndSection { ActionTable OnEnded { get; }}

    public interface IOldSequence : IOldActiveSection, IOldCancelSection, IOldCompleteSection, IOldEndSection
    {
        // ActionTable OnActivated { get; }
        // ActionTable OnCanceled { get; }
        // ActionTable OnCompleted { get; }
        // ActionTable OnEnded { get; }
    }

    public interface IOldConditionalSequence : IOldSequence
    {
        ConditionTable Conditions { get; }
    }
    
    public interface IOldProjectorSequence : IOldSequence
    {
        float CastingTime { get; }
        Vector2 SizeVector { get; }
    }

    public static class SequenceExtension
    {
        /// <summary>
        /// main에서 Action을 실행할 때, sub의 ActionTable을 참조한다. 
        /// </summary>
        public static void CombineAsReference(this IOldSequence main, string key, IOldSequence sub)
        {
            main.OnActivated.Register(key, sub.OnActivated);
            main.OnCanceled.Register(key, sub.OnCanceled);
            main.OnCompleted.Register(key, sub.OnCompleted);
            main.OnEnded.Register(key, sub.OnEnded);
        }
        
        /// <summary>
        /// 결합 시점에 sub 시퀀스 ActionTable의 Action만 포함한다. 
        /// </summary>
        public static void CombineAsValue(this IOldSequence main, IOldSequence sub)
        {
            main.OnActivated.Register(sub.OnActivated);
            main.OnCanceled.Register(sub.OnCanceled);
            main.OnCompleted.Register(sub.OnCompleted);
            main.OnEnded.Register(sub.OnEnded);
        }

        public static void Clear(this IOldSequence sequence)
        {
            sequence.OnActivated.Clear();
            sequence.OnCanceled.Clear();
            sequence.OnCompleted.Clear();
            sequence.OnEnded.Clear();
        }

        public static ActionTable ConvertSection(this IOldSequence sequence, SectionType sectionType)
        {
            return sectionType switch
            {
                SectionType.None => throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null),
                SectionType.Activation => sequence.OnActivated,
                SectionType.Cancel => sequence.OnCanceled,
                SectionType.Complete => sequence.OnCompleted,
                SectionType.End     => sequence.OnEnded,
                _                       => throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null)
            };
        }
    }
}