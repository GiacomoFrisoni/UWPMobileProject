namespace MyPoetry
{
    /// <summary>
    /// This class handles the local settings of the application.
    /// </summary>
    class AppLocalSettings
    {
        public const string USER_KEY = "user";

        private Windows.Storage.ApplicationDataContainer localSettings;

        /// <summary>
        /// Local settings retrievement.
        /// </summary>
        public AppLocalSettings()
        {
            this.localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        }

        /// <summary>
        /// Return the id of the logged user.
        /// </summary>
        /// <returns>User logged id</returns>
        public string GetUserLoggedId()
        {
            return localSettings.Values.ContainsKey(USER_KEY) ? (string)localSettings.Values[USER_KEY] : string.Empty;
        }

        /// <summary>
        /// Sets the user id for the login keeping.
        /// </summary>
        /// <param name="id">User logged id</param>
        public void SetUserLoggedId(string id)
        {
            localSettings.Values[USER_KEY] = id;
        }
    }
}
