using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wiki.Tests
{
    [TestClass()]
    public class ResultExtensionsTests
    {
        [TestMethod()]
        public void OnSuccessGenricResultTest()
        {
            Result<bool> result = Result.Ok<bool>(true);
            Result<bool> t = result.OnSuccess((x) => Result.Ok<bool>(true));
            Assert.IsTrue(t.IsSuccess);
        }
    }
}