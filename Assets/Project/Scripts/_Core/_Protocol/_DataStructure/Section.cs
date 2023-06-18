using System;
using UnityEngine;

[Serializable]
public class Section
{
    [SerializeField] private SectionType sectionType;

    public SectionType SectionType => sectionType;
    public ActionTable ActionTable { get; } = new();

    public Section() : this(SectionType.None) { }
    public Section(SectionType sectionType)
    {
        this.sectionType = sectionType;
    }
}
