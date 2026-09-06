using pocketbase.net;
using pocketbase.net.Services;
using pocketbase.net.Models.Helpers;
using System.Net;
using System.Net.Http.Json;
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

        [TestMethod]
        public async Task CreateUsesPostAndRecordsEndpoint()
        {
            var handler = new CaptureHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new { id = "record-1" })
            });
            var testClient = new Pocketbase(MockData.testUrl, new HttpClient(handler));

            await testClient.Collections(MockData.testCollName).Create<Dictionary<string, object>, object>(new { title = "hello" });

            Assert.AreEqual(HttpMethod.Post, handler.Method);
            Assert.AreEqual("/api/collections/posts/records/", handler.RequestUri!.AbsolutePath);
        }

        [TestMethod]
        public async Task PasswordAuthUsesCurrentEndpointAndStoresRecord()
        {
            var handler = new CaptureHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new
                {
                    token = "auth-token",
                    record = new { id = "user-1", email = "user@example.com" }
                })
            });
            var testClient = new Pocketbase(MockData.testUrl, new HttpClient(handler));

            var record = await testClient.Collections("users").AuthWithPassword("user@example.com", "password");

            Assert.AreEqual(HttpMethod.Post, handler.Method);
            Assert.AreEqual("/api/collections/users/auth-with-password", handler.RequestUri!.AbsolutePath);
            Assert.AreEqual("auth-token", testClient.authStore.token);
            Assert.IsTrue(testClient.authStore.isValid);
            Assert.AreEqual("user-1", record!.id);
            Assert.AreEqual("user@example.com", record.email);
        }

        private sealed class CaptureHandler(HttpResponseMessage response) : HttpMessageHandler
        {
            public HttpMethod? Method { get; private set; }
            public Uri? RequestUri { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                Method = request.Method;
                RequestUri = request.RequestUri;
                return Task.FromResult(response);
            }
        }

        //[TestMethod]
        //public void AuthTest()
        //{
        //    Assert.AreEqual("admins", auth.collectionName);
        //    Assert.AreEqual("api/admins", auth.urlBuilder.CollectionUrl());
        //}
    }
}
