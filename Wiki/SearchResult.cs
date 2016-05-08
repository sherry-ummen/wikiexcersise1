using System.Collections.Generic;

namespace Wiki
{
    public class SearchResult
    {
        public string Batchcomplete { get; set; }
        public Query Query { get; set; }
    }

    public class Query {
        public IList<Geosearch> Geosearch { get; set; }
    }

    public class Geosearch {
        public int Pageid { get; set; }
        public int Ns { get; set; }
        public string Title { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Dist { get; set; }
        public string Primary { get; set; }
    }

}
