using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MangaReader.Models {
    public class UnformatedMangaChapterList {
        public List<List<object>> images { get; set; }

        public static async Task<UnformatedMangaChapterList> GetMangaChapterAsync(string id) {

            try {
                var http = new HttpClient();
                var response = await http.GetAsync("http://www.mangaeden.com/api/chapter/" + id + "/");
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UnformatedMangaChapterList>(result);
                return data;
            }
            catch (Exception e) {
                // Throw an HttpException with customized message.
                throw new Exception("No connection");
            }

        }
    }
}
