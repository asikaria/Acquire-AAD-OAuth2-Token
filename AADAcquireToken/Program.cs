using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AADAcquireToken
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(getClientCredsToken());  // get a client creds token
            Console.WriteLine(getDeviceCodeToken());   // get a device code token
            Console.ReadLine();
        }

        static String getClientCredsToken()
        {
            string authPrefix = "https://login.microsoftonline.com/";
            string domain = "AAD-TENANT-GUID-HERE";         // aad tenant guid, from OAUTH2 URL
            string clientId = "APPID-GUID-HERE";
            string clientSecret = "APPID-SECRET-HERE";
            string resource = "https://management.core.windows.net/";  // use any other resource as appropriate
            string authUrl = authPrefix + domain;

            var aadContext = new AuthenticationContext(authUrl);
            var creds = new ClientCredential(clientId, clientSecret);
            AuthenticationResult token = aadContext.AcquireTokenAsync(resource, creds)
                .GetAwaiter()
                .GetResult();
            return token.AccessToken;
        }

        static string getDeviceCodeToken()
        {
            string clientId = "CLIENT-ID-HERE";
            string resource = "https://management.core.windows.net/";
            string authUrl = "https://login.microsoftonline.com/common";  // use any other resource as appropriate

            var aadContext = new AuthenticationContext(authUrl);
            DeviceCodeResult dcResult = aadContext.AcquireDeviceCodeAsync(resource, clientId).GetAwaiter().GetResult();
            Console.WriteLine("UserCode:  " + dcResult.UserCode);
            Console.WriteLine("VerificationUrl: " + dcResult.VerificationUrl);
            Console.WriteLine("Recommended Polling Interval: " + dcResult.Interval);
            Console.WriteLine("Expires: " + dcResult.ExpiresOn.DateTime.ToLocalTime());
            Console.WriteLine("Message:  " + dcResult.Message);
            Console.WriteLine("Devicecode: " + dcResult.DeviceCode);

            AuthenticationResult token = aadContext.AcquireTokenByDeviceCodeAsync(dcResult).GetAwaiter().GetResult();
            return token.AccessToken;
        }
    }
}
