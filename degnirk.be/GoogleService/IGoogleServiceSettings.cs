namespace Service.Google
{
    public interface IGoogleServiceSettings
    {
        /// <summary>
        /// See https://console.developers.google.com fore more info
        /// </summary>
        string ClientIDforNativeApplication { get; set; }
        string ClientSecret { get; set; }
        string Email { get; set; }
        /// <summary>
        /// See https://console.developers.google.com/project for this constant
        /// </summary>
        string ApplicationName { get; set; }
    }
}
