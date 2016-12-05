using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaReader.Models {
    public class MangaChapterView {
        public string title { get; set; }
        public string chapterId { get; set; }

        public MangaChapterView(string t, string i) {
            title = t;
            chapterId = i;
        }
    }
}
