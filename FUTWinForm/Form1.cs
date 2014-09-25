using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FUTFlipper;


namespace FUTWinForm
{
    public partial class Form1 : Form
    {

        private Flipper Flipper { get; set; }
        public Form1()
        {
            InitializeComponent();
            Flipper = new Flipper();

            loginBox.Items.Clear();
            loginBox.Items.AddRange(Flipper.GetAccountNames().ToArray());

            if (loginBox.Items.Count > 0)
            {
                loginBox.SelectedIndex = 0;
                Flipper.ChangeAccount(loginBox.Items[0].ToString());
            }
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            await Flipper.Login();
        }

        private async void searchButton_Click(object sender, EventArgs e)
        {
            await Flipper.PlayerForQuickSellSearch((uint) SearchPage.Value);
        }

        private async void watchListButton_Click(object sender, EventArgs e)
        {
            //await Flipper.Login();
            await Flipper.WatchList();
        }

        private async void tradePileButton_Click(object sender, EventArgs e)
        {
            await Flipper.TradePile();
        }

        private async void buyButton_Click(object sender, EventArgs e)
        {
            await Flipper.BuyBuyBuy((uint)SearchPage.Value);
        }

        private void loginBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string account = loginBox.SelectedItem.ToString();
            Flipper.ChangeAccount(account);
        }

        private void slowDay_CheckedChanged(object sender, EventArgs e)
        {
            Flipper.SlowDay = slowDay.Checked;
        }

        private async void searchAndBuy_Click(object sender, EventArgs e)
        {
            await Flipper.Login();
            Flipper.SearchToTransferMoney(Int32.Parse(buyAmount.Text));
        }

        private async void unassignedPile_Click(object sender, EventArgs e)
        {
            await Flipper.Login();
            await Flipper.UnnassignedPile();
        }
    }
}
