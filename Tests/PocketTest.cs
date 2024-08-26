using pocketbase.net;
using pocketbase.net.Services;
using Tests.Mock;

namespace Tests
{
    [TestClass]
    public class PocketTest
    {
        private Pocketbase client { get; set; } = default!;
        private RecordService _postRecord = default!;
        private CollectionService _collections = default!;

        [TestInitialize]
        public void Setup()
        {
            client = new Pocketbase(MockData.testUrl, null, null);
            _postRecord = client.Collections(MockData.testCollName);
            _collections = client.collection;
            // auth = client.authStore;
        }


        [TestMethod]
        public void TestClient()
        {

            Assert.AreEqual(MockData.testUrl, client.baseurl);
            Assert.AreEqual(MockData.testLang, client.lang);
            Assert.IsTrue(client.collection is not null and not null);
        }

        [TestMethod]
        public void TestRecord()
        {
            Assert.AreEqual($"api/collections/{MockData.testCollName}/records/", _postRecord.UrlBuilder.CollectionUrl());
        }

        [TestMethod]
        public void TestCollection()
        {
            var actual = _collections._baseService.UrlBuilder.CollectionUrl();
            Assert.AreEqual("api/collections/", actual);
            Assert.AreEqual(string.Empty, _collections._baseService.CollectionName);
        }

        //[TestMethod]
        //public void AuthTest()
        //{
        //    Assert.AreEqual("admins", auth.collectionName);
        //    Assert.AreEqual("api/admins", auth.urlBuilder.CollectionUrl());
        //}
    }
}
