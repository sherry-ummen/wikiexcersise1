using System;
using System.Collections.Generic;
using System.Linq;

namespace Wiki
{
    public class Article
    {
        public string PageId { get; set; }
        public string Title { get; set; }
        public List<string> Images { get; set; }
    }

    public static class Similarities
    {
        public static Result<IDictionary<string, Dictionary<string, Article>>> Find(IList<Article> articles)
        {
            try
            {
                //TODO: Optimize it by finding better algorithm
                IDictionary<string, Dictionary<string, Article>> similarities = new Dictionary<string, Dictionary<string, Article>>();
                foreach (var item in articles)
                {
                    foreach (var image in item.Images)
                    {
                        foreach (var ht in articles)
                        {
                            //TODO: Evaluate Phonetic algorithm as well for more accurate results?
                            foreach (var image1 in ht.Images.Where(image1 => image != image1 && image.CalculateSimilarity(image1) >= 0.9))
                            {
                                if (similarities.ContainsKey(image))
                                {
                                    if (!similarities[image].ContainsKey(image1))
                                    {
                                        similarities[image][image1] = ht;
                                    }
                                }
                                else
                                {
                                    similarities[image] = new Dictionary<string, Article>();
                                    similarities[image][image1] = ht;
                                }
                            }
                        }

                    }

                }
                var er = similarities.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, y => y.Value);
                return Result.Ok<IDictionary<string, Dictionary<string, Article>>>(er);
            }
            catch (Exception exception)
            {
                return Result.Fail<IDictionary<string, Dictionary<string, Article>>>("Failed to find similarities with error:\n" + exception.Message);
            }
        }
    }
}
