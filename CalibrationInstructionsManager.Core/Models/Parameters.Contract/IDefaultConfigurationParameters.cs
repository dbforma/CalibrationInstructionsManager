using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalibrationInstructionsManager.Core.Models.Parameters.Contract
{
    public interface IDefaultConfigurationParameters
    {
        int ParameterId { get; set; }
        int TemplateId { get; set; }
        int TypeId { get; set; }
        string TypeName { get; set; }
        double? Value { get; set; }
    }
}
