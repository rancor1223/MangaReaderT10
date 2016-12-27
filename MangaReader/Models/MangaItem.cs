using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

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
        
        //constructor

        public MangaItem(){} //prázdný constructor je nutný při deserializaci dat ze souboru

        //full
        public MangaItem(string t, string im, string i, string al, List<string> cat, string desc, string a, string ar, List<ChapterListItem> ch)
            : this(t, im, i, al) {
            categories = cat;
            description = desc;
            author = a;
            artist = ar;
            chapters = ch;
        }

        //partial (for list view)
        public MangaItem(string t, string im, string i, string al) {
            title = t;
            image = im;
            id = i;
            alias = al;
        }

        //List of manga with only partial info

        public static async Task<List<MangaItem>> GetListAsync(string search) {
            search = search.Replace("-", " ").ToLower();

            var localSettings = ApplicationData.Current.LocalSettings;
            var storageFolder = ApplicationData.Current.LocalFolder;

            var filename = "mangaeden.json";
            var fileLoc = await storageFolder.TryGetItemAsync(filename);
            var fileExists = fileLoc != null;

            DateTime lastUpdate;
            var lastUpdateString = Convert.ToString(localSettings.Values["MangaedenLastUpdate"]);

            var unfilteredMangaList = new List<MangaItem>();

            if (DateTime.TryParse(lastUpdateString, out lastUpdate) && fileExists) {
                if ((lastUpdate - DateTime.Now).TotalDays < 1) {
                    //unfilteredMangaList = await FormatMangaListAsync();
                    unfilteredMangaList = await RetrieveLocalMangaListAsync();
                } else {
                    unfilteredMangaList = await FormatMangaListAsync();
                }
            } else {
                unfilteredMangaList = await FormatMangaListAsync();
            }


            var filteredMangaList = unfilteredMangaList.Where(item => item.alias.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            return filteredMangaList.ToList();
        }

        public static async Task<List<MangaItem>> GetListAsync() {
            var localSettings = ApplicationData.Current.LocalSettings;
            var storageFolder = ApplicationData.Current.LocalFolder;

            var filename = "mangaeden.json";
            var fileLoc = await storageFolder.TryGetItemAsync(filename);
            var fileExists = fileLoc != null;

            DateTime lastUpdate;
            var lastUpdateString = Convert.ToString(localSettings.Values["MangaedenLastUpdate"]);

            if (DateTime.TryParse(lastUpdateString, out lastUpdate) && fileExists) {
                if ((lastUpdate - DateTime.Now).TotalDays < 1) {
                    return await RetrieveLocalMangaListAsync();
                } else {
                    return await FormatMangaListAsync();
                }
            } else {
                return await FormatMangaListAsync();
            }
        }

        private static async Task<List<MangaItem>> FormatMangaListAsync() {
            var mangaListContainer = await UnformatedMangaList.GetListRemoteAsync();
            var mangaList = new List<MangaItem>();

            //zformátovaní dat
            foreach (UnformatedMangaListItem unformatedManga in mangaListContainer) {
                var manga = new MangaItem(
                    unformatedManga.t,
                    "https://cdn.mangaeden.com/mangasimg/" + unformatedManga.im,
                    unformatedManga.i,
                    unformatedManga.a.Replace("-", " ")
                    );

                mangaList.Add(manga);
            }

            //catchování dat do JSON souboru pro rychlejší přístup
            SavelMangaListToFileAsync(mangaList);

            return mangaList;
        }

        private static async void SavelMangaListToFileAsync(List<MangaItem> mangaList) {
            //serializace dat do JSON souboru
            var storageFolder = ApplicationData.Current.LocalFolder; //aplikační data složka
            var localSettings = ApplicationData.Current.LocalSettings; //aplikační nastavení
            
            var mangaedenFile = await storageFolder.CreateFileAsync("mangaeden.json", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            var stream = await mangaedenFile.OpenAsync(FileAccessMode.ReadWrite);

            using (var outputStream = stream.GetOutputStreamAt(0)) {
                using (var dataWriter = new Windows.Storage.Streams.DataWriter(outputStream)) {
                    string json = JsonConvert.SerializeObject(mangaList);
                    dataWriter.WriteString(json);
                    await dataWriter.StoreAsync();
                    await outputStream.FlushAsync();
                }
            }
            stream.Dispose();

            localSettings.Values["MangaedenLastUpdate"] = Convert.ToString(DateTime.UtcNow);
            Debug.WriteLine("JSON saved!");
        }

        private static async Task<List<MangaItem>> RetrieveLocalMangaListAsync() {
            //deserializace dat z JSON souboru
            var mangaList = new List<MangaItem>();
            var storageFolder = ApplicationData.Current.LocalFolder;
            var mangaedenFile = await storageFolder.GetFileAsync("mangaeden.json");
            var localSettings = ApplicationData.Current.LocalSettings;

            var stream = await mangaedenFile.OpenAsync(FileAccessMode.ReadWrite);
            ulong size = stream.Size;

            using (var inputStream = stream.GetInputStreamAt(0)) {
                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream)) {
                    uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                    string json = dataReader.ReadString(numBytesLoaded);
                    mangaList = JsonConvert.DeserializeObject<List<MangaItem>>(json);
                }
            }
            stream.Dispose();
            
            return mangaList;
        }

        //Complete detail of one manga

        public static async Task<MangaItem> GetDetailAsync(string id) {
            return await FormatMangaDetailAsync(id);
        }

        private static async Task<MangaItem> FormatMangaDetailAsync(string id) {
            var unformatedMangaDetail = await UnformatedMangaDetail.GetMangaDetailAsync(id);

            var chapters = new List<ChapterListItem>();

            //zformátovaní dat
            foreach (var chapter in unformatedMangaDetail.chapters) {
                string title;
                if (Convert.ToString(chapter[0]) == (string)chapter[2]) {
                    //některé kapitoly mají místo jména pouze číslo kapitolyu
                    //to se musí odstranit, jinak by se zobrazovalo dvakrát (je už vyplněné v položce 'chapter')
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
