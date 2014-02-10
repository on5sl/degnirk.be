using System;
using Google;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using umbraco.cms.businesslogic.member;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var googleApi = new GoogleApi();
            var events = googleApi.GetEvents(DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(2));
        }
    }
}
