using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using PayPal.Api;
using log4net;

namespace PayPalWebApplication.Models
{
    public static class Configuration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        // Logs output statements, errors, debug info to a text file    
        private static ILog logger = LogManager.GetLogger(typeof(Configuration));

        // Static constructor for setting the readonly static members.
        static Configuration()
        {
            // Load the log4net configuration settings from Web.config or App.config    
            log4net.Config.XmlConfigurator.Configure();

            var config = GetConfig();
            ClientId = config["clientId"];
            logger.Debug("ClientId:" + ClientId);
            ClientSecret = config["clientSecret"];
            logger.Debug("ClientSecret:" + ClientSecret);
        }

        // Create the configuration map that contains mode and other optional configuration details.
        public static Dictionary<string, string> GetConfig()
        {
            Dictionary < string, string> map = ConfigManager.Instance.GetProperties();

            var secrets = ConfigurationManager.GetSection("paypalSecrets") as NameValueCollection;
            if(secrets == null)
            {
                logger.Error("Paypal authentication data not supplied");
                throw new System.Exception("Paypal authentication data not supplied");
            }
            string clientId = secrets["clientId"];
            map.Add("clientId", clientId);

            string clientSecret = secrets["clientSecret"];
            map.Add("clientSecret", clientSecret);

            return map;
        }

        // Create accessToken
        private static string GetAccessToken()
        {
            // ###AccessToken
            // Retrieve the access token from
            // OAuthTokenCredential by passing in
            // ClientID and ClientSecret
            // It is not mandatory to generate Access Token on a per call basis.
            // Typically the access token can be generated once and
            // reused within the expiry window                
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
            logger.Debug("accessToken:" + accessToken);
            return accessToken;
        }

        // Returns APIContext object
        public static APIContext GetAPIContext()
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();

            // Use this variant if you want to pass in a request id  
            // that is meaningful in your application, ideally 
            // a order id.
            // String requestId = Long.toString(System.nanoTime();
            // APIContext apiContext = new APIContext(GetAccessToken(), requestId ));

            return apiContext;
        }

    }
}
