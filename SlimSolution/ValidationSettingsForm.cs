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
            AppSettings.CheckComponents = lstSettings.GetItemChecked(0);
            AppSettings.CheckProcesses = lstSettings.GetItemChecked(1);
            AppSettings.AlwaysPublish = chkBoxAlwaysPublish.Checked;
        }

        private void MapSettingsToCheckboxes()
        {
            lstSettings.SetItemChecked(0, AppSettings.CheckComponents);
            lstSettings.SetItemChecked(1, AppSettings.CheckProcesses);
            chkBoxAlwaysPublish.Checked = AppSettings.AlwaysPublish;
        }
        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            MapCheckboxesToSettings();
            this.Close();
        }
    }
}
