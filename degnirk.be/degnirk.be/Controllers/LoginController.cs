using System;
using System.Configuration;
using System.Web.Mvc;

using DTO;

using Services.Facebook;
using umbraco;

using Umbraco.Core.Logging;
using Umbraco.Core.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace degnirk.be.Controllers
{
    public class LoginController : SurfaceController
    {
        private readonly IMemberService _memberService;
        private readonly IMemberTypeService _memberTypeService;

        public LoginController()
        {
            _memberService = this.Services.MemberService;
            _memberTypeService = this.Services.MemberTypeService;
        }

        private const string Birthday = "birthday";
        private const string Location = "location";
        private const string Link = "link";
        private const string LastLogin = "umbracoLastLoginPropertyTypeAlias";

        public ActionResult UserInfo(string accessToken)
        {
            HttpContext.Session["AccessToken"] = accessToken;

            var facebookService = new FacebookService(accessToken, long.Parse(ConfigurationManager.AppSettings["FacebookPageId"]));
            var user = facebookService.GetCurrentUser();
            
            var member = _memberService.GetByEmail(user.Email);
            if (member == null)
            {
                MakeNewUmbracoMember(user);
                SendNewMemberMail(user);
            }
            else
            {
                member.LastLoginDate = DateTime.Now;
                _memberService.Save(member);
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

        private void MakeNewUmbracoMember(DeGnirkMember user)
        {
            var member = _memberService.CreateMember(user.Name, user.Email, user.Name, _memberTypeService.Get("Member"));
            member.SetValue(Birthday, user.DateOfBirth);
            member.SetValue(Location, user.Location);
            member.SetValue(Link, user.FacebookLink);
            _memberService.Save(member);
        }
    }
}
