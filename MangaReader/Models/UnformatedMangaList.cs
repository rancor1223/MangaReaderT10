using System.Collections.Generic;

namespace MangaReader.Models {
    public class UnformatedMangaList {
        public int end { get; set; }
        public List<UnformatedMangaListItem> manga { get; set; }
        public int page { get; set; }
        public int start { get; set; }
        public int total { get; set; }
    }
}
