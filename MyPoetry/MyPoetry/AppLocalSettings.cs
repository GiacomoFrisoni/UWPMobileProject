namespace MyPoetry
{
    /// <summary>
    /// This class handles the local settings of the application.
    /// </summary>
    class AppLocalSettings
    {
        public const string SALT_KEY = "salt";

        private Windows.Storage.ApplicationDataContainer localSettings;

        /// <summary>
        /// Local settings retrievement.
        /// </summary>
        public AppLocalSettings()
        {
            this.localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        }

        /// <summary>
        /// Return the salt associated to the user login.
        /// </summary>
        /// <returns>User's salt</returns>
        public byte[] getSalt()
        {
            return localSettings.Values[SALT_KEY] != null ? (byte[])localSettings.Values[SALT_KEY] : null;
        }

        /// <summary>
        /// Sets the salt for the login keeping.
        /// </summary>
        /// <param name="salt">Salt</param>
        public void setSalt(byte[] salt)
        {
            localSettings.Values[SALT_KEY] = salt;
        }
    }
}
