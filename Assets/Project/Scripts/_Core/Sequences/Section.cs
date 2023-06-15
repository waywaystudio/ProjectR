using System;
using UnityEngine;

namespace Sequences
{
    [Serializable]
    public class Section : EventTable
    {
        [SerializeField] private SectionType sectionType;
        
        public SectionType SectionType => sectionType;

        public Section() : this(SectionType.None){ }
        public Section(SectionType sectionType)
        {
            this.sectionType = sectionType;
        } 
    }
    
    // [Serializable]
    // public class Section<T> : EventTable<T>
    // {
    //     [SerializeField] private SectionType sectionType;
    //     
    //     public SectionType SectionType => sectionType;
    //
    //     public Section()
    //     {
    //         sectionType = SectionType.VariantParam;
    //     }
    // }
    //
    // [Serializable]
    // public class AwaitSection<T> : AwaitEventTable<T>
    // {
    //     [SerializeField] private SectionType sectionType;
    //     
    //     public SectionType SectionType => sectionType;
    //
    //     public AwaitSection()
    //     {
    //         sectionType = SectionType.VariantParam;
    //     }
    // }
}
