using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ParameterService.Contract
{
    public interface IParameterService
    {
        void PassParametersToDatabase(Dictionary<string, int> dict);
        void GetParameters(ObservableCollection<object> parameters);
    }
}
