using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using McTools.Xrm.Connection;
using System.Windows.Controls;
using Solution_Quality_Checker.Models;
using Microsoft.Xrm.Sdk.Organization;

namespace Solution_Quality_Checker
{
    public partial class SQCControl : PluginControlBase
    {
        private Settings mySettings;

        public SQCControl()
        {
            InitializeComponent();
        }

        private void SQCControl_Load(object sender, EventArgs e)
        {
            //ShowInfoNotification("This is a notification that can lead to XrmToolBox repository", new Uri("https://github.com/MscrmTools/XrmToolBox"));

            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SQCControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }


        IEnumerable<Entity> originalSolutions = new List<Entity>();
        private void btnLoadSolutions_Click(object sender, EventArgs e)
        {

            lstSolutions.Items.Clear();

            QueryExpression solutionsQuery = new QueryExpression("solution");
            solutionsQuery.ColumnSet = new ColumnSet(true);
            solutionsQuery.Criteria.AddCondition("ismanaged", ConditionOperator.Equal, false);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting all unmanaged solutions",

                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(solutionsQuery);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as EntityCollection;
                    if (result != null)
                    {
                        originalSolutions = result.Entities.ToList();
                        foreach (Entity solution in result.Entities)
                        {
                            ListBoxItem lstItem = new ListBoxItem();
                            lstItem.Name = solution.GetAttributeValue<string>("uniquename");
                            lstItem.Content = solution.GetAttributeValue<string>("friendlyname");
                            lstSolutions.Items.Add(lstItem);
                        }
                    }
                }
            });
        }

        private void lstSolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listItem = lstSolutions.SelectedItem;
            if (listItem != null)
            {
                btnCheckSolution.Visible = true;
                btnCheckSolution.Enabled = true;
            }
            else
            {
                btnCheckSolution.Visible = false;
                btnCheckSolution.Enabled = false;
            }
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            ValidationSettingsForm settingsForm = new ValidationSettingsForm(mySettings);
            settingsForm.Text = "Quality Settings";
            settingsForm.ShowDialog();
        }

        private void btnCheckSolution_Click(object sender, EventArgs e)
        {
            CRMSolution crmSolution;
            SolutionHealthManager healthManager = new SolutionHealthManager(Service);
            var selectedSolutionItem = lstSolutions.SelectedItem as ListBoxItem;
            if (selectedSolutionItem != null)
            {
                Entity solutionRecord = originalSolutions.FirstOrDefault(x => x.GetAttributeValue<string>("uniquename") == selectedSolutionItem.Name);
                crmSolution = new CRMSolution(solutionRecord);
                ValidationResults finalResults = new ValidationResults();

                WorkAsync(new WorkAsyncInfo
                {
                    Message = "Checking solution health ... ",

                    Work = (worker, args) =>
                    {
                        healthManager.OnProgressChanged += (source, progressArgs) =>
                        {
                            worker.ReportProgress(0, progressArgs.Message);
                        };
                        finalResults = healthManager.Validate(crmSolution);
                    },
                    PostWorkCallBack = (args) =>
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        // bind the finalResults here
                        foreach (Models.ValidationResult vr in finalResults.ResultRecords)
                        {
                            gvResults.Rows.Add(vr.Type, vr.Description, vr.Suggestions, vr.PriorityLevel);
                        }
                    },
                    ProgressChanged = (progressArgs) =>
                    {
                        SetWorkingMessage(progressArgs.UserState.ToString());
                    },
                });

            }
            else
            {
                MessageBox.Show("Please select a solution from the list first.");
            }
        }


    }
}