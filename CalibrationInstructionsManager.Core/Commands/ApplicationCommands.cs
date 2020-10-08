using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace CalibrationInstructionsManager.Core.Commands
{
    public class ApplicationCommands : IApplicationCommands
    {
        public CompositeCommand WriteToDatabaseCommand { get; } = new CompositeCommand(); 
        public CompositeCommand EnableButtonCommand { get; } = new CompositeCommand(); 

    }
}
