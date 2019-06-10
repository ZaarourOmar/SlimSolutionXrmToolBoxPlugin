using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SlimSolution.Models;

namespace SlimSolution
{
    public partial class ValidationSettingsForm : Form
    {
        public Settings AppSettings { get; set; }

        public ValidationSettingsForm(Settings mySettings)
        {
            InitializeComponent();
            AppSettings = mySettings;
            MapSettingsToCheckboxes();
        }



        private void MapCheckboxesToSettings()
        {
            AppSettings.ValidationSettings[0].Value = lstSettings.GetItemChecked(0);
            AppSettings.ValidationSettings[1].Value = lstSettings.GetItemChecked(1);
            AppSettings.ValidationSettings[2].Value = chkBoxAlwaysPublish.Checked;
        }

        private void MapSettingsToCheckboxes()
        {
            lstSettings.SetItemChecked(0, AppSettings.ValidationSettings[0].Value);
            lstSettings.SetItemChecked(1, AppSettings.ValidationSettings[1].Value);
            chkBoxAlwaysPublish.Checked = AppSettings.ValidationSettings[2].Value;
        }
        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            MapCheckboxesToSettings();
            this.Close();
        }
    }
}
