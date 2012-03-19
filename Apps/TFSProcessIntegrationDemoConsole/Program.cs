using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSProcessIntegrationDemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri collectionUri = new Uri(Properties.Settings.Default.TFSCollectionUrl);
            string projectName = Properties.Settings.Default.TFSProjectName;
            using(var projectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(collectionUri))
            {
                var workItemStore = projectCollection.GetService<WorkItemStore>();
                Project activeProject = workItemStore.Projects[projectName];
                WorkItem[] admIssues = activeProject.GetActiveADMIssues(workItemStore);
                DateTime?[] dueDates = admIssues.Select(GetIssueDueDate).ToArray();
            }
        }

        private static DateTime? GetIssueDueDate(WorkItem issue)
        {
            Field field = issue.Fields["Due Date"];
            if (field.Value != null)
                return ((DateTime) field.Value).Date;
            return null;
        }
    }

    public static class Extensions
    {
        public static WorkItem[] GetActiveADMIssues(this Project activeProject, WorkItemStore workItemStore)
        {
            const string wiqlQuery = "Select ID, Title from Issue where (State = 'Active') order by Title";
            var workItems = workItemStore.Query(wiqlQuery);
            List<WorkItem> result = new List<WorkItem>();
            foreach(WorkItem workItem in workItems)
            {
                if (workItem.Project != activeProject)
                    continue;
                if (workItem.Title.StartsWith("ADM: ") == false)
                    continue;
                result.Add(workItem);
            }
            return result.ToArray();
        }

    }
}
