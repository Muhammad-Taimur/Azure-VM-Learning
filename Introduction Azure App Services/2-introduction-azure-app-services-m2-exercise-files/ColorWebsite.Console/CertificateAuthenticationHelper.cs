using Microsoft.Azure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public static class CertificateAuthenticationHelper
    {
        internal static SubscriptionCloudCredentials GetCredentials()
        {
            string certKey = ConfigurationManager.AppSettings["cert"];
            string subscriptionId = ConfigurationManager.AppSettings["subscriptionId"];

            return new CertificateCloudCredentials(subscriptionId, new System.Security.Cryptography.X509Certificates.X509Certificate2(Convert.FromBase64String(certKey)));
        }
    }
}
