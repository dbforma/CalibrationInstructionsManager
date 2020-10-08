using Prism.Commands;

namespace CalibrationInstructionsManager.Core.Commands
{
    public interface IApplicationCommands
    {
        CompositeCommand WriteToDatabaseCommand { get; }
        CompositeCommand EnableButtonCommand { get; }
    }
}