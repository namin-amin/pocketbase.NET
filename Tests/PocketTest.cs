﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Mock;
using pocketbase.net;
using pocketbase.net.Services;

namespace Tests
{
    [TestClass]
    public class PocketTest
    {
        public Pocketbase cleint { get; set; } = default!;
        public RecordService postRecord = default!;

        [TestInitialize]
        public void Setup()
        {
            cleint = new Pocketbase(MockData.testUrl, null, null);
            postRecord = cleint.Collections(MockData.testCollName);
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
    }
}