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

        private Dictionary<string, Flipper> Flippers { get; set; }
        private string currentAccount;
        public Form1()
        {
            InitializeComponent();
            Flipper startingFlipper = new Flipper();

            loginBox.Items.Clear();
            loginBox.Items.AddRange(startingFlipper.GetAccountNames().ToArray());

            Flippers = startingFlipper.GetAccountNames().ToDictionary(a => a, a =>
                {
                    Flipper f = new Flipper();
                    f.ChangeAccount(a);
                    return f;
                });

            if (loginBox.Items.Count > 0)
            {
                loginBox.SelectedIndex = 0;
                currentAccount = loginBox.Items[0].ToString();
            }
            buyType.SelectedIndex = 0;
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            await Flippers[currentAccount].Login();
        }

        private async void searchButton_Click(object sender, EventArgs e)
        {
            await Flippers[currentAccount].PlayerForQuickSellSearch((uint)SearchPage.Value);
        }

        private async void watchListButton_Click(object sender, EventArgs e)
        {
            if (!Flippers[currentAccount].LoggedIn) await Flippers[currentAccount].Login();
            await Flippers[currentAccount].WatchList();
        }

        private async void tradePileButton_Click(object sender, EventArgs e)
        {
            await Flippers[currentAccount].TradePile();
        }

        private async void buyButton_Click(object sender, EventArgs e)
        {
            await Flippers[currentAccount].BuyBuyBuy((uint)SearchPage.Value);
        }

        private void loginBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentAccount = loginBox.SelectedItem.ToString();
            Flippers[currentAccount].ChangeAccount(currentAccount);
        }

        private void slowDay_CheckedChanged(object sender, EventArgs e)
        {
            Flippers[currentAccount].SlowDay = slowDay.Checked;
        }

        private async void searchAndBuy_Click(object sender, EventArgs e)
        {
            if (!Flippers[currentAccount].LoggedIn) await Flippers[currentAccount].Login();
            if (!Flippers[currentAccount].LoggedIn) return;
            Flippers[currentAccount].SearchToTransferMoney(Int32.Parse(buyAmount.Text), buyType.SelectedItem.ToString());
        }

        private async void unassignedPile_Click(object sender, EventArgs e)
        {
            await Flippers[currentAccount].Login();
            await Flippers[currentAccount].UnnassignedPile();
        }

        private async void getCodeButton_Click(object sender, EventArgs e)
        {
            string code = await Flippers[currentAccount].GetTwoFactorCode();
            codeTextBox.Text = code ?? "Can't find code";
        }
    }
}
