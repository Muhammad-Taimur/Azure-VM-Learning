using Microsoft.WindowsAzure.Management.WebSites;

namespace ConsoleApplication1
{
    class Program
    {
        private static string webSpaceName = "Pluralsight-WestEuropewebspace";
        private static string webSiteName = "pluralsightcolorwebsite";

        static void Main(string[] args)
        {
            //retrieve credentials for your subscription at 
            //https://manage.windowsazure.com/publishsettings/
            var credentials = CertificateAuthenticationHelper.GetCredentials();
            
            WebSiteManagementClient client = new WebSiteManagementClient(credentials);
                        
            client.WebSites.SwapSlots(webSpaceName, webSiteName, "production", "staging");
        }
    }
}
