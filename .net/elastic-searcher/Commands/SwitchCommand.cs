using System.CommandLine;
using System.CommandLine.Parsing;
namespace elastic_searcher.Commands;
internal class SwitchCommand : Command
{
    public SwitchCommand() : base(SwitchMessages.Name, SwitchMessages.Description)
    {
        var arg = new SwitchArgument();
        this.AddArgument(arg);
        this.SetHandler(SetHandler, arg);
    }

    private void SetHandler(byte ordinal)
    {
        if (Context.HasZeroClients)
        {
            ConsoleExtension.WriteInfo(SwitchMessages.NoConnectionEstablished);
        }
        else
        {
            if (ordinal > Context.Clients.Length)
            {
                ConsoleExtension.WriteError(SwitchMessages.ArgOutOfRange);
            }
            else
            {
                Context.SetCurrentURI(ordinal);
                ConsoleExtension.WriteSuccess(SwitchMessages.ConnectionSwitched);
            }
        }
    }
}


internal class SwitchArgument : Argument<byte>
{
    public SwitchArgument() : base(SwitchMessages.ArgName, SwitchMessages.ArgDescription)
    {
        this.AddValidator(Validator);
    }

    private void Validator(ArgumentResult result)
    {
        try
        {
            var ordinal = result.GetValueOrDefault<byte>();
            if (ordinal == 0)
            {
                result.ErrorMessage = SwitchMessages.ArgToSmall;
            }
        }
        catch (InvalidOperationException)
        {
        }
        catch (Exception)
        {
            throw;
        }
    }
}


internal class SwitchMessages
{
    internal const string Name = "switch";
    internal const string Description = "Switch connection.";
    internal const string NoConnectionEstablished = "No connection established yet.";
    internal const string ArgName = "Connection ordinal";
    internal const string ArgDescription = "Ordinal number of the saved connection.";
    internal const string ArgEmpty = "Ordinal cannot be empty.";
    internal const string ArgNotWellFormatted = "Ordinal is not well formed.";
    internal const string TooManyArgs = "Too many arguments.";
    internal const string ArgToSmall = "Ordinal must be greater than 0.";
    internal const string ArgOutOfRange = "Ordinal out of range.";
    internal const string ConnectionSwitched = "Connection switched";
}

