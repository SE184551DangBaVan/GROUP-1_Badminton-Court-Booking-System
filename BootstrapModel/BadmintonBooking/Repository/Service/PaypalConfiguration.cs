using PayPal.Api;

namespace BadmintonBooking.Repository.Service
{
    public static class PaypalConfiguration
    {
        private static Dictionary<string, string> GetConfig(string mode)
        {
            //return PayPal.Api.ConfigManager.Instance.GetProperties();
            return new Dictionary<string, string>()
            {
                {"mode",mode }
            };
        }

        private static string GetAccessToken(string ClientId, string ClientSecret, string mode)
        {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, new Dictionary<string, string>()
            {
                {"mode",mode }
            }).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext(string ClientId, string ClientSecret, string mode)
        {
            APIContext apiContext = new APIContext(GetAccessToken(ClientId, ClientSecret, mode));
            apiContext.Config = GetConfig(mode);
            return apiContext;
        }
    }
}
