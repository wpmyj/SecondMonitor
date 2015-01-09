using System;
using System.Management;

namespace HYMonitors.Test
{
    class Class1
    {
        //public string CreateWebSite(string serverID, string serverComment, string defaultVrootPath, string HostName, string IP, string Port)
        //{
        //    try
        //    {
        //        ManagementObject oW3SVC = new ManagementObject(_scope, new ManagementPath(@"IIsWebService='W3SVC'"), null);

        //        if (IsWebSiteExists(serverID))
        //        {
        //            return "Site Already Exists...";
        //        }

        //        ManagementBaseObject inputParameters = oW3SVC.GetMethodParameters("CreateNewSite");
        //        ManagementBaseObject[] serverBinding = new ManagementBaseObject[1];
        //        serverBinding[0] = CreateServerBinding(HostName, IP, Port);
        //        inputParameters["ServerComment"] = serverComment;
        //        inputParameters["ServerBindings"] = serverBinding;
        //        inputParameters["PathOfRootVirtualDir"] = defaultVrootPath;
        //        inputParameters["ServerId"] = serverID;

        //        ManagementBaseObject outParameter = null;
        //        outParameter = oW3SVC.InvokeMethod("CreateNewSite", inputParameters, null);

        //        // 启动网站
        //        string serverName = "W3SVC/" + serverID;
        //        ManagementObject webSite = new ManagementObject(_scope, new ManagementPath(@"IIsWebServer='" + serverName + "'"), null);
        //        webSite.InvokeMethod("Start", null);

        //        return (string)outParameter.Properties["ReturnValue"].Value;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        private static ManagementObject CreateServerBinding(ManagementScope scope, string hostName, string ip, int port)
        {
            ManagementClass mc = new ManagementClass(scope, new ManagementPath("ServerBinding"), null);
            ManagementObject mco = mc.CreateInstance();
            mco.Properties["Hostname"].Value = hostName;
            mco.Properties["IP"].Value = ip;
            mco.Properties["Port"].Value = port;
            mco.Put();

            return mco;
        }


        public static string CreateWebsite(string serverName, string appPoolName, string ip,
            string pathToRoot, string hostName, string domainName, int port)
        {
            ConnectionOptions options = new ConnectionOptions();
            options.Authentication = AuthenticationLevel.Connect;
            options.EnablePrivileges = true;
            options.Impersonation = ImpersonationLevel.Impersonate;
            ManagementScope scope = new ManagementScope(string.Format(@"\\{0}\root\MicrosoftIISv2", serverName), options);
            scope.Connect();
            ManagementObject oW3SVC = new ManagementObject(scope, new ManagementPath(@"IIsWebService='W3SVC'"), null);

            ManagementBaseObject[] serverBindings = new ManagementBaseObject[1];
            serverBindings[0] = CreateServerBinding(scope, string.Format("{0}.{1}", hostName, domainName), ip, port);
            ManagementBaseObject inputParameters = oW3SVC.GetMethodParameters("CreateNewSite");
            inputParameters["ServerComment"] = string.Format("{0}.{1}", hostName, domainName);
            inputParameters["ServerBindings"] = serverBindings;
            inputParameters["PathOfRootVirtualDir"] = pathToRoot;
            ManagementBaseObject outParameter = oW3SVC.InvokeMethod("CreateNewSite", inputParameters, null);

            string siteId = Convert.ToString(outParameter.Properties["ReturnValue"].Value).Replace("IIsWebServer='W3SVC/", "").Replace("'", "");
            ManagementObject oWebVirtDir = new ManagementObject(scope,
            new ManagementPath(string.Format(@"IIsWebVirtualDirSetting.Name='W3SVC/{0}/root'", siteId)), null);
            oWebVirtDir.Properties["AppFriendlyName"].Value = string.Format("{0}.{1}", hostName, domainName);
            oWebVirtDir.Properties["AccessRead"].Value = true;
            oWebVirtDir.Properties["AuthFlags"].Value = 5; // Integrated Windows Auth.
            oWebVirtDir.Properties["AccessScript"].Value = true;
            oWebVirtDir.Properties["AuthAnonymous"].Value = true;
            oWebVirtDir.Properties["AppPoolId"].Value = appPoolName;
            oWebVirtDir.Put();

            ManagementObject site = new ManagementObject(scope,
              new ManagementPath(Convert.ToString(
              outParameter.Properties["ReturnValue"].Value)), null);
            site.InvokeMethod("Start", null);
            return siteId;
        }

        public static void AddHostHeader(string serverName, string hostHeader,
            string ip, int port, string websiteID)
        {
            ManagementScope scope = new ManagementScope(string.Format(@"\\{0}\root\MicrosoftIISV2", serverName));
            scope.Connect();

            string siteName = string.Format("'W3SVC/{0}'", websiteID);

            ManagementObject mo = new ManagementObject(scope,
              new System.Management.ManagementPath("IIsWebServerSetting=" + siteName), null);
            ManagementBaseObject[] websiteBindings =
              (ManagementBaseObject[])mo.Properties["ServerBindings"].Value;

            ManagementObject mco = CreateServerBinding(scope, hostHeader, ip, port);

            ManagementBaseObject[] newWebsiteBindings =
              new ManagementBaseObject[websiteBindings.Length + 1];
            websiteBindings.CopyTo(newWebsiteBindings, 0);
            newWebsiteBindings[newWebsiteBindings.Length - 1] = mco;

            mo.Properties["ServerBindings"].Value = newWebsiteBindings;

            mo.Put();
        }


        public static void AddVirtualFolder(string serverName, string websiteId, string name, string path)
        {
            ManagementScope scope = new ManagementScope(string.Format(@"\\{0}\root\MicrosoftIISV2", serverName));
            scope.Connect();

            string siteName = string.Format("W3SVC/{0}/Root/{1}", websiteId, name);

            ManagementClass mc = new ManagementClass(scope, new ManagementPath("IIsWebVirtualDirSetting"), null);
            ManagementObject oWebVirtDir = mc.CreateInstance();

            oWebVirtDir.Properties["Name"].Value = siteName;
            oWebVirtDir.Properties["Path"].Value = path;
            oWebVirtDir.Properties["AuthFlags"].Value = 5; // Integrated Windows Auth.
            oWebVirtDir.Properties["EnableDefaultDoc"].Value = true;
            // date, time, size, extension, longdate ;
            oWebVirtDir.Properties["DirBrowseFlags"].Value = 0x4000003E;
            oWebVirtDir.Properties["AccessFlags"].Value = 513; // read script 
            oWebVirtDir.Put();

            ManagementObject mo = new ManagementObject(scope,
              new System.Management.ManagementPath("IIsWebVirtualDir='" +
              siteName + "'"), null);
            ManagementBaseObject inputParameters = mo.GetMethodParameters("AppCreate2");
            inputParameters["AppMode"] = 2;
            mo.InvokeMethod("AppCreate2", inputParameters, null);
            mo = new ManagementObject(scope, new System.Management.ManagementPath(
                     "IIsWebVirtualDirSetting='" + siteName + "'"), null);
            mo.Properties["AppFriendlyName"].Value = name;
            mo.Put();
        }
    }
}
