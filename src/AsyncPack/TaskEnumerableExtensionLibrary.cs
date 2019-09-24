using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncPack
{
    public static class TaskEnumerableExtensionLibrary
    {
        public static Task WhenAll(this IEnumerable<Task> tasks)
        {
            return Task.WhenAll(tasks);
        }

        public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks)
        {
            return Task.WhenAll(tasks);
        }
    }
}
