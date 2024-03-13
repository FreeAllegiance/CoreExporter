using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace CoreExporter
{
    internal class IgnorePropertiesWithName
    {
        private readonly string[] _ignoredNames;

        public IgnorePropertiesWithName(params string[] ignoredNames)
            => _ignoredNames = ignoredNames;

        public void ModifyTypeInfo(JsonTypeInfo ti)
        {
            if (ti.Kind != JsonTypeInfoKind.Object)
                return;

            foreach (var prop in ti.Properties)
                Console.WriteLine(prop.Name + ": " + prop.PropertyType.FullName);

            ti.Properties.RemoveAll(prop => _ignoredNames.Contains(prop.Name));

        }
    }

    public static class ListHelpers
    {
        // IList<T> implementation of List<T>.RemoveAll method.
        public static void RemoveAll<T>(this IList<T> list, Predicate<T> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i--);
                }
            }
        }
    }
}
