using System.Linq;

namespace Core
{
    public static class InterfaceExtension
    {
        public static void CopyPropertiesTo<T>(this T source, T dest)
        {
            var plist = 
                from prop 
                in typeof(T).GetProperties() 
                where prop.CanRead && prop.CanWrite 
                select prop;

            foreach (var prop in plist)
            {
                prop.SetValue(dest, prop.GetValue(source, null), null);
            }
        }
    }
}
