namespace Services.Google
{
    public class GoogleServiceSettings : IGoogleServiceSettings
    {
        public string ClientIDforNativeApplication { get; set; }
        public string ClientSecret { get; set; }
        public string Email { get; set; }
        public string ApplicationName { get; set; }

        public GoogleServiceSettings(string clientIDforNativeApplication, string clientSecret, string email, string applicationName)
        {
            this.ClientIDforNativeApplication = clientIDforNativeApplication;
            this.ClientSecret = clientSecret;
            this.Email = email;
            this.ApplicationName = applicationName;
        }
    }
}
