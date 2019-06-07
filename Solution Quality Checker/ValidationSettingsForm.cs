using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Solution_Quality_Checker.Models;

namespace Solution_Quality_Checker
{
    public partial class ValidationSettingsForm : Form
    {
        public Settings AppSettings { get; set; }

        public ValidationSettingsForm(Settings mySettings)
        {
            InitializeComponent();
            AppSettings = mySettings;
           // MapSettingsToCheckboxes();
        }


        private void CheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void MapCheckboxesToSettings()
        {
            AppSettings.CurrentValidationSettings.Clear();

            foreach (var item in lstSettings.CheckedItems)
            {
                switch (item.ToString())
                {
                    case "Extra Components":
                        AppSettings.CurrentValidationSettings.CheckExtraComponents = true;
                        break;
                    case "Processes":
                        AppSettings.CurrentValidationSettings.CheckProcesses = true;
                        break;
                    case "Business Rules":
                        AppSettings.CurrentValidationSettings.CheckBusinessRules = true;
                        break;
                    case "BPF":
                        AppSettings.CurrentValidationSettings.CheckBPF = true;
                        break;
                    case "Dependencies":
                        AppSettings.CurrentValidationSettings.CheckDependencies = true;
                        break;
                    case "Modularity":
                        AppSettings.CurrentValidationSettings.CheckModularity = true;
                        break;
                    case "Web Resources":
                        AppSettings.CurrentValidationSettings.CheckWebResources = true;
                        break;
                    case "Publishers":
                        AppSettings.CurrentValidationSettings.CheckPublishers = true;
                        break;
                }
            }
        }

        private void MapSettingsToCheckboxes()
        {

            for (int i = 0; i < lstSettings.Items.Count; i++)
                lstSettings.SetItemChecked(i, true);

            if (!AppSettings.CurrentValidationSettings.CheckExtraComponents)
            {
                lstSettings.SetItemChecked(0, false);
            }
            if (!AppSettings.CurrentValidationSettings.CheckProcesses)
            {
                lstSettings.SetItemChecked(1, false);
            }
            if (!AppSettings.CurrentValidationSettings.CheckBusinessRules)
            {
                lstSettings.SetItemChecked(2, false);
            }
            if (!AppSettings.CurrentValidationSettings.CheckDependencies)
            {
                lstSettings.SetItemChecked(3, false);
            }
            if (!AppSettings.CurrentValidationSettings.CheckPublishers)
            {
                lstSettings.SetItemChecked(4, false);
            }
            if (!AppSettings.CurrentValidationSettings.CheckWebResources)
            {
                lstSettings.SetItemChecked(5, false);
            }
            if (!AppSettings.CurrentValidationSettings.CheckModularity)
            {
                lstSettings.SetItemChecked(6, false);
            }
            if (!AppSettings.CurrentValidationSettings.CheckBPF)
            {
                lstSettings.SetItemChecked(7, false);
            }
        }
        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
           // MapCheckboxesToSettings();
            this.Close();
        }
    }
}
