using System.Collections.ObjectModel;

namespace TemplateService.Contract
{
    public interface ITemplateService
    {
        void CopyTemplate(object selectedItem);
        void CopyParameters();
        void GetTemplates(ObservableCollection<object> Templates);
    }
}
