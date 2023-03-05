using System.Net.Security;
using Microsoft.Extensions.DependencyInjection;
using pocketbase.net;
using pocketbase.net.Models.Helpers;
using pocketbase.net.Services;
using Tests.Mock;

namespace Tests
{
    [TestClass]
    public class PocketTest
    {
        public Pocketbase cleint { get; set; } = default!;
        public RecordService postRecord = default!;
        public CollectionService collections = default!;

        [TestInitialize]
        public void Setup()
        {
            cleint = new Pocketbase(MockData.testUrl, null, null);
            postRecord = cleint.Collections(MockData.testCollName);
            collections = cleint.collection;
            // auth = cleint.authStore;
        }


        [TestMethod]
        public void TestCleint()
        {

            Assert.AreEqual(MockData.testUrl.ToString(), cleint.baseurl);
            Assert.AreEqual(MockData.testLang.ToString(), cleint.lang);
            Assert.IsTrue(cleint.collection is not null and CollectionService);
        }

        [TestMethod]
        public void TestRecord()
        {
            Assert.AreEqual($"api/collections/{MockData.testCollName}/records/", postRecord.urlBuilder.CollectionUrl());
        }

        [TestMethod]
        public void TestCollection()
        {
            var actual = collections._baseService.urlBuilder.CollectionUrl();
            Assert.AreEqual("api/collections/", actual);
            Assert.AreEqual(string.Empty, collections._baseService.collectionName);
        }

        //[TestMethod]
        //public void AuthTest()
        //{
        //    Assert.AreEqual("admins", auth.collectionName);
        //    Assert.AreEqual("api/admins", auth.urlBuilder.CollectionUrl());
        //}
    }
}
