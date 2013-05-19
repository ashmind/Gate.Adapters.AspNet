using System;
using System.Collections.Generic;
using System.Linq;
using Gate.Adapters.AspNet.IntegrationTests.Internal;
using Xunit;

namespace Gate.Adapters.AspNet.IntegrationTests {
    public class FileTests {
        [Fact]
        public void File_WithTextContent_FromController() {
            var result = HttpTestHelper.GetString("/Test/FileWithTextContent");
            Assert.Equal("Test data from file.", result);
        }

        [Fact]
        public void File_WithTextContent_FromDirectRequest() {
            var result = HttpTestHelper.GetString("/Content/Test.txt");
            Assert.Equal("Test data from file.", result);
        }
    }
}
