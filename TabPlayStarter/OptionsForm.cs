using System;
using System.Windows.Forms;


namespace TabScoreStarter
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
            EnterLeadCardCheckbox.Checked = opt.EnterLeadCard;
            ValidateLeadCardCheckbox.Checked = opt.ValidateLeadCard;
            NameSourceCombobox.SelectedIndex = opt.NameSource;
            NumberEntryEachRoundCheckbox.Checked = opt.NumberEntryEachRound;
            EnterResultsMethodCombobox.SelectedIndex = opt.EnterResultsMethod;
            TabletMovesCheckbox.Checked = opt.TabletMoves;

            ShowPercentageCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            ShowHandRecordCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            ValidateLeadCardCheckbox.Enabled = EnterLeadCardCheckbox.Checked;
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
                EnterLeadCard = EnterLeadCardCheckbox.Checked,
                ValidateLeadCard = ValidateLeadCardCheckbox.Checked,
                NameSource = NameSourceCombobox.SelectedIndex,
                NumberEntryEachRound = NumberEntryEachRoundCheckbox.Checked,
                EnterResultsMethod = EnterResultsMethodCombobox.SelectedIndex,
                TabletMoves = TabletMovesCheckbox.Checked
            };
            opt.UpdateDB();
            Properties.Settings.Default.TabletMoves = opt.TabletMoves;
            Properties.Settings.Default.Save();
            Close();
        }

        private void ShowTraveller_CheckedChanged(object sender, EventArgs e)
        {
            ShowPercentageCheckbox.Enabled = ShowTravellerCheckbox.Checked;
            ShowHandRecordCheckbox.Enabled = ShowTravellerCheckbox.Checked;
        }

        private void EnterLeadCard_CheckedChanged(object sender, EventArgs e)
        {
            ValidateLeadCardCheckbox.Enabled = EnterLeadCardCheckbox.Checked;
        }
    }
}
