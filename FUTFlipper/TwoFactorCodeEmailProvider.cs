using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ForAspNet.POP3;
using log4net;
using UltimateTeam.Toolkit;
using FlipperUtil;
using UltimateTeam.Toolkit.Services;

namespace FUTFlipper
{
    public class TwoFactorCodeEmailProvider : ITwoFactorCodeProvider
    {
        public string Username { get; set; }
        public string Password { get; set; }
        private Regex codeRegex = new Regex(@"\<strong\>Your Origin Security Code: \</strong\>.*\<strong\>(?<code>\d{6})\</strong\>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        private ILog Log;


        public TwoFactorCodeEmailProvider(string username, string password, ILog log)
        {
            License.LicenseKey = "LUxpY2Vuc2VkIHRvIEJlbiBSaWNoYXJkc29uIChXZWIgU2l0ZSBMaWNlbnNlKQEAAAABAAAA/z839HUoyiswag6nZ6GyCV/pb9TB3wpmdjwuKTbT8z1zTZ984JsYPJignadK6IsraqqiH14eeMg=";
            Username = username;
            Password = password;
            Log = log;
        }

        public Task<string> GetTwoFactorCodeAsync()
        {
            Task<string> task = Task.Factory.StartNew<string>(() =>
            {
                string code = null;
                int attempts = 0;
                while (code == null && attempts < 20)
                {

                    using (var pop = new POP3("pop3.live.com", 995, Username, Password))
                    {
                        pop.UseSSL = true;
                        if (!pop.Connect())
                        {
                            Log.Error("Couldn't connect to email account to get two factor code: {0}".Args(pop.LastError));
                            return null;
                        }

                        if (pop.IsAPOPSupported)
                        {
                            if (!pop.SecureLogin())
                            {
                                Log.Error("Couldn't login to email account to get two factor code: {0}".Args(pop.LastError));
                                return null;
                            }
                        }
                        else
                        {
                            if (!pop.Login())
                            {
                                Log.Error("Couldn't login to email account to get two factor code: {0}".Args(pop.LastError));
                                return null;
                            }

                        }

                        attempts++;

                        pop.QueryServer();

                        Log.Info("Attempting to get two factor code: {0}, inbox count: {1}".Args(attempts, pop.TotalMailCount));
                        for (var i = pop.TotalMailCount; i > 0; i--)
                        {
                            var msg = pop.GetMessage(i, false);
                            if (msg.Subject == "Your Origin Security Code")
                            {
                                if (code == null)
                                {
                                    var body = msg.ToString();
                                    var match = codeRegex.Match(body);
                                    if (match.Success)
                                    {
                                        code = match.Groups["code"].Value;
                                        Log.Info("Found code: {0}".Args(code));
                                    }
                                }

                                pop.DeleteMessage(i);
                                Log.Info("Delete Message: {0}".Args(i));
                            }
                        }

                        pop.Close();
                    }

                    if (code == null)
                    {
                        System.Threading.Thread.Sleep(5000);
                    }
                }
                return code;
            });

            return task;
        }
    }
}
