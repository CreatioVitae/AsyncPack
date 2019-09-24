using AsyncPack;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OZmall.AsyncPack.Test
{
    public class TaskEnumerableExtensionLibrary
    {
        [Fact]
        public async Task UseTaskExtensionWhenAll()
        {
            #region Enumerable async(Comment Out;)
            //static async Task CopyAsync((string originDirectory, string destinationDirectory) copySetting) {
            //    (var originDirectory, var destinationDirectory) = copySetting;

            //    foreach (var filename in Directory.EnumerateFiles(originDirectory)) {
            //        using var originStream = File.Open(filename, FileMode.Open);
            //        using var destinationStream = File.Create($"{destinationDirectory}{filename.Substring(filename.LastIndexOf('\\'))}");
            //        await originStream.CopyToAsync(destinationStream);
            //    }
            //}
            #endregion

            #region Select async(Comment Out;)
            //static async Task CopyAsync((string originDirectory, string destinationDirectory) copySetting) {
            //    (var originDirectory, var destinationDirectory) = copySetting;

            //    await Task.WhenAll(Directory.EnumerateFiles(originDirectory).Select(async filename => {
            //        using var originStream = File.Open(filename, FileMode.Open);
            //        using var destinationStream = File.Create($"{destinationDirectory}{filename.Substring(filename.LastIndexOf('\\'))}");
            //        await originStream.CopyToAsync(destinationStream);
            //    }));
            //}
            #endregion

            static async Task CopyAsync((string originDirectory, string destinationDirectory) copyParam)
            {
                (var originDirectory, var destinationDirectory) = copyParam;

                await Directory.EnumerateFiles(originDirectory).Select(async filename => {
                    using var SourceStream = File.Open(filename, FileMode.Open);
                    using var DestinationStream = File.Create($"{destinationDirectory}{filename.Substring(filename.LastIndexOf('\\'))}");
                    await SourceStream.CopyToAsync(DestinationStream);
                }).WhenAll();
            }

            var appDirectory = new FileInfo(typeof(EnumerableExtensionLibrary).Assembly.Location).Directory.Parent.Parent.Parent.FullName;

            await CopyAsync((originDirectory: $"{appDirectory}/Origin", destinationDirectory: $"{appDirectory}/Destination"));
        }
    }
}