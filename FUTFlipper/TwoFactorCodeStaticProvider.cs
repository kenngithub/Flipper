using System.Threading.Tasks;
using UltimateTeam.Toolkit.Services;

namespace FUTFlipper
{
    public class TwoFactorCodeStaticProvider : ITwoFactorCodeProvider
    {
        private string code;
        public TwoFactorCodeStaticProvider(string code)
        {
            this.code = code;
        }
        public Task<string> GetTwoFactorCodeAsync()
        {
            Task<string> task = Task.Factory.StartNew<string>(() =>
            {
                return code;
            });

            return task;
        }
    }
}
