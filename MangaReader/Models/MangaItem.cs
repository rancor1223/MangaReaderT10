using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MangaReader.Models {
    public class MangaItem {
        public string id { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public string alias { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public string artist { get; set; }
        public List<string> categories { get; set; }
        public List<ChapterListItem> chapters { get; set; }

        public bool isFavourite {
            get {
                return false; //placeholder
            }
        }
        public int unreadChapters {
            get {
                Random rnd = new Random();
                return rnd.Next(0, 15); //placeholder
            }
        }

        //List of manga with only basic info

        public static async Task<List<MangaItem>> GetListAsync(string search) {
            search = search.Replace("-", " ").ToLower();
            var unfilteredMangaList = await FormatMangaListAsync();
            var filteredMangaList = unfilteredMangaList.Where(item => item.alias.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);

            return filteredMangaList.ToList();
        }

        public static async Task<List<MangaItem>> GetListAsync() {
            return await FormatMangaListAsync();
        }

        private static async Task<List<MangaItem>> FormatMangaListAsync() {
            var mangaListContainer = await UnformatedMangaList.GetListAsync();
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

        //Complete detail of one manga

        public static async Task<MangaItem> GetDetailAsync(string id) {
            return await FormatMangaDetailAsync(id);
        }

        private static async Task<MangaItem> FormatMangaDetailAsync(string id) {
            var unformatedMangaDetail = await UnformatedMangaDetail.GetMangaDetailAsync(id);
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
