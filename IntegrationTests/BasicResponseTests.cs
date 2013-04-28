using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Xunit;

namespace Gate.Adapters.AspNetMvc.IntegrationTests
{
    public class BasicResponseTests
    {
        [Fact]
        public void Content_FromQueryString() {
            var result = HttpTestHelper.GetString("/Test/ContentFromQueryString?content=ABC");

            Assert.Equal("ABC", result);
        }
    }
}
