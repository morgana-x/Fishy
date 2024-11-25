using System.Security.Cryptography;
using System.Text;
using GenHTTP.Api.Content.Authentication;
using GenHTTP.Modules.Authentication.Basic;

namespace Fishy.Webserver
{
    class Authentication
    {
        public static ValueTask<IUser?> Auth(string user, string password)
        {
            string hashedPw;
            byte[] data = SHA512.HashData(Encoding.UTF8.GetBytes(password));
            hashedPw = GetStringFromHash(data).ToLower();

            if (user == WebserverExtension.GetUsername() && 
                hashedPw == WebserverExtension.GetPassword().ToLower())
                return new(new BasicAuthenticationUser(user));
            else
                return new();
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new ();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
