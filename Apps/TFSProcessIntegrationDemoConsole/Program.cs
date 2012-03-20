using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ChangeRequest_v1_0;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using StatusTracking_v1_0;

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
                activeProject.ClearADMUserStories(workItemStore);
                activeProject.ClearADMTasks(workItemStore);
            }
            using (var projectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(collectionUri))
            {
                var workItemStore = projectCollection.GetService<WorkItemStore>();
                Project activeProject = workItemStore.Projects[projectName];
                activeProject.AddADMUserStories(workItemStore);
                activeProject.AddADMTasks(workItemStore);
            }
        }

        private static DateTime? GetIssueDueDate(WorkItem issue)
        {
            if (issue.Fields.Contains("Due Date") == false)
                return null;
            Field field = issue.Fields["Due Date"];
            if (field.Value != null)
                return ((DateTime) field.Value).Date;
            return null;
        }
    }

    public static class Extensions
    {
        public static void AddADMTasks(this Project activeProject, WorkItemStore workItemStore)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory);
            dir = dir.Parent.Parent.Parent.Parent;
            dir = new DirectoryInfo(Path.Combine(dir.FullName, @"Abstractions\AbstractionContent\StatusTracking\In"));
            FileInfo[] xmlFiles = dir.GetFiles("*.xml", SearchOption.AllDirectories);
            foreach (FileInfo fInfo in xmlFiles)
            {
                StatusTrackingAbstractionType statusAbs = LoadXml<StatusTrackingAbstractionType>(fInfo.FullName);
                foreach(GroupType group in statusAbs.Groups)
                {
                    WorkItem parentTask = CreateTask(activeProject, workItemStore);
                    parentTask.Title = GetADMPrefixedString(group.name);
                    var groupItems =
                        statusAbs.StatusItems.Where(
                            statusItem => group.ItemRef.Count(itemRef => itemRef.itemName == statusItem.name) > 0).
                            ToArray();
                    foreach(var statusItem in groupItems)
                    {
                        if (statusItem.StatusValue.trafficLightIndicator == StatusValueTypeTrafficLightIndicator.green)
                            continue;
                        WorkItem childTask = CreateTask(activeProject, workItemStore);
                        childTask.Title = GetADMPrefixedString(statusItem.displayName);
                        childTask.Description = statusItem.description;
                        SetRemaining(childTask, statusItem.StatusValue.indicatorValue);
                        // Add Child Link
                        childTask.Save();
                        var linkTypeEnd = workItemStore.WorkItemLinkTypes[CoreLinkTypeReferenceNames.Hierarchy].ForwardEnd;
                        var childLink = new WorkItemLink(linkTypeEnd, parentTask.Id, childTask.Id);
                        parentTask.Links.Add(childLink);
                    }
                    parentTask.Save();
                }
            }
        }

        private static void SetRemaining(WorkItem childTask, decimal indicatorValue)
        {
            if (childTask.Fields.Contains("Remaining Work") == false)
                return;
            Field remaining = childTask.Fields["Remaining Work"];
            remaining.Value = indicatorValue;
        }

        public static void AddADMUserStories(this Project activeProject, WorkItemStore workItemStore)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory);
            dir = dir.Parent.Parent.Parent.Parent;
            dir = new DirectoryInfo(Path.Combine(dir.FullName, @"Abstractions\AbstractionContent\ChangeRequest\In"));
            FileInfo[] xmlFiles = dir.GetFiles("*.xml", SearchOption.AllDirectories);
            foreach(FileInfo fInfo in xmlFiles)
            {
                ChangeRequestAbstractionType crAbs = LoadXml<ChangeRequestAbstractionType>(fInfo.FullName);
                ChangeRequestPackageType package = crAbs.Item as ChangeRequestPackageType;
                if (package == null)
                    continue;
                WorkItem parentUserStory = CreateUserStory(activeProject, workItemStore);
                parentUserStory.Title = GetADMPrefixedString(package.name);
                parentUserStory.Description = "TODO: Package description";
                parentUserStory.Save();
                foreach(var pcr in package.PackagedChangeRequest)
                {
                    WorkItem childUserStory = CreateUserStory(activeProject, workItemStore);
                    childUserStory.Title = GetADMPrefixedString(pcr.name);
                    childUserStory.Description = pcr.Description;
                    // Add Child Link
                    childUserStory.Save();
                    var linkTypeEnd = workItemStore.WorkItemLinkTypes[CoreLinkTypeReferenceNames.Hierarchy].ForwardEnd;
                    var childLink = new WorkItemLink(linkTypeEnd, parentUserStory.Id, childUserStory.Id);
                    parentUserStory.Links.Add(childLink);
                }
                parentUserStory.Save();
            }
        }

        private static string GetADMPrefixedString(string strValue)
        {
            return "ADM: " + strValue;
        }

        private static WorkItem CreateUserStory(Project activeProject, WorkItemStore workItemStore)
        {
            WorkItemType wiType = activeProject.WorkItemTypes["User Story"];
            var workItem = new WorkItem(wiType);
            return workItem;
        }

        private static WorkItem CreateTask(Project activeProject, WorkItemStore workItemStore)
        {
            WorkItemType wiType = activeProject.WorkItemTypes["Task"];
            var workItem = new WorkItem(wiType);
            return workItem;
        }

        public static void ClearADMUserStories(this Project activeProject, WorkItemStore workItemStore)
        {
            const string wiqlQuery = "Select * from WorkItems where (State = 'Active') and [Work Item Type] = 'User Story'";
            var workItems = workItemStore.Query(wiqlQuery);
            List<int> destroyWorkItemIDs = new List<int>();
            foreach(WorkItem workItem in workItems)
            {
                if (workItem.Project != activeProject)
                    continue;
                if (workItem.Title.StartsWith("ADM: ") == false)
                    continue;
                destroyWorkItemIDs.Add(workItem.Id);
            }
            var result = workItemStore.DestroyWorkItems(destroyWorkItemIDs);
            int errCount = result.Count();
        }

        public static void ClearADMTasks(this Project activeProject, WorkItemStore workItemStore)
        {
            const string wiqlQuery = "Select * from WorkItems where (State = 'Active') and [Work Item Type] = 'Task'";
            var workItems = workItemStore.Query(wiqlQuery);
            List<int> destroyWorkItemIDs = new List<int>();
            foreach (WorkItem workItem in workItems)
            {
                if (workItem.Project != activeProject)
                    continue;
                if (workItem.Title.StartsWith("ADM: ") == false)
                    continue;
                destroyWorkItemIDs.Add(workItem.Id);
            }
            var result = workItemStore.DestroyWorkItems(destroyWorkItemIDs);
            int errCount = result.Count();
        }


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

        static T LoadXml<T>(string xmlFileName)
        {
            using (FileStream fStream = File.OpenRead(xmlFileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                T result = (T)serializer.Deserialize(fStream);
                fStream.Close();
                return result;
            }
        }
    }
}
