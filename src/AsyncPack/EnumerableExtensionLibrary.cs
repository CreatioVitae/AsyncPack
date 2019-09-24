using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncPack {
    public static class EnumerableExtensionLibrary {
        public static async Task ForEachAsync<T>(this IEnumerable<T> sources, Func<T, Task> func, int concurrency, CancellationToken cancellationToken = default, bool configureAwait = false) {
            static void ValidateAndThrowExceptionIfArgumentError<TSource>(IEnumerable<TSource> collection, Func<TSource, Task> action, int concurrency) {
                if (collection == null || !collection.Any()) { throw new ArgumentNullException(nameof(collection)); }
                if (action == null) { throw new ArgumentNullException(nameof(action)); }
                if (concurrency <= 0) { throw new ArgumentOutOfRangeException($"{nameof(concurrency)}は1以上、{int.MaxValue.ToString()}以下に設定してください。但し、同時並列数は現実的な範囲内で設定することをお勧めします。"); }
            }

            ValidateAndThrowExceptionIfArgumentError(sources, func, concurrency);

            using var semaphore = new SemaphoreSlim(initialCount: concurrency, maxCount: concurrency);
            var exceptionCount = 0;
            var tasks = new List<Task>();

            foreach (var item in sources) {
                if (exceptionCount > 0) { break; }
                cancellationToken.ThrowIfCancellationRequested();

                await semaphore.WaitAsync(cancellationToken).ConfigureAwait(configureAwait);

                var task = func(item).ContinueWith(t => {
                    semaphore.Release();

                    if (t.IsFaulted) {
                        Interlocked.Increment(ref exceptionCount);
                        throw t.Exception;
                    }
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks.ToArray()).ConfigureAwait(configureAwait);
        }


    }
}
