using System;
using NUnit.Framework;
using Service;
using DTO;

namespace GoogleService_Test
{
    [TestFixture]
    public class Test
    {
        private GoogleService _googleService;
        // See https://console.developers.google.com fore more info
        private const string ClientIDforNativeApplication = "539942760355-ndbtaf1pi3iih0o31m6sh47vh16gr8h4.apps.googleusercontent.com";
        private const string ClientSecret = "L4XjviYSsciNU1TpvExREotB";
        private const string Email = "jhdegnirk@gmail.com";

        // See https://console.developers.google.com/project for this constant
        private const string ApplicationName = "test1";

        [SetUp]
        public void SetUp()
        {
            _googleService = new GoogleService(new GoogleServiceSettings(ClientIDforNativeApplication, ClientSecret, Email, ApplicationName));
        }

        [Test]
        public void Get_Last_Three_Months_Of_Events()
        {
            var events = _googleService.GetEvents(DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(1));
            Assert.IsNotNull(events);
        }
    }
}
