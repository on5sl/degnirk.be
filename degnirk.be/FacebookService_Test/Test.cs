using System;
using System.Linq;
using NUnit.Framework;
using Service;

namespace FacebookService_Test
{
    [TestFixture]
    public class Test
    {
        private const string AccessToken = "442171809217325|Q3rA6b68G7TuWYF1beRUNsLUe94";
        private const string PageId = "56615007038";
        private FacebookService _facebookService;

        [SetUp]
        public void SetUp()
        {
            _facebookService = new FacebookService(AccessToken);
        }

        [Test]
        public void Get_Latest_Event()
        {
            var facebookEvents = _facebookService.GetLatestFacebookEvents(PageId, 1);
            Assert.AreEqual(1, facebookEvents.Count());
        }

        [Test]
        public void Get_Latest_Events()
        {
            var facebookEvents = _facebookService.GetLatestFacebookEvents(PageId, 3);
            Assert.AreEqual(3, facebookEvents.Count());
        }

        [Test]
        public void Get_Albums()
        {
            var facebookAlbums = _facebookService.GetFacebookAlbums(PageId);
            Assert.IsNotNull(facebookAlbums);
        }

        [Test]
        public void Get_Last_Three_Months_Of_Events()
        {
            var facebookEvents = _facebookService.GetFacebookEvents(
                PageId,
                DateTime.Now.AddMonths(-2),
                DateTime.Now.AddMonths(1));
            Assert.IsNotNull(facebookEvents);
        }
    }
}
