namespace Core
{
    public interface ISearchedListTaker : ISearchEngine
    {
        ICombatTaker MainTarget { get; set; }
        ICombatTaker Self { get; }
    }
}