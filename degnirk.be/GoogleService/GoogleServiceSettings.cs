using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GoogleServiceSettings : IGoogleServiceSettings
    {
        public string ClientIDforNativeApplication { get; set; }
        public string ClientSecret { get; set; }
        public string Email { get; set; }
        public string ApplicationName { get; set; }

        public GoogleServiceSettings(string ClientIDforNativeApplication, string ClientSecret, string Email, string ApplicationName)
        {
            this.ClientIDforNativeApplication = ClientIDforNativeApplication;
            this.ClientSecret = ClientSecret;
            this.Email = Email;
            this.ApplicationName = ApplicationName;
        }
    }
}
