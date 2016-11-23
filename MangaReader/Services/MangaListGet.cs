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
    class MangaListGet {
        public static async Task<List<MangaItem>> GetListAsync(string search) {
            search = search.Replace("-", " ").ToLower();
            var unfilteredMangaList = await FormatMangaListAsync();
            var filteredMangaList = unfilteredMangaList.Where(item => item.alias.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);

            return filteredMangaList.ToList();
        }

        public static async Task<List<MangaItem>> GetListAsync() {
            return await FormatMangaListAsync();
        }

        private static async Task<List<UnformatedMangaListItem>> GetMangaListAsync() {
            var http = new HttpClient();
            var response = await http.GetAsync("http://www.mangaeden.com/api/list/0/?p=0&l=60");
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(UnformatedMangaList));
            //rewrite to Newtonsoft serielizer?

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (UnformatedMangaList)serializer.ReadObject(ms);

            return data.manga;
        }

        private static async Task<List<MangaItem>> FormatMangaListAsync() {
            var mangaListContainer = await GetMangaListAsync();
            var mangaList = new List<MangaItem>();

            foreach (UnformatedMangaListItem unformatedManga in mangaListContainer) {
                var manga = new MangaItem();

                manga.id = unformatedManga.i;
                manga.title = unformatedManga.t;
                manga.image = "https://cdn.mangaeden.com/mangasimg/" + unformatedManga.im;
                manga.categories = unformatedManga.c;
                manga.alias = unformatedManga.a.Replace("-", " ");

                mangaList.Add(manga);
            }

            return mangaList;
        }
    }
}
