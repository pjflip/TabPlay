namespace TabPlayStarter
{
    partial class TabPlayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TabPlayForm));
            this.ScoringDatabaseLabel = new System.Windows.Forms.Label();
            this.PathToDBLabel = new System.Windows.Forms.Label();
            this.AddHandRecordFileButton = new System.Windows.Forms.Button();
            this.HandRecordFileLabel = new System.Windows.Forms.Label();
            this.PathToHandRecordFileLabel = new System.Windows.Forms.Label();
            this.AddDatabaseFileButton = new System.Windows.Forms.Button();
            this.DatabaseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SessionStatusLabel = new System.Windows.Forms.Label();
            this.AnalysingLabel = new System.Windows.Forms.Label();
            this.AnalysingProgressBar = new System.Windows.Forms.ProgressBar();
            this.HandRecordFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.AnalysisCalculationBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.OptionsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ScoringDatabaseLabel
            // 
            this.ScoringDatabaseLabel.AutoSize = true;
            this.ScoringDatabaseLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScoringDatabaseLabel.Location = new System.Drawing.Point(15, 17);
            this.ScoringDatabaseLabel.Name = "ScoringDatabaseLabel";
            this.ScoringDatabaseLabel.Size = new System.Drawing.Size(137, 16);
            this.ScoringDatabaseLabel.TabIndex = 0;
            this.ScoringDatabaseLabel.Text = "Scoring Database:";
            // 
            // PathToDBLabel
            // 
            this.PathToDBLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PathToDBLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PathToDBLabel.Location = new System.Drawing.Point(16, 39);
            this.PathToDBLabel.Name = "PathToDBLabel";
            this.PathToDBLabel.Size = new System.Drawing.Size(477, 21);
            this.PathToDBLabel.TabIndex = 0;
            this.PathToDBLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddHandRecordFileButton
            // 
            this.AddHandRecordFileButton.Location = new System.Drawing.Point(302, 90);
            this.AddHandRecordFileButton.Name = "AddHandRecordFileButton";
            this.AddHandRecordFileButton.Size = new System.Drawing.Size(191, 23);
            this.AddHandRecordFileButton.TabIndex = 2;
            this.AddHandRecordFileButton.TabStop = false;
            this.AddHandRecordFileButton.Text = "Add hand record file...";
            this.AddHandRecordFileButton.UseVisualStyleBackColor = true;
            this.AddHandRecordFileButton.Visible = false;
            this.AddHandRecordFileButton.Click += new System.EventHandler(this.AddHandRecordFileButton_Click);
            // 
            // HandRecordFileLabel
            // 
            this.HandRecordFileLabel.AutoSize = true;
            this.HandRecordFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HandRecordFileLabel.Location = new System.Drawing.Point(15, 93);
            this.HandRecordFileLabel.Name = "HandRecordFileLabel";
            this.HandRecordFileLabel.Size = new System.Drawing.Size(134, 16);
            this.HandRecordFileLabel.TabIndex = 0;
            this.HandRecordFileLabel.Text = "Hand Record File:";
            // 
            // PathToHandRecordFileLabel
            // 
            this.PathToHandRecordFileLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PathToHandRecordFileLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PathToHandRecordFileLabel.Location = new System.Drawing.Point(18, 114);
            this.PathToHandRecordFileLabel.Name = "PathToHandRecordFileLabel";
            this.PathToHandRecordFileLabel.Size = new System.Drawing.Size(475, 21);
            this.PathToHandRecordFileLabel.TabIndex = 0;
            this.PathToHandRecordFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AddDatabaseFileButton
            // 
            this.AddDatabaseFileButton.Location = new System.Drawing.Point(302, 14);
            this.AddDatabaseFileButton.Name = "AddDatabaseFileButton";
            this.AddDatabaseFileButton.Size = new System.Drawing.Size(191, 23);
            this.AddDatabaseFileButton.TabIndex = 1;
            this.AddDatabaseFileButton.TabStop = false;
            this.AddDatabaseFileButton.Text = "Add scoring database file...";
            this.AddDatabaseFileButton.UseVisualStyleBackColor = true;
            this.AddDatabaseFileButton.Visible = false;
            this.AddDatabaseFileButton.Click += new System.EventHandler(this.AddDatabaseFileButton_Click);
            // 
            // DatabaseFileDialog
            // 
            this.DatabaseFileDialog.Filter = "BWS Files (*.bws)|*.bws";
            this.DatabaseFileDialog.Title = "Select Scoring Database";
            // 
            // SessionStatusLabel
            // 
            this.SessionStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SessionStatusLabel.ForeColor = System.Drawing.Color.Red;
            this.SessionStatusLabel.Location = new System.Drawing.Point(150, 194);
            this.SessionStatusLabel.Name = "SessionStatusLabel";
            this.SessionStatusLabel.Size = new System.Drawing.Size(203, 23);
            this.SessionStatusLabel.TabIndex = 0;
            this.SessionStatusLabel.Text = "Session Not Started";
            this.SessionStatusLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AnalysingLabel
            // 
            this.AnalysingLabel.AutoSize = true;
            this.AnalysingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnalysingLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.AnalysingLabel.Location = new System.Drawing.Point(15, 151);
            this.AnalysingLabel.Name = "AnalysingLabel";
            this.AnalysingLabel.Size = new System.Drawing.Size(120, 16);
            this.AnalysingLabel.TabIndex = 0;
            this.AnalysingLabel.Text = "Analysis Complete";
            this.AnalysingLabel.Visible = false;
            // 
            // AnalysingProgressBar
            // 
            this.AnalysingProgressBar.ForeColor = System.Drawing.Color.Blue;
            this.AnalysingProgressBar.Location = new System.Drawing.Point(141, 147);
            this.AnalysingProgressBar.Name = "AnalysingProgressBar";
            this.AnalysingProgressBar.Size = new System.Drawing.Size(352, 23);
            this.AnalysingProgressBar.Step = 1;
            this.AnalysingProgressBar.TabIndex = 0;
            this.AnalysingProgressBar.Visible = false;
            // 
            // HandRecordFileDialog
            // 
            this.HandRecordFileDialog.Filter = "PBN Files (*.pbn)|*.pbn";
            this.HandRecordFileDialog.Title = "Select Hand Record File";
            // 
            // AnalysisCalculationBackgroundWorker
            // 
            this.AnalysisCalculationBackgroundWorker.WorkerReportsProgress = true;
            this.AnalysisCalculationBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AnalysisCalculation_DoWork);
            this.AnalysisCalculationBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.AnalysisCalculation_ProgressChanged);
            this.AnalysisCalculationBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AnalysisCalculation_RunWorkerCompleted);
            // 
            // OptionsButton
            // 
            this.OptionsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OptionsButton.Location = new System.Drawing.Point(12, 189);
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.Size = new System.Drawing.Size(84, 34);
            this.OptionsButton.TabIndex = 3;
            this.OptionsButton.TabStop = false;
            this.OptionsButton.Text = "Options";
            this.OptionsButton.UseVisualStyleBackColor = true;
            this.OptionsButton.Visible = false;
            this.OptionsButton.Click += new System.EventHandler(this.OptionsButton_Click);
            // 
            // TabPlayForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(505, 234);
            this.Controls.Add(this.OptionsButton);
            this.Controls.Add(this.AnalysingProgressBar);
            this.Controls.Add(this.AnalysingLabel);
            this.Controls.Add(this.SessionStatusLabel);
            this.Controls.Add(this.AddDatabaseFileButton);
            this.Controls.Add(this.PathToHandRecordFileLabel);
            this.Controls.Add(this.HandRecordFileLabel);
            this.Controls.Add(this.AddHandRecordFileButton);
            this.Controls.Add(this.PathToDBLabel);
            this.Controls.Add(this.ScoringDatabaseLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TabPlayForm";
            this.Text = "TabPlayStarter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TabPlayForm_FormClosed);
            this.Load += new System.EventHandler(this.TabPlayForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ScoringDatabaseLabel;
        private System.Windows.Forms.Label PathToDBLabel;
        private System.Windows.Forms.Button AddHandRecordFileButton;
        private System.Windows.Forms.Label HandRecordFileLabel;
        private System.Windows.Forms.Label PathToHandRecordFileLabel;
        private System.Windows.Forms.Button AddDatabaseFileButton;
        private System.Windows.Forms.OpenFileDialog DatabaseFileDialog;
        private System.Windows.Forms.Label SessionStatusLabel;
        private System.Windows.Forms.Label AnalysingLabel;
        private System.Windows.Forms.ProgressBar AnalysingProgressBar;
        private System.Windows.Forms.OpenFileDialog HandRecordFileDialog;
        private System.ComponentModel.BackgroundWorker AnalysisCalculationBackgroundWorker;
        private System.Windows.Forms.Button OptionsButton;
    }
}