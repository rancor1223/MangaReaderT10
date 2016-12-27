using Newtonsoft.Json;
using System;
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

        public static async Task<List<UnformatedMangaListItem>> GetListRemoteAsync() {
            try {
                var http = new HttpClient();
                var response = await http.GetAsync("http://www.mangaeden.com/api/list/0/");
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UnformatedMangaList>(result);
                return data.manga;
            }
            catch (Exception e) {
                // Throw an HttpException with customized message.
                throw new Exception("No connection");
            }
        }

        public static async Task<List<UnformatedMangaListItem>> GetListLocalAsync() {
            try {
                var http = new HttpClient();
                var response = await http.GetAsync("http://www.mangaeden.com/api/list/0/");
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UnformatedMangaList>(result);
                return data.manga;
            }
            catch (Exception e) {
                // Throw an HttpException with customized message.
                throw new Exception("No connection");
            }
        }
    }
}
