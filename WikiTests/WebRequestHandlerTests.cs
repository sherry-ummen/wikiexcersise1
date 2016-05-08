using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Wiki.Tests {
    [TestClass()]
    public class WebRequestHandlerTests {
        [TestMethod()]
        public void GetResponseFromTest(){
            Result<string> response = WebRequestHandler.GetResponseFrom(new Uri("https://localhost:9999"));
            Assert.IsTrue(response.IsFailure);
        }
    }
}