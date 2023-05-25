using System.CommandLine;

namespace elastic_searcher.Commands
{
    internal class ClusterCommand : Command
    {
        public ClusterCommand() : base(ClusterMessages.Name, ClusterMessages.Description)
        {
            var arg = new ClusterOperationArg();
            this.AddArgument(arg);
            this.SetHandler(SetHandler, arg);
        }

        private async Task SetHandler(string operation)
        {
            switch (operation)
            {
                case "health":
                    {
                        await OperationsHandler.HandleOperationAsync(Context.CurrentClient.Cluster.HealthAsync);
                        break;
                    }
                case "pending-tasks":
                    {
                        await OperationsHandler.HandleOperationAsync(Context.CurrentClient.Cluster.PendingTasksAsync);
                        break;
                    }
                case "get-settings":
                    {
                        await OperationsHandler.HandleOperationAsync(Context.CurrentClient.Cluster.GetSettingsAsync);
                        break;
                    }
                default:
                    {
                        ConsoleExtension.WriteWarning(ClusterMessages.OperationNotDefined);
                        break;
                    }
            }
        }
    }

    internal class ClusterOperationArg : Argument<string>
    {
        public ClusterOperationArg() : base(ClusterMessages.ArgName, ClusterMessages.ArgDescription)
        { }
    }

    internal class ClusterMessages
    {
        internal const string Name = "cluster";
        internal const string Description = "Get cluster data.";
        internal const string ArgName = "Cluster operation";
        internal const string ArgDescription = "Name of the cluster operation.";
        internal const string OperationNotDefined = "Operation not defined.";
    }
}
