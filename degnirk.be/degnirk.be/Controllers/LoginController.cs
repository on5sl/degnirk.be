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
            dynamic user = client.Get("me", new { fields = "name,id,email" });

            this.MakeNewUmbracoMember(user);

            return Json(new
            {
                id = user.id,
                name = user.name,
                email = user.email
            });
        }

        private void MakeNewUmbracoMember(dynamic user)
        {
            // First we check if the user doesn't already exist
            if (!Member.IsMember(user.email))
            {
                // Then we make the member in Umbraco
                var member = Member.MakeNew(user.name, user.email, user.email, MemberType.GetByAlias("Member"), new User(0));
                Member.AddMemberToCache(member);
            }
            
        }
    }
}
