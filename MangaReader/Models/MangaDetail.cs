using System.Collections.Generic;

namespace MangaReader.Models {
    public class MangaDetail {
        public List<object> aka { get; set; }
        //public List<object> aka-alias { get; set; }
        public string alias { get; set; }
        public string artist { get; set; }
        public List<object> artist_kw { get; set; }
        public string author { get; set; }
        public List<object> author_kw { get; set; }
        public List<string> categories { get; set; }
        public List<List<object>> chapters { get; set; }
        public int chapters_len { get; set; }
        public double created { get; set; }
        public string description { get; set; }
        public int hits { get; set; }
        public string image { get; set; }
        public int language { get; set; }
        public double last_chapter_date { get; set; }
        public List<double> random { get; set; }
        public object released { get; set; }
        public string startsWith { get; set; }
        public int status { get; set; }
        public string title { get; set; }
        public List<string> title_kw { get; set; }
        public int type { get; set; }
        public bool updatedKeywords { get; set; }
    }
}
