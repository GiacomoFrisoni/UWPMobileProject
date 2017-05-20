using System;
using Windows.ApplicationModel.Resources;

/// <summary>
/// Multithreaded Singleton to handle server bad request.
/// </summary>
namespace MyPoetry
{
    public sealed class ServerErrorInfo
    {
        private static volatile ServerErrorInfo instance;
        private static object syncRoot = new Object();
        
        private ServerErrorInfo() { }

        /// <summary>
        /// Gets the Singleton instance.
        /// If the instance does not exist, it creates a new one.
        /// </summary>
        public static ServerErrorInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ServerErrorInfo();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the localed description associated to an error code provided by
        /// the server.
        /// </summary>
        /// <param name="error">Error code</param>
        /// <returns>Error info</returns>
        public String GetInfo(String error)
        {
            var loader = new ResourceLoader();
            switch (error)
            {
                case "ERR_R_1":
                    return loader.GetString("Err_MissingData");
                case "ERR_R_2":
                    return loader.GetString("Err_InvalidPw");
                case "ERR_R_3":
                    return loader.GetString("Err_InvalidPws");
                case "ERR_R_4":
                    return loader.GetString("Err_InvalidEmail");
                case "ERR_R_5":
                    return loader.GetString("Err_InvalidGender");
                case "ERR_R_6":
                    return loader.GetString("Err_InvalidUsername");
                case "ERR_L_1":
                    return loader.GetString("Err_InvalidUsernamePw");
                case "ERR_P_1":
                    return loader.GetString("Err_InvalidEmail");
                case "ERR_P_2":
                    return loader.GetString("Err_NotAssociatedEmail");
                default:
                    return String.Empty;
            }
        }
    }
}
