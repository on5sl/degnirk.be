using System;
using System.Linq;
using NUnit.Framework;
using Service;

namespace FacebookService_Test
{
    [TestFixture]
    public class Test
    {
        private FacebookService _facebookService;

        [SetUp]
        public void SetUp()
        {
            _facebookService = new FacebookService();
        }

        [Test]
        public void Get_Latest_Event()
        {
            var facebookEvents = _facebookService.GetLatestFacebookEvents(1);
            Assert.AreEqual(1, facebookEvents.Count());
        }

        [Test]
        public void Get_Latest_Events()
        {
            var facebookEvents = _facebookService.GetLatestFacebookEvents(3);
            Assert.AreEqual(3, facebookEvents.Count());
        }

        [Test]
        public void Get_Albums()
        {
            var facebookAlbums = _facebookService.GetFacebookAlbums();
            Assert.IsNotNull(facebookAlbums);
        }

        [Test]
        public void Get_Last_Three_Months_Of_Events()
        {
            var facebookEvents = _facebookService.GetFacebookEvents(DateTime.Now.AddMonths(-2),
                DateTime.Now.AddMonths(1));
            Assert.IsNotNull(facebookEvents);
        }
    }
}
