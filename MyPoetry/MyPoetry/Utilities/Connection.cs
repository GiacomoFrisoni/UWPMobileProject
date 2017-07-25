using Windows.Networking.Connectivity;

namespace MyPoetry.Utilities
{
    public class Connection
    {
        public static bool HasInternetAccess { get; private set; }

        public Connection()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            CheckInternetAccess();
        }

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            CheckInternetAccess();
        }

        private void CheckInternetAccess()
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            HasInternetAccess = (connectionProfile != null &&
                connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }
    }
}
