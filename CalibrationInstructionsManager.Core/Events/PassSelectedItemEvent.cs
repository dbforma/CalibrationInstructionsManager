using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalibrationInstructionsManager.Core.Models.Templates;
using Prism.Events;

namespace CalibrationInstructionsManager.Core.Events
{
    public class PassSelectedItemEvent : PubSubEvent<DefaultConfigurationTemplate>
    {
    }
}
