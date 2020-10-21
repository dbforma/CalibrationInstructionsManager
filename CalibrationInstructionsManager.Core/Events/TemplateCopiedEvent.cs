using System.Collections.Generic;
using CalibrationInstructionsManager.Core.Models.Templates;
using Prism.Events;

namespace CalibrationInstructionsManager.Core.Events
{
    public class TemplateCopiedEvent : PubSubEvent<Dictionary<string, int>>
    {
    }
}