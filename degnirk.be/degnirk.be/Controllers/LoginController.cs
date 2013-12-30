using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using Facebook;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.member;
using Umbraco.Core;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class LoginController : SurfaceController
    {
        public ActionResult UserInfo(string accessToken)
        {
            HttpContext.Session["AccessToken"] = accessToken;

            var client = new FacebookClient(accessToken);
            dynamic result = client.Get("me", new { fields = "name,id,email" });


            var member = Member.MakeNew(result.name, result.email, result.email, MemberType.GetByAlias("Member"), new User(0));
            Member.AddMemberToCache(member);

            return Json(new
            {
                id = result.id,
                name = result.name,
                email = result.email
            });
        }

        private void MakeNewUmbracoMember(string firstName, string lastName, string email, string location, string birthday)
        {
            // First we check if the user doesn't already exist
            // Then we make the member in Umbraco
        }
    }
}
