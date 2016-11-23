using MangaReader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MangaReader.Services {
    class MangaChapterGet {
        public static async Task<List<MangaPage>> GetAsync(string id) {
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
                var formatedPage = new MangaPage();
                formatedPage.page = (int)page[0];
                formatedPage.url = "https://cdn.mangaeden.com/mangasimg/" + (string)page[1];
                formatedPage.width = (int)page[2];
                formatedPage.heigth = (int)page[3];
                pageList.Add(formatedPage);
            }

            return pageList;
        }
    }
}
