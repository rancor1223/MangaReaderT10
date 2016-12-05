using MangaReader.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MangaReader.Models {
    public class MangaPage{
        public int page { get; set; }
        public string url { get; set; }
        public double width { get; set; }
        public double height { get; set; }

        public MangaPage(int p, string u, double w, double h) {
            page = p;
            url = u;
            width = w;
            height = h;
        }

        public static async Task<List<MangaPage>> GetListAsync(string id) {
            return await FormatMangaChapterAsync(id);
        }

        private static async Task<UnformatedMangaChapterList> GetMangaChapterAsync(string id) {
            var http = new HttpClient();
            var response = await http.GetAsync("http://www.mangaeden.com/api/chapter/" + id + "/");
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(UnformatedMangaChapterList));
            //rewrite to Newtonsoft serielizer?

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (UnformatedMangaChapterList)serializer.ReadObject(ms);

            return data;
        }

        private static async Task<List<MangaPage>> FormatMangaChapterAsync(string id) {
            var pageListContainer = await GetMangaChapterAsync(id);
            var pageList = new List<MangaPage>();

            foreach (List<object> page in pageListContainer.images) {
                var formatedPage = new MangaPage(
                    (int)page[0],
                    "https://cdn.mangaeden.com/mangasimg/" + (string)page[1],
                    Convert.ToDouble((int)page[2]),
                    Convert.ToDouble((int)page[3])
                    );
                pageList.Add(formatedPage);
            }

            return pageList;
        }
    }
}
