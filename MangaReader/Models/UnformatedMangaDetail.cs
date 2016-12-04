using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MangaReader.Models {
    public class UnformatedMangaDetail {
        public List<object> aka { get; set; }
        //public List<object> aka-alias { get; set; }
        public string alias { get; set; }
        public string artist { get; set; }
        public List<object> artist_kw { get; set; }
        public string author { get; set; }
        public List<object> author_kw { get; set; }
        public List<string> categories { get; set; }
        public List<List<object>> chapters { get; set; }
        public int? chapters_len { get; set; }
        public double? created { get; set; }
        public string description { get; set; }
        public int? hits { get; set; }
        public string image { get; set; }
        public int language { get; set; }
        public double? last_chapter_date { get; set; }
        public List<double> random { get; set; }
        public object released { get; set; }
        public string startsWith { get; set; }
        public int? status { get; set; }
        public string title { get; set; }
        public List<string> title_kw { get; set; }
        public int? type { get; set; }
        public bool updatedKeywords { get; set; }

        public static async Task<UnformatedMangaDetail> GetMangaDetailAsync(string id) {
            var http = new HttpClient();
            var response = await http.GetAsync("http://www.mangaeden.com/api/manga/" + id + "/");
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(UnformatedMangaDetail));
            //rewrite to Newtonsoft serielizer?

            var ms = new MemoryStream(Encoding.Unicode.GetBytes(result));
            var data = (UnformatedMangaDetail)serializer.ReadObject(ms);

            return data;
        }
    }
}
