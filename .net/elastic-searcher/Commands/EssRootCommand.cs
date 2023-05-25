using System.CommandLine;

namespace elastic_searcher.Commands
{
    internal class EssRootCommand : RootCommand
    {
        public EssRootCommand(): base(EssMessages.Description)
        {
                
        }
    }
}

internal class EssMessages
{
    internal const string Description = "Application for reading data from Elasticsearch nodes.";
}