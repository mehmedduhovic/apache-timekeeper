using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TimeKeeper.API.Helpers
{
    public static class TokenUtility
    {
        public static Dictionary<string, string> GenToken(string id_token)
        {
            string[] jwt = id_token.Split('.');
            string header = Encoding.UTF8.GetString(Convert.FromBase64String(FitToB64(jwt[0])));
            string payload = Encoding.UTF8.GetString(Convert.FromBase64String(FitToB64(jwt[1])));
            string signature = Encoding.UTF8.GetString(Convert.FromBase64String(FitToB64(jwt[2])));
            payload = payload.Replace('[', ' ').Replace(']', ' ');
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);
        }

        static string FitToB64(string X)
        {
            while (X.Length % 4 != 0) X += "=";
            return X.Replace('-', '+').Replace('_', '/');
        }
    }
}