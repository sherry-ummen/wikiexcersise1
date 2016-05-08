using Wiki;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Take lattitude and longitude as command line argument
            var wikiParser = new WikiParser(12.9141, 74.856);
            var result = wikiParser.Parse();
            var articles = wikiParser.GetArticlesWithImages(result.Value);
            var sim = Similarities.Find(articles.Value);
            foreach (var similarity in sim.Value)
            {
                System.Console.WriteLine("ImageId: {0} | Similarity found: {1}", similarity.Key, similarity.Value.Count);
            }
            System.Console.ReadKey();
        }
    }
}
