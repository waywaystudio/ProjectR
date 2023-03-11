public interface IPoolable<T> where T : class, IPoolable<T>
{
    Pool<T> Pool { get; set; }
}