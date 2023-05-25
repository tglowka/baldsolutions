using System.CommandLine;

namespace elastic_searcher.Commands
{
    internal class ListCommand : Command
    {
        public ListCommand() : base(ListMessages.Name, ListMessages.Description)
        {
            this.SetHandler(SetHandler);
        }

        private void SetHandler()
        {
            if (!Context.Clients.Any())
            {
                ConsoleExtension.WriteInfo(ListMessages.NoConnectionEstablished);
            }
            else
            {
                var clients = Context.Clients
                    .OrderBy(x => x.Key)
                    .ToArray();

                for (int i = 0; i < clients.Length; i++)
                {
                    ConsoleExtension.WriteInfo($"{(clients[i].Key == Context.CurrentURI ? "*" : "")}{i+1}. {clients[i].Key}");
                }
            }
        }
    }

    internal class ListMessages
    {
        internal const string Name = "ls";
        internal const string Description = "List established connections.";
        internal const string NoConnectionEstablished = "No connection established yet.";
    }
}
