using Prism.Commands;

namespace CalibrationInstructionsManager.Core.Commands
{
    public interface IApplicationCommands
    {
        CompositeCommand EnableButtonCommand { get; }
    }
}