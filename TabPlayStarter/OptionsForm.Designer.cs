namespace TabScoreStarter
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
            this.ValidateLeadCardCheckbox = new System.Windows.Forms.CheckBox();
            this.EnterLeadCardCheckbox = new System.Windows.Forms.CheckBox();
            this.CanxButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.EnterResultsMethodGroup = new System.Windows.Forms.GroupBox();
            this.EnterResultsMethodCombobox = new System.Windows.Forms.ComboBox();
            this.TabletMovesGroupBox = new System.Windows.Forms.GroupBox();
            this.TabletMovesCheckbox = new System.Windows.Forms.CheckBox();
            this.TravellerGroup.SuspendLayout();
            this.PlayersGroup.SuspendLayout();
            this.RankingListGroup.SuspendLayout();
            this.LeadCardGroup.SuspendLayout();
            this.EnterResultsMethodGroup.SuspendLayout();
            this.TabletMovesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // TravellerGroup
            // 
            this.TravellerGroup.Controls.Add(this.ShowPercentageCheckbox);
            this.TravellerGroup.Controls.Add(this.ShowHandRecordCheckbox);
            this.TravellerGroup.Controls.Add(this.ShowTravellerCheckbox);
            this.TravellerGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TravellerGroup.Location = new System.Drawing.Point(12, 74);
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
            this.PlayersGroup.Location = new System.Drawing.Point(238, 91);
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
            this.RankingListGroup.Location = new System.Drawing.Point(12, 179);
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
            this.LeadCardGroup.Controls.Add(this.ValidateLeadCardCheckbox);
            this.LeadCardGroup.Controls.Add(this.EnterLeadCardCheckbox);
            this.LeadCardGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LeadCardGroup.Location = new System.Drawing.Point(238, 12);
            this.LeadCardGroup.Name = "LeadCardGroup";
            this.LeadCardGroup.Size = new System.Drawing.Size(251, 73);
            this.LeadCardGroup.TabIndex = 0;
            this.LeadCardGroup.TabStop = false;
            this.LeadCardGroup.Text = "Lead Card";
            // 
            // ValidateLeadCardCheckbox
            // 
            this.ValidateLeadCardCheckbox.AutoSize = true;
            this.ValidateLeadCardCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValidateLeadCardCheckbox.Location = new System.Drawing.Point(6, 46);
            this.ValidateLeadCardCheckbox.Name = "ValidateLeadCardCheckbox";
            this.ValidateLeadCardCheckbox.Size = new System.Drawing.Size(130, 19);
            this.ValidateLeadCardCheckbox.TabIndex = 7;
            this.ValidateLeadCardCheckbox.TabStop = false;
            this.ValidateLeadCardCheckbox.Text = "Validate Lead Card";
            this.ValidateLeadCardCheckbox.UseVisualStyleBackColor = true;
            // 
            // EnterLeadCardCheckbox
            // 
            this.EnterLeadCardCheckbox.AutoSize = true;
            this.EnterLeadCardCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterLeadCardCheckbox.Location = new System.Drawing.Point(6, 21);
            this.EnterLeadCardCheckbox.Name = "EnterLeadCardCheckbox";
            this.EnterLeadCardCheckbox.Size = new System.Drawing.Size(115, 19);
            this.EnterLeadCardCheckbox.TabIndex = 6;
            this.EnterLeadCardCheckbox.TabStop = false;
            this.EnterLeadCardCheckbox.Text = "Enter Lead Card";
            this.EnterLeadCardCheckbox.UseVisualStyleBackColor = true;
            this.EnterLeadCardCheckbox.CheckedChanged += new System.EventHandler(this.EnterLeadCard_CheckedChanged);
            // 
            // CanxButton
            // 
            this.CanxButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CanxButton.Location = new System.Drawing.Point(284, 253);
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
            this.OKButton.Location = new System.Drawing.Point(408, 253);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 11;
            this.OKButton.TabStop = false;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // EnterResultsMethodGroup
            // 
            this.EnterResultsMethodGroup.Controls.Add(this.EnterResultsMethodCombobox);
            this.EnterResultsMethodGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterResultsMethodGroup.Location = new System.Drawing.Point(12, 12);
            this.EnterResultsMethodGroup.Name = "EnterResultsMethodGroup";
            this.EnterResultsMethodGroup.Size = new System.Drawing.Size(220, 56);
            this.EnterResultsMethodGroup.TabIndex = 5;
            this.EnterResultsMethodGroup.TabStop = false;
            this.EnterResultsMethodGroup.Text = "Enter Results Method";
            // 
            // EnterResultsMethodCombobox
            // 
            this.EnterResultsMethodCombobox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnterResultsMethodCombobox.FormattingEnabled = true;
            this.EnterResultsMethodCombobox.Items.AddRange(new object[] {
            "Tricks plus or minus",
            "Total tricks won"});
            this.EnterResultsMethodCombobox.Location = new System.Drawing.Point(7, 22);
            this.EnterResultsMethodCombobox.Name = "EnterResultsMethodCombobox";
            this.EnterResultsMethodCombobox.Size = new System.Drawing.Size(207, 23);
            this.EnterResultsMethodCombobox.TabIndex = 1;
            this.EnterResultsMethodCombobox.TabStop = false;
            // 
            // TabletMovesGroupBox
            // 
            this.TabletMovesGroupBox.Controls.Add(this.TabletMovesCheckbox);
            this.TabletMovesGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabletMovesGroupBox.Location = new System.Drawing.Point(238, 179);
            this.TabletMovesGroupBox.Name = "TabletMovesGroupBox";
            this.TabletMovesGroupBox.Size = new System.Drawing.Size(251, 56);
            this.TabletMovesGroupBox.TabIndex = 8;
            this.TabletMovesGroupBox.TabStop = false;
            this.TabletMovesGroupBox.Text = "Moving Tablets";
            // 
            // TabletMovesCheckbox
            // 
            this.TabletMovesCheckbox.AutoSize = true;
            this.TabletMovesCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabletMovesCheckbox.Location = new System.Drawing.Point(6, 21);
            this.TabletMovesCheckbox.Name = "TabletMovesCheckbox";
            this.TabletMovesCheckbox.Size = new System.Drawing.Size(223, 19);
            this.TabletMovesCheckbox.TabIndex = 6;
            this.TabletMovesCheckbox.TabStop = false;
            this.TabletMovesCheckbox.Text = "Tablet Moves With North/East Player";
            this.TabletMovesCheckbox.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 293);
            this.Controls.Add(this.TabletMovesGroupBox);
            this.Controls.Add(this.EnterResultsMethodGroup);
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
            this.Text = "TabScore Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.TravellerGroup.ResumeLayout(false);
            this.TravellerGroup.PerformLayout();
            this.PlayersGroup.ResumeLayout(false);
            this.PlayersGroup.PerformLayout();
            this.RankingListGroup.ResumeLayout(false);
            this.LeadCardGroup.ResumeLayout(false);
            this.LeadCardGroup.PerformLayout();
            this.EnterResultsMethodGroup.ResumeLayout(false);
            this.TabletMovesGroupBox.ResumeLayout(false);
            this.TabletMovesGroupBox.PerformLayout();
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
        private System.Windows.Forms.CheckBox ValidateLeadCardCheckbox;
        private System.Windows.Forms.CheckBox EnterLeadCardCheckbox;
        private System.Windows.Forms.Button CanxButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.GroupBox EnterResultsMethodGroup;
        private System.Windows.Forms.ComboBox EnterResultsMethodCombobox;
        private System.Windows.Forms.GroupBox TabletMovesGroupBox;
        private System.Windows.Forms.CheckBox TabletMovesCheckbox;
    }
}