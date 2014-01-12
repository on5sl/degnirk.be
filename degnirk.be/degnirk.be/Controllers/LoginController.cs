using System;
using System.Globalization;
using System.Web.Mvc;
using Facebook;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.member;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class LoginController : SurfaceController
    {
        private const string name = "name";
        private const string id = "id";
        private const string email = "email";
        private const string birthday = "birthday";
        private const string location = "location";
        private const string link = "link";
        private const string lastLogin = "umbracoLastLoginPropertyTypeAlias";

        public ActionResult UserInfo(string accessToken)
        {
            HttpContext.Session["AccessToken"] = accessToken;

            var client = new FacebookClient(accessToken);
            dynamic user = client.Get("me", new { fields = string.Format("{0},{1},{2},{3},{4},{5}", name, id, email, birthday, location, link) });

            MakeNewUmbracoMember(user);

            return Json(new
            {
                id = user.id,
                name = user.name,
                email = user.email
            });
        }

        private static void MakeNewUmbracoMember(dynamic user)
        {
            // First we check if the user doesn't already exist
            Member member = Member.GetMemberFromEmail(user.email);
            if (member == null)
            {
                // Then we make the member in Umbraco
                member = Member.MakeNew(user.name, user.email, user.email, MemberType.GetByAlias("Member"),
                    new User(0));
                member.ChangePassword(System.Web.Security.Membership.GeneratePassword(10, 2));
                member.getProperty(birthday).Value = ParseFacebookBirthday(user.birthday, user.name);
                member.getProperty(location).Value = ((Facebook.JsonObject) user.location)["name"];
                member.getProperty(link).Value = user.link;
                Member.AddMemberToCache(member);
                member.Save();

                //TODO: When the new member api is finished use this
                /*var cs = Services.ContentService;
                var content = cs.GetById(member.Id);
                content.SetValue("birthday", DateTime.Now);
                cs.PublishWithStatus(content);*/
            }
            else
            {
                member.getProperty(lastLogin).Value = DateTime.Now;
                member.Save();
            }
        }

        private static DateTime ParseFacebookBirthday(string dateTime,string s)
        {
            try
            {
                return DateTime.ParseExact(dateTime, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None);
            }
            catch (Exception exception)
            {
                LogHelper.Error<LogController>(string.Format("The format of {0} could not be parsed to the birthday date for {1}.", dateTime, s), exception);
                return DateTime.MinValue;
            }
        }
    }
}
