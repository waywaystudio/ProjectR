namespace Core.Singleton
{
    public class Singleton<T> where T : class, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // Debug.Log("instance is null");
                    instance = new T();

                    return instance;
                }

                // Debug.Log("instance is Not null");
                return instance;
            }
        }

        protected Singleton() { }
    }
}