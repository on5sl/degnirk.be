using System;
using System.Configuration;
using System.Web.Mvc;

using DTO;

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

            var facebookService = new FacebookService(accessToken, long.Parse(ConfigurationManager.AppSettings["FacebookPageId"]));
            var user = facebookService.GetCurrentUser();
            
            Member member = Member.GetMemberFromEmail(user.Email);
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
                id = user.FacebookId,
                name = user.Name,
                email = user.Email
            });
        }

        private static void SendNewMemberMail(DeGnirkMember user)
        {
            try
            {
                library.SendMail(ConfigurationManager.AppSettings["infoEmail"],
                    "jhdegnirk@gmail.com",
                    string.Format("Nieuw lid {0} heeft zich ingeschreven via de website", user.Name),
                    string.Format("{0} heeft zich ingeschreven, dit zijn de gegevens:\r\n" +
                                  "Naam: {1}\r\n" +
                                  "Geboortedatum: {2}\r\n" +
                                  "Vervaldatum: {3}\r\n" +
                                  "Plaats: {4}",
                        user.Name,
                        user.Name,
                        user.DateOfBirth,
                        DateTime.Now.AddYears(1),
                        user.Location
                        ),
                    false);
            }
            catch (Exception exception)
            {
                LogHelper.Error<LogController>(string.Format("Notification mail for {0} not sent", user.Name), exception);
                throw;
            }
        }

        private static void MakeNewUmbracoMember(DeGnirkMember user)
        {
            // Then we make the member in Umbraco
            Member member = Member.MakeNew(user.Name, user.Email, user.Email, MemberType.GetByAlias("Member"),
                new User(0));
            member.ChangePassword(System.Web.Security.Membership.GeneratePassword(10, 2));
            member.getProperty(Birthday).Value = user.DateOfBirth;
            member.getProperty(Location).Value = user.Location;
            member.getProperty(Link).Value = user.FacebookLink;
            Member.AddMemberToCache(member);
            member.Save();

            //TODO: When the new member api is finished use this
            /*var cs = Services.ContentService;
            var content = cs.GetById(member.Id);
            content.SetValue("birthday", DateTime.Now);
            cs.PublishWithStatus(content);*/
        }
    }
}
