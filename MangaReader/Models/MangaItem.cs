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

        //constructor

        //full
        public MangaItem(string t, string im, string i, string al, List<string> cat, string desc, string a, string ar, List<ChapterListItem> ch)
            : this(i, im, t, al, cat) {
            description = desc;
            author = a;
            artist = ar;
            chapters = ch;
        }

        //partial (for list view)
        public MangaItem(string t, string im, string i, string al, List<string> cat) : this(t, im) {
            id = i;
            alias = al;
            categories = cat;
        }

        //minimal (for DesignTime)
        public MangaItem(string t, string im) {
            title = t;
            image = im;
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
                var manga = new MangaItem(
                    unformatedManga.t,
                    "https://cdn.mangaeden.com/mangasimg/" + unformatedManga.im,
                    unformatedManga.i,
                    unformatedManga.a.Replace("-", " "),
                    unformatedManga.c
                    );

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

            var chapters = new List<ChapterListItem>();

            foreach (var chapter in unformatedMangaDetail.chapters) {
                string title;
                if (Convert.ToString(chapter[0]) == (string)chapter[2]) {
                    //some chapters are named with their number, which would be duplicitely displayed
                    title = "";
                } else {
                    title = (string)chapter[2];
                }

                var formatedChapter = new ChapterListItem(
                    Convert.ToString(chapter[0]),
                    title,
                    (string)chapter[3]
                    );

                chapters.Add(formatedChapter);
            }


            var formatedMangaDetail = new MangaItem(
                unformatedMangaDetail.title,
                "https://cdn.mangaeden.com/mangasimg/" + unformatedMangaDetail.image,
                "",
                "",
                unformatedMangaDetail.categories,
                unformatedMangaDetail.description,
                unformatedMangaDetail.author,
                unformatedMangaDetail.artist,
                chapters
                );

            return formatedMangaDetail;
        }
    }
}
