namespace TabPlayStarter
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.TravellerGroup = new System.Windows.Forms.GroupBox();
            this.ShowPercentageCheckbox = new System.Windows.Forms.CheckBox();
            this.ShowHandRecordCheckbox = new System.Windows.Forms.CheckBox();
            this.ShowTravellerCheckbox = new System.Windows.Forms.CheckBox();
            this.PlayersGroup = new System.Windows.Forms.GroupBox();
            this.NameSourceCombobox = new System.Windows.Forms.ComboBox();
            this.NumberEntryEachRoundCheckbox = new System.Windows.Forms.CheckBox();
            this.RankingListGroup = new System.Windows.Forms.GroupBox();
            this.ShowRankingCombobox = new System.Windows.Forms.ComboBox();
            this.LeadCardGroup = new System.Windows.Forms.GroupBox();
            this.PollIntervalNud = new System.Windows.Forms.NumericUpDown();
            this.CanxButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.TravellerGroup.SuspendLayout();
            this.PlayersGroup.SuspendLayout();
            this.RankingListGroup.SuspendLayout();
            this.LeadCardGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PollIntervalNud)).BeginInit();
            this.SuspendLayout();
            // 
            // TravellerGroup
            // 
            this.TravellerGroup.Controls.Add(this.ShowPercentageCheckbox);
            this.TravellerGroup.Controls.Add(this.ShowHandRecordCheckbox);
            this.TravellerGroup.Controls.Add(this.ShowTravellerCheckbox);
            this.TravellerGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TravellerGroup.Location = new System.Drawing.Point(12, 12);
            this.TravellerGroup.Name = "TravellerGroup";
            this.TravellerGroup.Size = new System.Drawing.Size(220, 99);
            this.TravellerGroup.TabIndex = 0;
            this.TravellerGroup.TabStop = false;
            this.TravellerGroup.Text = "Traveller";
            // 
            // ShowPercentageCheckbox
            // 
            this.ShowPercentageCheckbox.AutoSize = true;
            this.ShowPercentageCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowPercentageCheckbox.Location = new System.Drawing.Point(6, 46);
            this.ShowPercentageCheckbox.Name = "ShowPercentageCheckbox";
            this.ShowPercentageCheckbox.Size = new System.Drawing.Size(190, 19);
            this.ShowPercentageCheckbox.TabIndex = 3;
            this.ShowPercentageCheckbox.TabStop = false;
            this.ShowPercentageCheckbox.Text = "Show Percentage on Traveller";
            this.ShowPercentageCheckbox.UseVisualStyleBackColor = true;
            // 
            // ShowHandRecordCheckbox
            // 
            this.ShowHandRecordCheckbox.AutoSize = true;
            this.ShowHandRecordCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowHandRecordCheckbox.Location = new System.Drawing.Point(6, 71);
            this.ShowHandRecordCheckbox.Name = "ShowHandRecordCheckbox";
            this.ShowHandRecordCheckbox.Size = new System.Drawing.Size(133, 19);
            this.ShowHandRecordCheckbox.TabIndex = 4;
            this.ShowHandRecordCheckbox.TabStop = false;
            this.ShowHandRecordCheckbox.Text = "Show Hand Record";
            this.ShowHandRecordCheckbox.UseVisualStyleBackColor = true;
            // 
            // ShowTravellerCheckbox
            // 
            this.ShowTravellerCheckbox.AutoSize = true;
            this.ShowTravellerCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowTravellerCheckbox.Location = new System.Drawing.Point(6, 21);
            this.ShowTravellerCheckbox.Name = "ShowTravellerCheckbox";
            this.ShowTravellerCheckbox.Size = new System.Drawing.Size(107, 19);
            this.ShowTravellerCheckbox.TabIndex = 2;
            this.ShowTravellerCheckbox.TabStop = false;
            this.ShowTravellerCheckbox.Text = "Show Traveller";
            this.ShowTravellerCheckbox.UseVisualStyleBackColor = true;
            this.ShowTravellerCheckbox.CheckedChanged += new System.EventHandler(this.ShowTraveller_CheckedChanged);
            // 
            // PlayersGroup
            // 
            this.PlayersGroup.Controls.Add(this.NameSourceCombobox);
            this.PlayersGroup.Controls.Add(this.NumberEntryEachRoundCheckbox);
            this.PlayersGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayersGroup.Location = new System.Drawing.Point(238, 12);
            this.PlayersGroup.Name = "PlayersGroup";
            this.PlayersGroup.Size = new System.Drawing.Size(251, 82);
            this.PlayersGroup.TabIndex = 0;
            this.PlayersGroup.TabStop = false;
            this.PlayersGroup.Text = "Player Names/Numbers";
            // 
            // NameSourceCombobox
            // 
            this.NameSourceCombobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameSourceCombobox.FormattingEnabled = true;
            this.NameSourceCombobox.Items.AddRange(new object[] {
            "Internal database",
            "External database",
            "No name source",
            "Internal database first, then external"});
            this.NameSourceCombobox.Location = new System.Drawing.Point(6, 20);
            this.NameSourceCombobox.Name = "NameSourceCombobox";
            this.NameSourceCombobox.Size = new System.Drawing.Size(239, 23);
            this.NameSourceCombobox.TabIndex = 8;
            this.NameSourceCombobox.TabStop = false;
            // 
            // NumberEntryEachRoundCheckbox
            // 
            this.NumberEntryEachRoundCheckbox.AutoSize = true;
            this.NumberEntryEachRoundCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumberEntryEachRoundCheckbox.Location = new System.Drawing.Point(6, 50);
            this.NumberEntryEachRoundCheckbox.Name = "NumberEntryEachRoundCheckbox";
            this.NumberEntryEachRoundCheckbox.Size = new System.Drawing.Size(209, 19);
            this.NumberEntryEachRoundCheckbox.TabIndex = 9;
            this.NumberEntryEachRoundCheckbox.TabStop = false;
            this.NumberEntryEachRoundCheckbox.Text = "Player Number Entry Each Round";
            this.NumberEntryEachRoundCheckbox.UseVisualStyleBackColor = true;
            // 
            // RankingListGroup
            // 
            this.RankingListGroup.Controls.Add(this.ShowRankingCombobox);
            this.RankingListGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RankingListGroup.Location = new System.Drawing.Point(12, 117);
            this.RankingListGroup.Name = "RankingListGroup";
            this.RankingListGroup.Size = new System.Drawing.Size(220, 56);
            this.RankingListGroup.TabIndex = 0;
            this.RankingListGroup.TabStop = false;
            this.RankingListGroup.Text = "Ranking List";
            // 
            // ShowRankingCombobox
            // 
            this.ShowRankingCombobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowRankingCombobox.FormattingEnabled = true;
            this.ShowRankingCombobox.Items.AddRange(new object[] {
            "Don\'t show",
            "Show after each round",
            "Show at end of session"});
            this.ShowRankingCombobox.Location = new System.Drawing.Point(7, 22);
            this.ShowRankingCombobox.Name = "ShowRankingCombobox";
            this.ShowRankingCombobox.Size = new System.Drawing.Size(207, 23);
            this.ShowRankingCombobox.TabIndex = 5;
            this.ShowRankingCombobox.TabStop = false;
            // 
            // LeadCardGroup
            // 
            this.LeadCardGroup.Controls.Add(this.PollIntervalNud);
            this.LeadCardGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeadCardGroup.Location = new System.Drawing.Point(238, 102);
            this.LeadCardGroup.Name = "LeadCardGroup";
            this.LeadCardGroup.Size = new System.Drawing.Size(251, 53);
            this.LeadCardGroup.TabIndex = 0;
            this.LeadCardGroup.TabStop = false;
            this.LeadCardGroup.Text = "Poll Interval (in milliseconds)";
            // 
            // PollIntervalNud
            // 
            this.PollIntervalNud.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PollIntervalNud.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.PollIntervalNud.Location = new System.Drawing.Point(7, 22);
            this.PollIntervalNud.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.PollIntervalNud.Name = "PollIntervalNud";
            this.PollIntervalNud.Size = new System.Drawing.Size(238, 22);
            this.PollIntervalNud.TabIndex = 0;
            this.PollIntervalNud.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // CanxButton
            // 
            this.CanxButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CanxButton.Location = new System.Drawing.Point(309, 166);
            this.CanxButton.Name = "CanxButton";
            this.CanxButton.Size = new System.Drawing.Size(75, 23);
            this.CanxButton.TabIndex = 10;
            this.CanxButton.TabStop = false;
            this.CanxButton.Text = "Cancel";
            this.CanxButton.UseVisualStyleBackColor = true;
            this.CanxButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKButton.Location = new System.Drawing.Point(408, 166);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 11;
            this.OKButton.TabStop = false;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 201);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CanxButton);
            this.Controls.Add(this.PlayersGroup);
            this.Controls.Add(this.RankingListGroup);
            this.Controls.Add(this.LeadCardGroup);
            this.Controls.Add(this.TravellerGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "TabPlay Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.TravellerGroup.ResumeLayout(false);
            this.TravellerGroup.PerformLayout();
            this.PlayersGroup.ResumeLayout(false);
            this.PlayersGroup.PerformLayout();
            this.RankingListGroup.ResumeLayout(false);
            this.LeadCardGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PollIntervalNud)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox TravellerGroup;
        private System.Windows.Forms.CheckBox ShowPercentageCheckbox;
        private System.Windows.Forms.CheckBox ShowHandRecordCheckbox;
        private System.Windows.Forms.CheckBox ShowTravellerCheckbox;
        private System.Windows.Forms.GroupBox PlayersGroup;
        private System.Windows.Forms.ComboBox NameSourceCombobox;
        private System.Windows.Forms.CheckBox NumberEntryEachRoundCheckbox;
        private System.Windows.Forms.GroupBox RankingListGroup;
        private System.Windows.Forms.ComboBox ShowRankingCombobox;
        private System.Windows.Forms.GroupBox LeadCardGroup;
        private System.Windows.Forms.Button CanxButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.NumericUpDown PollIntervalNud;
    }
}