using System;
using System.Collections.Generic;

namespace Sequences
{
    [Serializable]
    public class SectionTable : Table<SectionType, Section>
    {
        public SectionTable() { }
        public SectionTable(SectionType initialType) => AddSection(initialType);
        public SectionTable(IEnumerable<SectionType> initialTypeArray)
        {
            foreach (var sectionType in initialTypeArray)
            {
                AddSection(sectionType);
            }
        }


        private void AddSection(SectionType type) =>Add(type, new Section(type));
        private void RemoveSection(SectionType type) => Remove(type);
    }
}
