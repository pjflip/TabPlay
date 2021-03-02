using System;
using System.Windows.Forms;

namespace TabPlayStarter
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            Options opt = new Options(new Database(Tag.ToString()));

            ShowTravellerCheckbox.Checked = opt.ShowTraveller;
            ShowPercentageCheckbox.Checked = opt.ShowPercentage;
            ShowHandRecordCheckbox.Checked = opt.ShowHandRecord;
            ShowRankingCombobox.SelectedIndex = opt.ShowRanking;
            NameSourceCombobox.SelectedIndex = opt.NameSource;
            NumberEntryEachRoundCheckbox.Checked = opt.NumberEntryEachRound;
            PollIntervalNud.Value = opt.PollInterval;

            ShowPercentageCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            ShowHandRecordCheckbox.Enabled = ShowTravellerCheckbox.Checked;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Options opt = new Options(new Database(Tag.ToString()))
            {
                ShowTraveller = ShowTravellerCheckbox.Checked,
                ShowPercentage = ShowPercentageCheckbox.Checked,
                ShowHandRecord = ShowHandRecordCheckbox.Checked,
                ShowRanking = ShowRankingCombobox.SelectedIndex,
                NameSource = NameSourceCombobox.SelectedIndex,
                NumberEntryEachRound = NumberEntryEachRoundCheckbox.Checked,
                PollInterval = Convert.ToInt32(PollIntervalNud.Value)
            };
            opt.UpdateDB();
            TabPlayStarter.Properties.Settings.Default.PollInterval = opt.PollInterval;
            TabPlayStarter.Properties.Settings.Default.Save();
            Close();
        }

        private void ShowTraveller_CheckedChanged(object sender, EventArgs e)
        {
            ShowPercentageCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            ShowHandRecordCheckbox.Enabled = ShowTravellerCheckbox.Checked;
        }
    }
}
