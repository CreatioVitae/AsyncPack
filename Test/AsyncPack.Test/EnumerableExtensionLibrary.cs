using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AsyncPack.Test
{
    public class EnumerableExtensionLibrary
    {
        [Fact]
        public async Task UseEnumerableExtensionForEachAsync()
        {
            var httpgetUrlList = new string[] { "https://www.yahoo.co.jp/", "https://www.google.co.jp/" };

            var httpClient = new HttpClient();

            await httpgetUrlList.ForEachAsync(async url => {
                var content = await httpClient.GetStringAsync(url);
                Console.WriteLine(content);
            }, 50);
        }
    }
}
