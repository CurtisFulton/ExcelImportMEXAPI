using System.Collections.Generic;
using System.Linq;

namespace ImportTemplateView
{
    public class MainWindowViewModel : BaseViewModel
    {
        public TemplateEditorViewModel TemplateEditorViewModel { get; set; }
        public ExcelImportViewModel ExcelImportViewModel { get; set; }

        public MainWindowViewModel()
        {
            TemplateEditorViewModel = new TemplateEditorViewModel();
            ExcelImportViewModel = new ExcelImportViewModel();
        }
    }
}
