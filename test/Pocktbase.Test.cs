using pocketbase.net;
using pocketbase.net.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Mock;

namespace test
{
    public class PocketbaseTest
    {
        public Pocketbase cleint { get; set; } = default!;

        [SetUp]
        public void Setup()
        {
            cleint = new Pocketbase(MockData.testUrl, null, null);
        }

        [Test]
        public void TestCleint()
        {
            Assert.That(cleint.baseurl, Is.EqualTo(MockData.testUrl));
            Assert.That(cleint.lang , Is.EqualTo(MockData.testLang));
            Assert.That(cleint.collection is CollectionService);
        }
    }
}
