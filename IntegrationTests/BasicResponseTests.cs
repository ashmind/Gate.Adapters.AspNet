using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Gate.Adapters.AspNet.IntegrationTests.Internal;
using Xunit;

namespace Gate.Adapters.AspNet.IntegrationTests {
    public class BasicResponseTests {
        [Fact]
        public void Content_FromQueryString() {
            var result = HttpTestHelper.GetString("/Test/ContentFromQueryString?content=ABC");
            Assert.Equal("ABC", result);
        }

        [Fact]
        public void Content_FromForm() {
            var form = new Dictionary<string, string> {
                { "content", "ABC" }
            };
            var result = HttpTestHelper.PostAndGetString("/Test/ContentFromForm", new FormUrlEncodedContent(form));
            Assert.Equal("ABC", result);
        }

        [Fact]
        public void Content_FromApplicationData() {
            var result = HttpTestHelper.GetString("/Test/ContentFromApplicationData");
            Assert.Equal(TestHost.TestApplicationDataValue, result);
        }

        [Fact]
        public void View_FromQueryString() {
            var result = HttpTestHelper.GetString("/Test/ViewFromQueryString?content=Unicorn");
            Assert.Contains("Unicorn", result);
        }
    }
}
