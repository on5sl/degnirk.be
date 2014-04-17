using System;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using Services.Facebook;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.member;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class LoginController : SurfaceController
    {
        private const string Birthday = "birthday";
        private const string Location = "location";
        private const string Link = "link";
        private const string LastLogin = "umbracoLastLoginPropertyTypeAlias";

        public ActionResult UserInfo(string accessToken)
        {
            HttpContext.Session["AccessToken"] = accessToken;

            var facebookService = new FacebookService(accessToken);
            var user = facebookService.GetCurrentUser();
            
            Member member = Member.GetMemberFromEmail(user.email);
            if (member == null)
            {
                MakeNewUmbracoMember(user);
                SendNewMemberMail(user);
            }
            else
            {
                member.getProperty(LastLogin).Value = DateTime.Now.ToString();
                member.Save();
            }

            return Json(new
            {
                id = user.id,
                name = user.name,
                email = user.email
            });
        }

        private static void SendNewMemberMail(dynamic user)
        {
            try
            {
                library.SendMail(ConfigurationManager.AppSettings["infoEmail"],
                    "jhdegnirk@gmail.com",
                    string.Format("Nieuw lid {0} heeft zich ingeschreven via de website", user.name),
                    string.Format("{0} heeft zich ingeschreven, dit zijn de gegevens:\r\n" +
                                  "Naam: {1}\r\n" +
                                  "Geboortedatum: {2}\r\n" +
                                  "Vervaldatum: {3}\r\n" +
                                  "Plaats: {4}",
                        user.name,
                        user.name,
                        ParseFacebookBirthday(user),
                        DateTime.Now.AddYears(1),
                        user.location["name"]
                        ),
                    false);
            }
            catch (Exception exception)
            {
                LogHelper.Error<LogController>(string.Format("Notification mail for {0} not sent", user.name), exception);
                throw;
            }
        }

        private static void MakeNewUmbracoMember(dynamic user)
        {
            // Then we make the member in Umbraco
            Member member = Member.MakeNew(user.name, user.email, user.email, MemberType.GetByAlias("Member"),
                new User(0));
            member.ChangePassword(System.Web.Security.Membership.GeneratePassword(10, 2));
            member.getProperty(Birthday).Value = ParseFacebookBirthday(user);
            member.getProperty(Location).Value = user.location["name"];
            member.getProperty(Link).Value = user.link;
            Member.AddMemberToCache(member);
            member.Save();

            //TODO: When the new member api is finished use this
            /*var cs = Services.ContentService;
            var content = cs.GetById(member.Id);
            content.SetValue("birthday", DateTime.Now);
            cs.PublishWithStatus(content);*/
        }

        private static DateTime ParseFacebookBirthday(dynamic user)
        {
            try
            {
                return DateTime.ParseExact(user.birthday, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None);
            }
            catch (Exception exception)
            {
                LogHelper.Error<LogController>(string.Format("The format of {0} could not be parsed to the birthday date for {1}.", user.birthday, user.name), exception);
                return DateTime.MinValue;
            }
        }
    }
}
