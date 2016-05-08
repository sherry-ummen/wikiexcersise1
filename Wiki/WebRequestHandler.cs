using System;
using System.IO;
using System.Net;

namespace Wiki {
    public static class WebRequestHandler {

        public static Result<string> GetResponseFrom(Uri uri) {
            try {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(uri);
                string response;
                using (HttpWebResponse webResponse = (HttpWebResponse)myRequest.GetResponse()) {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream())) {
                        response = reader.ReadToEnd();
                    }
                }
                return Result.Ok(response);
            } catch (Exception exception) {
                return Result.Fail<string>("Failed to get response with error:\n" + exception.Message);
            }
        }
    }
}
