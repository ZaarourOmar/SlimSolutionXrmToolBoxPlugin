using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using SlimSolution.Models;

namespace SlimSolution.Validators
{
    /// <summary>
    /// This validator is responsible for Process Validation. For now it only finds those process that are inactive in an unmanaged solution.
    /// </summary>
    public class ProcessValidator : IValidator
    {

        public event EventHandler<ErrorEventArgs> OnValidatorError;
        public event EventHandler<ProgressEventArgs> OnValidatorProgress;

        public IOrganizationService CRMService { get; set; }
        public string Message { get { return "Checking Processes"; } }

        public ProcessValidator(IOrganizationService service)
        {
            CRMService = service;
        }



        public ValidationResults Validate(CRMSolution solution, out List<CRMSolutionComponent> extraComponents)
        {
            try
            {
                ValidationResults results = new ValidationResults();

                //get all solution compontents of type workflow that belong to the specified solution
                QueryExpression processQuery = new QueryExpression("solutioncomponent");
                processQuery.ColumnSet = new ColumnSet(true);
                processQuery.Criteria.AddCondition("componenttype", ConditionOperator.Equal, Constants.PROCESS_COMPONENT_TYPE);
                processQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, solution.Id);

                var allProcesses = CRMService.RetrieveMultiple(processQuery);
                if (allProcesses != null && allProcesses.Entities.Count > 0)
                {
                    results = ValidateProcesses(allProcesses.Entities);
                }

                extraComponents = null;
                return results;
            }
            catch (Exception ex)
            {
                OnValidatorError?.Invoke(this, new ErrorEventArgs(ex));
                extraComponents = null;
                return null;
            }

        }



        /// <summary>
        /// For now, this function checks for inactive processes that needs to be removed form the solution
        /// </summary>
        /// <param name="processEntities"></param>
        /// <returns></returns>
        private ValidationResults ValidateProcesses(DataCollection<Entity> processEntities)
        {
            ValidationResults results = new ValidationResults();

            QueryExpression processesQuery = new QueryExpression("workflow");
            processesQuery.ColumnSet = new ColumnSet(true);
            var allIds = processEntities.Select(x => x.GetAttributeValue<Guid>("objectid")).ToArray<Guid>();

            processesQuery.Criteria.AddCondition(new ConditionExpression("workflowid", ConditionOperator.In, allIds));
            var fullProcesses = CRMService.RetrieveMultiple(processesQuery);


            //check if any is in draft mode
            foreach (var fullProcess in fullProcesses.Entities)
            {
                // check if the process is activated or not
                if (fullProcess.GetAttributeValue<OptionSetValue>("statuscode").Value == 1)
                {
                    var singleResult = new ValidationResult();
                    singleResult.Description = $"{fullProcess.GetAttributeValue<string>("name")} is not activated";
                    singleResult.Suggestions = $"Inactive processes should be removed from unmanaged solutions";
                    singleResult.PriorityLevel = ValidationResultLevel.Medium;
                    singleResult.Type = fullProcess.FormattedValues["category"];
                    results.AddResult(singleResult);
                }
            }

            return results;

        }
    }
}
