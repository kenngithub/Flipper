namespace FUTWinForm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loginButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.watchListButton = new System.Windows.Forms.Button();
            this.tradePileButton = new System.Windows.Forms.Button();
            this.buyButton = new System.Windows.Forms.Button();
            this.SearchPage = new System.Windows.Forms.NumericUpDown();
            this.loginBox = new System.Windows.Forms.ComboBox();
            this.slowDay = new System.Windows.Forms.CheckBox();
            this.searchAndBuy = new System.Windows.Forms.Button();
            this.buyAmount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.unassignedPile = new System.Windows.Forms.Button();
            this.buyType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.SearchPage)).BeginInit();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(13, 13);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(13, 43);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // watchListButton
            // 
            this.watchListButton.Location = new System.Drawing.Point(168, 43);
            this.watchListButton.Name = "watchListButton";
            this.watchListButton.Size = new System.Drawing.Size(75, 23);
            this.watchListButton.TabIndex = 3;
            this.watchListButton.Text = "WatchList";
            this.watchListButton.UseVisualStyleBackColor = true;
            this.watchListButton.Click += new System.EventHandler(this.watchListButton_Click);
            // 
            // tradePileButton
            // 
            this.tradePileButton.Location = new System.Drawing.Point(249, 43);
            this.tradePileButton.Name = "tradePileButton";
            this.tradePileButton.Size = new System.Drawing.Size(75, 23);
            this.tradePileButton.TabIndex = 4;
            this.tradePileButton.Text = "TradePile";
            this.tradePileButton.UseVisualStyleBackColor = true;
            this.tradePileButton.Click += new System.EventHandler(this.tradePileButton_Click);
            // 
            // buyButton
            // 
            this.buyButton.Location = new System.Drawing.Point(13, 73);
            this.buyButton.Name = "buyButton";
            this.buyButton.Size = new System.Drawing.Size(75, 23);
            this.buyButton.TabIndex = 5;
            this.buyButton.Text = "BuyBuyBuy";
            this.buyButton.UseVisualStyleBackColor = true;
            this.buyButton.Click += new System.EventHandler(this.buyButton_Click);
            // 
            // SearchPage
            // 
            this.SearchPage.Location = new System.Drawing.Point(94, 47);
            this.SearchPage.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.SearchPage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SearchPage.Name = "SearchPage";
            this.SearchPage.Size = new System.Drawing.Size(68, 20);
            this.SearchPage.TabIndex = 6;
            this.SearchPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // loginBox
            // 
            this.loginBox.FormattingEnabled = true;
            this.loginBox.Items.AddRange(new object[] {
            "Ken",
            "Steve",
            "Ken1",
            "Ken2",
            "Ken3",
            "Ken4",
            "Ken5",
            "Ken6"});
            this.loginBox.Location = new System.Drawing.Point(94, 13);
            this.loginBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.loginBox.Name = "loginBox";
            this.loginBox.Size = new System.Drawing.Size(92, 21);
            this.loginBox.TabIndex = 7;
            this.loginBox.Text = "Ken";
            this.loginBox.SelectedIndexChanged += new System.EventHandler(this.loginBox_SelectedIndexChanged);
            // 
            // slowDay
            // 
            this.slowDay.AutoSize = true;
            this.slowDay.Location = new System.Drawing.Point(94, 78);
            this.slowDay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.slowDay.Name = "slowDay";
            this.slowDay.Size = new System.Drawing.Size(71, 17);
            this.slowDay.TabIndex = 8;
            this.slowDay.Text = "Slow Day";
            this.slowDay.UseVisualStyleBackColor = true;
            this.slowDay.CheckedChanged += new System.EventHandler(this.slowDay_CheckedChanged);
            // 
            // searchAndBuy
            // 
            this.searchAndBuy.Location = new System.Drawing.Point(329, 95);
            this.searchAndBuy.Name = "searchAndBuy";
            this.searchAndBuy.Size = new System.Drawing.Size(119, 23);
            this.searchAndBuy.TabIndex = 9;
            this.searchAndBuy.Text = "Search and Buy";
            this.searchAndBuy.UseVisualStyleBackColor = true;
            this.searchAndBuy.Click += new System.EventHandler(this.searchAndBuy_Click);
            // 
            // buyAmount
            // 
            this.buyAmount.Location = new System.Drawing.Point(99, 98);
            this.buyAmount.Name = "buyAmount";
            this.buyAmount.Size = new System.Drawing.Size(100, 20);
            this.buyAmount.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Buy Amount";
            // 
            // unassignedPile
            // 
            this.unassignedPile.Location = new System.Drawing.Point(329, 43);
            this.unassignedPile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.unassignedPile.Name = "unassignedPile";
            this.unassignedPile.Size = new System.Drawing.Size(74, 23);
            this.unassignedPile.TabIndex = 12;
            this.unassignedPile.Text = "Unassigned";
            this.unassignedPile.UseVisualStyleBackColor = true;
            this.unassignedPile.Click += new System.EventHandler(this.unassignedPile_Click);
            // 
            // buyType
            // 
            this.buyType.FormattingEnabled = true;
            this.buyType.Items.AddRange(new object[] {
            "Staff",
            "ClubInfo"});
            this.buyType.Location = new System.Drawing.Point(205, 98);
            this.buyType.Name = "buyType";
            this.buyType.Size = new System.Drawing.Size(118, 21);
            this.buyType.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 468);
            this.Controls.Add(this.buyType);
            this.Controls.Add(this.unassignedPile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buyAmount);
            this.Controls.Add(this.searchAndBuy);
            this.Controls.Add(this.slowDay);
            this.Controls.Add(this.loginBox);
            this.Controls.Add(this.SearchPage);
            this.Controls.Add(this.buyButton);
            this.Controls.Add(this.tradePileButton);
            this.Controls.Add(this.watchListButton);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.loginButton);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.SearchPage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button watchListButton;
        private System.Windows.Forms.Button tradePileButton;
        private System.Windows.Forms.Button buyButton;
        private System.Windows.Forms.NumericUpDown SearchPage;
        private System.Windows.Forms.ComboBox loginBox;
        private System.Windows.Forms.CheckBox slowDay;
        private System.Windows.Forms.Button searchAndBuy;
        private System.Windows.Forms.TextBox buyAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button unassignedPile;
        private System.Windows.Forms.ComboBox buyType;
    }
}

