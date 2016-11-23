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
    class MangaDetailGet {
        public static async Task<MangaItem> GetAsync(string id) {
            return await FormatMangaDetailAsync(id);
        }

        private static async Task<UnformatedMangaDetail> GetMangaDetailAsync(string id) {
            var http = new HttpClient();
            var response = await http.GetAsync("http://www.mangaeden.com/api/manga/" + id + "/");
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(UnformatedMangaDetail));
            //rewrite to Newtonsoft serielizer?

            var ms = new MemoryStream(Encoding.Unicode.GetBytes(result));
            var data = (UnformatedMangaDetail)serializer.ReadObject(ms);

            return data;
        }

        private static async Task<MangaItem> FormatMangaDetailAsync(string id) {
            var unformatedMangaDetail = await GetMangaDetailAsync(id);
            var formatedMangaDetail = new MangaItem();

            formatedMangaDetail.title = unformatedMangaDetail.title;
            formatedMangaDetail.image = "https://cdn.mangaeden.com/mangasimg/" + unformatedMangaDetail.image;
            formatedMangaDetail.description = unformatedMangaDetail.description;
            formatedMangaDetail.author = unformatedMangaDetail.author;
            formatedMangaDetail.artist = unformatedMangaDetail.artist;
            formatedMangaDetail.categories = unformatedMangaDetail.categories;
            formatedMangaDetail.chapters = new List<ChapterListItem>();

            foreach (var chapter in unformatedMangaDetail.chapters) {
                var formatedChapter = new ChapterListItem();
                formatedChapter.number = Convert.ToString(chapter[0]);

                if (formatedChapter.number == (string)chapter[2]) {
                    //some chapters are named with their number, which would be duplicitely displayed
                    formatedChapter.title = "";
                } else {
                    formatedChapter.title = (string)chapter[2];
                }

                formatedChapter.id = (string)chapter[3];

                formatedMangaDetail.chapters.Add(formatedChapter);
            }

            return formatedMangaDetail;
        }
    }
}
