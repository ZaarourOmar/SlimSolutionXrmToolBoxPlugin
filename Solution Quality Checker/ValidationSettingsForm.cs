﻿using System;
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
            MapSettingsToCheckboxes();
        }


        private void CheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void MapCheckboxesToSettings()
        {
            ValidationSettings.CurrentValidationSettings.SettingsKVPs["CheckEntityComponents"] = lstSettings.GetItemChecked(0);
            ValidationSettings.CurrentValidationSettings.SettingsKVPs["CheckProcesses"] = lstSettings.GetItemChecked(1);
            ValidationSettings.CurrentValidationSettings.SettingsKVPs["CheckPublishers"] = lstSettings.GetItemChecked(2);
            ValidationSettings.CurrentValidationSettings.SettingsKVPs["CheckCode"] = lstSettings.GetItemChecked(3);
        }

        private void MapSettingsToCheckboxes()
        {

            for (int i = 0; i < lstSettings.Items.Count; i++)
                lstSettings.SetItemChecked(i, true);

            if (!ValidationSettings.CurrentValidationSettings.SettingsKVPs["CheckEntityComponents"])
            {
                lstSettings.SetItemChecked(0, false);
            }
            if (!ValidationSettings.CurrentValidationSettings.SettingsKVPs["CheckProcesses"])
            {
                lstSettings.SetItemChecked(1, false);
            }
            if (!ValidationSettings.CurrentValidationSettings.SettingsKVPs["CheckPublishers"])
            {
                lstSettings.SetItemChecked(2, false);
            }
            if (!ValidationSettings.CurrentValidationSettings.SettingsKVPs["CheckCode"])
            {
                lstSettings.SetItemChecked(3, false);
            }

        }
        private void BtnSaveSettings_Click(object sender, EventArgs e)
        {
            MapCheckboxesToSettings();
            this.Close();
        }
    }
}