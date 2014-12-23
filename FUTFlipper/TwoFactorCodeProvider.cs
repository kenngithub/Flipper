using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForAspNet.POP3;
using UltimateTeam.Toolkit;

namespace FUTFlipper
{
    public class TwoFactorCodeProvider : ITwoFactorCodeProvider
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public TwoFactorCodeProvider(string username, string password)
        {
            License.LicenseKey = "LUxpY2Vuc2VkIHRvIEJlbiBSaWNoYXJkc29uIChXZWIgU2l0ZSBMaWNlbnNlKQEAAAABAAAA/z839HUoyiswag6nZ6GyCV/pb9TB3wpmdjwuKTbT8z1zTZ984JsYPJignadK6IsraqqiH14eeMg=";
            Username = username;
            Password = password;
        }

        public Task<string> GetTwoFactorCodeAsync()
        {
            Task<string> task = Task.Factory.StartNew<string>(() =>
            {
                var pop = new POP3("pop3.live.com", 995, Username, Password);
                pop.UseSSL = true;
                if (!pop.Connect()) { return pop.LastError; }

                //Check if server supports secure authentication.
                if (pop.IsAPOPSupported)
                {
                    if (!pop.SecureLogin()) return "Can't login";
                }
                else
                {
                    if (!pop.Login()) return "Can't login";
                }
                pop.QueryServer();
                for (var i = pop.TotalMailCount; i > 0; i--)
                {
                    var msg = pop.GetMessage(i, false);

                }
                return "bla";
            });

            return task;
        }
    }
}
