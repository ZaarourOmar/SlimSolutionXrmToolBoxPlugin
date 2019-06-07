using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Solution_Quality_Checker.Models;

namespace Solution_Quality_Checker.Validators
{
    public class ProcessValidator : Validator
    {
        public ProcessValidator(IOrganizationService service) : base(service)
        {
        }
        public override string Message => "Checking Processes";

        public override ValidationResults Validate(CRMSolution solution)
        {
            ValidationResults results = new ValidationResults();

            //get all solution compontents of type workflow that belong to the specified solution
            QueryExpression processQuery = new QueryExpression("solutioncomponent");
            processQuery.ColumnSet = new ColumnSet(true);
            processQuery.Criteria.AddCondition("componenttype", ConditionOperator.Equal, 29); //worflow type
            processQuery.Criteria.AddCondition("solutionid", ConditionOperator.Equal, solution.Id);

            var allProcesses = CRMService.RetrieveMultiple(processQuery);
            if (allProcesses != null && allProcesses.Entities.Count > 0)
            {
                results = ValidateProcesses(allProcesses.Entities);
            }

            return results;
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
