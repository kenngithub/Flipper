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
            ((System.ComponentModel.ISupportInitialize)(this.SearchPage)).BeginInit();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(17, 16);
            this.loginButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(100, 28);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(17, 53);
            this.searchButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(100, 28);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // watchListButton
            // 
            this.watchListButton.Location = new System.Drawing.Point(224, 53);
            this.watchListButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.watchListButton.Name = "watchListButton";
            this.watchListButton.Size = new System.Drawing.Size(100, 28);
            this.watchListButton.TabIndex = 3;
            this.watchListButton.Text = "WatchList";
            this.watchListButton.UseVisualStyleBackColor = true;
            this.watchListButton.Click += new System.EventHandler(this.watchListButton_Click);
            // 
            // tradePileButton
            // 
            this.tradePileButton.Location = new System.Drawing.Point(332, 53);
            this.tradePileButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tradePileButton.Name = "tradePileButton";
            this.tradePileButton.Size = new System.Drawing.Size(100, 28);
            this.tradePileButton.TabIndex = 4;
            this.tradePileButton.Text = "TradePile";
            this.tradePileButton.UseVisualStyleBackColor = true;
            this.tradePileButton.Click += new System.EventHandler(this.tradePileButton_Click);
            // 
            // buyButton
            // 
            this.buyButton.Location = new System.Drawing.Point(17, 90);
            this.buyButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buyButton.Name = "buyButton";
            this.buyButton.Size = new System.Drawing.Size(100, 28);
            this.buyButton.TabIndex = 5;
            this.buyButton.Text = "BuyBuyBuy";
            this.buyButton.UseVisualStyleBackColor = true;
            this.buyButton.Click += new System.EventHandler(this.buyButton_Click);
            // 
            // SearchPage
            // 
            this.SearchPage.Location = new System.Drawing.Point(125, 58);
            this.SearchPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.SearchPage.Size = new System.Drawing.Size(91, 22);
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
            this.loginBox.Location = new System.Drawing.Point(125, 16);
            this.loginBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loginBox.Name = "loginBox";
            this.loginBox.Size = new System.Drawing.Size(121, 24);
            this.loginBox.TabIndex = 7;
            this.loginBox.Text = "Ken";
            this.loginBox.SelectedIndexChanged += new System.EventHandler(this.loginBox_SelectedIndexChanged);
            // 
            // slowDay
            // 
            this.slowDay.AutoSize = true;
            this.slowDay.Location = new System.Drawing.Point(125, 96);
            this.slowDay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.slowDay.Name = "slowDay";
            this.slowDay.Size = new System.Drawing.Size(88, 21);
            this.slowDay.TabIndex = 8;
            this.slowDay.Text = "Slow Day";
            this.slowDay.UseVisualStyleBackColor = true;
            this.slowDay.CheckedChanged += new System.EventHandler(this.slowDay_CheckedChanged);
            // 
            // searchAndBuy
            // 
            this.searchAndBuy.Location = new System.Drawing.Point(273, 121);
            this.searchAndBuy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.searchAndBuy.Name = "searchAndBuy";
            this.searchAndBuy.Size = new System.Drawing.Size(159, 28);
            this.searchAndBuy.TabIndex = 9;
            this.searchAndBuy.Text = "Search and Buy";
            this.searchAndBuy.UseVisualStyleBackColor = true;
            this.searchAndBuy.Click += new System.EventHandler(this.searchAndBuy_Click);
            // 
            // buyAmount
            // 
            this.buyAmount.Location = new System.Drawing.Point(132, 121);
            this.buyAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buyAmount.Name = "buyAmount";
            this.buyAmount.Size = new System.Drawing.Size(132, 22);
            this.buyAmount.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 124);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "Buy Amount";
            // 
            // unassignedPile
            // 
            this.unassignedPile.Location = new System.Drawing.Point(439, 53);
            this.unassignedPile.Name = "unassignedPile";
            this.unassignedPile.Size = new System.Drawing.Size(99, 28);
            this.unassignedPile.TabIndex = 12;
            this.unassignedPile.Text = "Unassigned";
            this.unassignedPile.UseVisualStyleBackColor = true;
            this.unassignedPile.Click += new System.EventHandler(this.unassignedPile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 576);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
    }
}

