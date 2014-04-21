﻿using System;
using System.Linq;
using NUnit.Framework;
using Services.Facebook;
using DTO;

namespace FacebookService_Test
{
    [TestFixture]
    public class Test
    {
        private const string AccessToken = "442171809217325|Q3rA6b68G7TuWYF1beRUNsLUe94";
        private const long PageId = 56615007038;
        private IFacebookService _facebookService;

        [SetUp]
        public void SetUp()
        {
            _facebookService = new FacebookService(AccessToken, PageId);
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
            var facebookAlbums = _facebookService.GetFacebookAlbums(PageId);
            Assert.IsNotNull(facebookAlbums);
        }

        [Test]
        public void Get_Last_Three_Months_Of_Events()
        {
            var facebookEvents = _facebookService.GetEvents(DateTime.Now.AddMonths(-2),
                DateTime.Now.AddMonths(1));
            Assert.IsNotNull(facebookEvents);
        }
    }
}
