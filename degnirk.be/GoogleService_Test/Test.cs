using System;
using NUnit.Framework;
using Service;

namespace GoogleService_Test
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void Get_Last_Three_Months_Of_Events()
        {
            var googleService = new GoogleService();
            var events = googleService.GetEvents(DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(1));
            Assert.IsNotNull(events);
        }
    }
}
