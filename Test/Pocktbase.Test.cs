using pocketbase.net;
using pocketbase.net.Services;
using test.Mock;


namespace Test
{

    public class PocketbaseTest
    {
        public Pocketbase cleint { get; set; } = default!;
        public RecordService postRecord = default!;

        //[initt]
        public void Setup()
        {
            cleint = new Pocketbase(MockData.testUrl, null, null);
            postRecord = cleint.Collections(MockData.testCollName);
        }

        //[Test]
        public void TestCleint()
        {
            Assert.Multiple(() =>
            {
                Assert.That(cleint.baseurl, Is.EqualTo(MockData.testUrl));
                Assert.That(cleint.lang, Is.EqualTo(MockData.testLang));
                Assert.That(cleint.collection is not null and CollectionService);
            });
        }

        //[Test]
        public void TestRecord()
        {
            Assert.Multiple(() =>
            {
                Assert.That(postRecord.urlBuilder.CollectionUrl(), Is.EqualTo($"{MockData.testUrl}/api/{MockData.testCollName}/records"));

                Assert.That(postRecord.collectionName, Is.EqualTo(MockData.testCollName));
                //Assert.That(postRecord, Is.EqualTo(MockData.testCollName));
            });
        }
    }
}
