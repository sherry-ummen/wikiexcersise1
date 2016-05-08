using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wiki
{
    public class WikiParser
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string WikiURI { get; private set; }
        public WikiParser(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            WikiURI = string.Format("https://en.wikipedia.org/w/api.php?action=query&list=geosearch&gsradius=10000&gscoord={0}%7C{1}&gslimit=50&format=json", Latitude, Longitude);
        }

        public Result<SearchResult> Parse()
        {
            return WebRequestHandler.GetResponseFrom(new Uri(WikiURI))
                .OnSuccess((x) => JObject.Parse(x.Value).ToObject<SearchResult>().ToResult());
        }

        public Result<List<Article>> GetArticlesWithImages(SearchResult searchResult)
        {
            try
            {
                if (searchResult.Query == null)
                {
                    throw new ArgumentException("Search Result is empty!");
                }
                Uri query = GetWikiImageQuery(searchResult.Query.Geosearch.Select(x => x.Pageid));
                return WebRequestHandler.GetResponseFrom(query).OnSuccess<string, List<Article>>((x) => ParseToArticles(x.Value));
            }
            catch (Exception exception)
            {
                return Result.Fail<List<Article>>(exception.Message);
            }
        }

        private Result<List<Article>> ParseToArticles(string jsonString)
        {
            try
            {   // Memo: Wiki Image API is very brittle. Its a headache!
                var queryPages = JObject.Parse(jsonString).SelectToken(string.Format("query.pages"));
                if (queryPages == null)
                {
                    throw new Exception("Query returned nothing!");
                }
                List<Article> listOfArticles = new List<Article>();
                foreach (var value in queryPages.Values().Where(value => value.SelectToken("images") != null))
                {
                    var article = new Article();
                    article.Title = value.SelectToken("title").ToString();
                    article.PageId = value.SelectToken("pageid").ToString();
                    var listOfImages = new List<string>();
                    listOfImages.AddRange(value.Last.Values().Select(v => v.Last.Last.ToString()));
                    article.Images = listOfImages;
                    listOfArticles.Add(article);
                }
                return Result.Ok<List<Article>>(listOfArticles);
            }
            catch (Exception exception)
            {
                return Result.Fail<List<Article>>("Parsing to Articles failed with error:\n" + exception.Message);
            }
        }

        private Uri GetWikiImageQuery(IEnumerable<int> pageIds)
        {
            var listOfPageIds = string.Join("|", pageIds.ToArray());
            Uri result = new Uri(string.Format("https://en.wikipedia.org/w/api.php?action=query&prop=images&pageids={0}&format=json&imlimit=max", listOfPageIds));
            return result;
        }

    }
}
