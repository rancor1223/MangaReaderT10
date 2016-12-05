namespace MangaReader.Models {
    public class ChapterListItem {
        public string number { get; set; }
        public string title { get; set; }
        public string id { get; set; }

        public ChapterListItem(string n, string t, string i) {
            number = n;
            title = t;
            id = i;
        }
    }
}
