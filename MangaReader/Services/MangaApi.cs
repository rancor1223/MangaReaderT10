using MangaReader.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MangaReader.Services {

    class MangaApi {
        

        public static async Task PopulateMangaListAsync(ObservableCollection<MangaListItem> mangaList, string search) {
            search = search.Replace("-", " ").ToLower();
            var unfilteredMangaList = await MangaApi.FormatMangaListAsync();
            var filteredMangaList = unfilteredMangaList.Where(item => item.alias.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);

            mangaList.Clear();
            foreach (var mangaItem in filteredMangaList)
                mangaList.Add(mangaItem);
        }


        public static async Task PopulateMangaListAsync(ObservableCollection<MangaListItem> mangaList) {
            mangaList.Clear();
            var ml = await MangaApi.FormatMangaListAsync();
            foreach (var mangaItem in ml)
                mangaList.Add(mangaItem);
        }

        public static async Task PopulateMangaDetailAsync(MangaDetail mangaDetail, string id) {
            mangaDetail = await MangaApi.GetMangaDetailAsync(id);
        }


        //PRIVATE

        private static async Task<List<UnformatedMangaListItem>> GetMangaListAsync() {
            var http = new HttpClient();
            var response = await http.GetAsync("http://www.mangaeden.com/api/list/0/?p=0&l=60");
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(MangaListRootObject));
            //rewrite to Newtonsoft serielizer?

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (MangaListRootObject)serializer.ReadObject(ms);

            return data.manga;
        }

        private static async Task<List<MangaListItem>> FormatMangaListAsync() {
            var mangaListContainer = await GetMangaListAsync();
            var mangaList = new List<MangaListItem>();

            foreach (UnformatedMangaListItem unformatedManga in mangaListContainer) {
                var manga = new MangaListItem();

                manga.id = unformatedManga.i;
                manga.title = unformatedManga.t;
                manga.image = "https://cdn.mangaeden.com/mangasimg/" + unformatedManga.im;
                manga.category = unformatedManga.c;
                manga.status = unformatedManga.s;
                manga.alias = unformatedManga.a.Replace("-", " ");

                mangaList.Add(manga);
            }

            return mangaList;
        }
        
        private static async Task<MangaDetail> GetMangaDetailAsync(string id) {
            var http = new HttpClient();
            var response = await http.GetAsync("http://www.mangaeden.com/api/manga/" + id + "/");
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(MangaDetail));
            //rewrite to Newtonsoft serielizer?

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (MangaDetail)serializer.ReadObject(ms);

            return data;
        }
    }
}
