using System.Collections.Generic;

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
                return false;
            }
        }
        public int unreadChapters {
            get {
                return 0;
            }
        }
    }
}
