using System.Reflection;

namespace rubicon_blog.Helpers
{
    public static class Helper
    {
        public static PropertyInfo[] GetNonNullProperties<T> (this T obj)
        {
            PropertyInfo[] properties = Activator.CreateInstance<T>().GetType().GetProperties();
            return properties.Where(r => r.GetValue(obj) != null).ToArray();
        }

        public static PropertyInfo[] GetNullProperties<T>(this T obj)
        {
            PropertyInfo[] properties = Activator.CreateInstance<T>().GetType().GetProperties();
            return properties.Where(r => r.GetValue(obj) == null).ToArray();
        }

        public static void SetNullProperties<T>(this T obj, T oldObj)
        {
            var nullProperties = obj.GetNullProperties();
            foreach(var property in nullProperties)
            {
                property.SetValue(obj, property.GetValue(oldObj));
            }
        }

    }
}
