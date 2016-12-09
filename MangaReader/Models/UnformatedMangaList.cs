using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MangaReader.Models {
    public class UnformatedMangaList {
        public int end { get; set; }
        public List<UnformatedMangaListItem> manga { get; set; }
        public int page { get; set; }
        public int start { get; set; }
        public int total { get; set; }

        public static async Task<List<UnformatedMangaListItem>> GetListAsync() {
            try {
                var http = new HttpClient();
                var response = await http.GetAsync("http://www.mangaeden.com/api/list/0/?p=0&l=60");
                var result = await response.Content.ReadAsStringAsync();
                var serializer = new DataContractJsonSerializer(typeof(UnformatedMangaList));
                //rewrite to Newtonsoft serielizer?

                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                var data = (UnformatedMangaList)serializer.ReadObject(ms);

                return data.manga;
            }
            catch (HttpRequestException e) {
                // Throw an HttpException with customized message.
                throw new HttpRequestException("No connection");
            }
        }
    }
}
