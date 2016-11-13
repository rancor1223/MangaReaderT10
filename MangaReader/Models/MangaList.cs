using System.Collections.Generic;

namespace MangaReader.Models {

    public class MangaListItem {
        public List<object> category { get; set; }
        public string id { get; set; }
        public string image { get; set; }
        public int status { get; set; }
        public string title { get; set; }
        public string alias { get; set; }
    }

    public class UnformatedMangaListItem {
        public string a { get; set; }
        public List<object> c { get; set; }
        public int h { get; set; }
        public string i { get; set; }
        public string im { get; set; }
        public double ld { get; set; }
        public int s { get; set; }
        public string t { get; set; }
    }

    public class MangaListRootObject {
        public int end { get; set; }
        public List<UnformatedMangaListItem> manga { get; set; }
        public int page { get; set; }
        public int start { get; set; }
        public int total { get; set; }
    }
}
