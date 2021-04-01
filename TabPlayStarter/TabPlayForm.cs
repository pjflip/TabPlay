using System;
using System.ComponentModel;
using System.Data.Odbc;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace TabPlayStarter
{
    public partial class TabPlayForm : Form
    {
        private string pathToDB = "";

        public TabPlayForm()
        {
            InitializeComponent();
        }

        private void TabPlayForm_Load(object sender, EventArgs e)
        {
            Text = $"TabPlayStarter - Version {Assembly.GetExecutingAssembly().GetName().Version}";
            
            string argsString = "";
            string[] arguments = Environment.GetCommandLineArgs();

            // Parse command line args correctly to get DB path
            foreach (string s in arguments)
            {
                argsString = argsString + s + " ";
            }
            arguments = argsString.Split(new Char[] { '/' });
            foreach (string s in arguments)
            {
                if (s.StartsWith("f:["))
                {
                    pathToDB = s.Split(new char[] { '[', ']' })[1];
                    break;
                }
            }

            OdbcConnectionStringBuilder connectionString = null;
            if (pathToDB != "") connectionString = Database.ConnectionString(pathToDB);
            if (!Database.Initialize(connectionString))
            {
                AddDatabaseFileButton.Visible = true;   // No valid database in arguments
            }
            else
            {
                HandsList handsList = new HandsList(connectionString);
                if (handsList.Count == 0)
                {
                    PathToHandRecordFileLabel.Text = "Please add a hand record (.pbn) file";
                    AddHandRecordFileButton.Visible = true;    // No hand records in database, so let user add them
                }
                else
                {
                    PathToHandRecordFileLabel.Text = "Included in Scoring Database";
                    StartSession();
                }
            }
        }

        private void AddDatabaseFileButton_Click(object sender, EventArgs e)
        {
            if (DatabaseFileDialog.ShowDialog() == DialogResult.OK)
            {
                pathToDB = DatabaseFileDialog.FileName;
                OdbcConnectionStringBuilder connectionString = Database.ConnectionString(pathToDB);
                if (Database.Initialize(connectionString))
                {
                    AddDatabaseFileButton.Enabled = false;
                    HandsList handsList = new HandsList(connectionString);
                    if (handsList.Count == 0)
                    {
                        PathToHandRecordFileLabel.Text = "Please add a hand record (.pbn) file";
                        AddHandRecordFileButton.Visible = true;    // No hand records in database, so let user add them
                    }
                    else
                    {
                        PathToHandRecordFileLabel.Text = "Included in Scoring Database";
                        StartSession();
                    }
                }
            }
        }

        private void AddHandRecordFileButton_Click(object sender, EventArgs e)
        {
            if (HandRecordFileDialog.ShowDialog() == DialogResult.OK)
            {
                PathToHandRecordFileLabel.Text = HandRecordFileDialog.FileName;
                HandsList handsList = new HandsList(HandRecordFileDialog.FileName);
                if (handsList.Count == 0)
                {
                    MessageBox.Show("File contains no hand records", "TabPlayStarter", MessageBoxButtons.OK);
                }
                else
                {
                    handsList.WriteToDB(Database.ConnectionString(PathToDBLabel.Text));
                    AddHandRecordFileButton.Enabled = false;
                    StartSession();
                }
            }
        }

        private void StartSession()
        {
            PathToDBLabel.Text = pathToDB;
            string pathToTabPlayDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabPlay\TabPlayDB.txt");
            System.IO.File.WriteAllText(pathToTabPlayDB, Database.ConnectionString(pathToDB).ToString());
            SessionStatusLabel.Text = "Session Running";
            SessionStatusLabel.ForeColor = Color.Green;
            OptionsButton.Visible = true;
            AnalysingLabel.Text = "Analysing...";
            AnalysingLabel.Visible = true;
            AnalysingProgressBar.Visible = true;
            AnalysisCalculationBackgroundWorker.RunWorkerAsync();
        }

        private void TabPlayForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            PathToDBLabel.Text = "";
            string pathToTabPlayDB = Environment.ExpandEnvironmentVariables(@"%Public%\TabPlay\TabPlayDB.txt");
            System.IO.File.WriteAllText(pathToTabPlayDB, "");
        }

        private void AnalysisCalculation_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            OdbcConnectionStringBuilder connectionString = Database.ConnectionString(PathToDBLabel.Text);
            HandsList handsList = new HandsList(connectionString);
            HandEvaluationsList handEvaluationsList = new HandEvaluationsList(connectionString);
            int counter = 0;
            foreach (Hand hand in handsList)
            {
                HandEvaluation handEvaluation = new HandEvaluation(hand);
                handEvaluationsList.Add(handEvaluation);
                counter++;
                worker.ReportProgress((int)((float)counter / (float)handsList.Count * 100.0));
            }
            handEvaluationsList.WriteToDB();
        }

        private void AnalysisCalculation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            AnalysingProgressBar.Value = e.ProgressPercentage;
        }

        private void AnalysisCalculation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            AnalysingProgressBar.Value = 100;
            AnalysingLabel.Text = "Analysis Complete";
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            Point mainFormLocation = Location;
            OptionsForm frmOptions = new OptionsForm
            {
                Tag = PathToDBLabel.Text,
                Location = new Point(mainFormLocation.X + 30, mainFormLocation.Y + 30)
            };
            frmOptions.ShowDialog();
        }
    }
}
