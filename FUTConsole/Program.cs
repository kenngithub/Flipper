using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUTFlipper;

namespace FUTConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: AccountName");
            }

            try
            {
                Task task = Do(args[0]);
                task.Wait();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
                Console.Out.WriteLine(e.StackTrace);
                Console.In.ReadLine();
            }
        }

        private async static Task Do(string account)
        {

            Flipper flipper = new Flipper();
            flipper.ChangeAccount(account);

            await flipper.BuyBuyBuy(1);
        }
    }
}
