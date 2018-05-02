using ImportTemplateModel;
using MEXModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace ImportTemplateView
{
    public class TemplateEditorViewModel : BaseViewModel
    {
        public ObservableCollection<TemplateViewModel> AllTemplates { get; set; }
        public ObservableCollection<Type> AllTypes { get; set; } = new ObservableCollection<Type>(ImportFactory.AllTypes);
        public TemplateViewModel SelectedTemplate { get; set; }

        private string TemplateFolderLocation { get; set; }

        public ICommand NewTemplateCommand { get; set; }
        public ICommand AddColumnCommand { get; set; }
        public ICommand RemoveColumnCommand { get; set; }
        public ICommand SaveCurrentTemplateCommand { get; set; }

        public TemplateEditorViewModel()
        {
            NewTemplateCommand = new RelayCommandParam(CreateNewTemplate);
            SaveCurrentTemplateCommand = new RelayCommand(SaveCurrentTemplateToJSON);

            AddColumnCommand = new RelayCommand(AddNewTemplateColumn);
            RemoveColumnCommand = new RelayCommand(RemoveTemplateColumn);

            TemplateFolderLocation = Path.Combine(Directory.GetCurrentDirectory(), "Templates");
            
            if (!Directory.Exists(TemplateFolderLocation)) 
                Directory.CreateDirectory(TemplateFolderLocation);

            AllTemplates = GetAllTemplates();
        }

        // This is used for testing when there are no templates created yet
        private void CreateTestTemplates()
        {
            var test = new TemplateViewModel("Asset Template", typeof(Asset));
            var test2 = new TemplateViewModel("Work Order Template", typeof(WorkOrder));

            test.TemplateColumns.Add(new TemplateColumn() { ColumnName = "Test", PropertyName = "TestProp" });
            test2.TemplateColumns.Add(new TemplateColumn() { ColumnName = "Test", PropertyName = "TestProp" });

            string templateString = JsonConvert.SerializeObject(test, Formatting.Indented);
            File.WriteAllText(Path.Combine(TemplateFolderLocation, test.TemplateName + ".json"), templateString);

            templateString = JsonConvert.SerializeObject(test2, Formatting.Indented);
            File.WriteAllText(Path.Combine(TemplateFolderLocation, test2.TemplateName + ".json"), templateString);
        }

        private ObservableCollection<TemplateViewModel> GetAllTemplates()
        {
            string[] files = Directory.GetFiles(TemplateFolderLocation);
            TemplateViewModel[] importTemplates = new TemplateViewModel[files.Length];

            for (int i = 0; i < files.Length; i++) {
                importTemplates[i] = JsonConvert.DeserializeObject<TemplateViewModel>(File.ReadAllText(files[i]));
            }

            return new ObservableCollection<TemplateViewModel>(importTemplates);
        }

        private void CreateNewTemplate(object param)
        {
            string templateName = (string)param;

            if (string.IsNullOrWhiteSpace(templateName))
                return;

            foreach (var template in AllTemplates) {
                if (template.TemplateName == templateName)
                    return;
            }

            TemplateViewModel newTemplate = new TemplateViewModel((string)templateName, AllTypes[0]);

            AllTemplates.Add(newTemplate);
            SelectedTemplate = newTemplate;
        }

        private void SaveCurrentTemplateToJSON()
        {
            string templateString = JsonConvert.SerializeObject(SelectedTemplate, Formatting.Indented);

            File.WriteAllText(Path.Combine(TemplateFolderLocation, SelectedTemplate.TemplateName + ".json"), templateString);
        }

        private void AddNewTemplateColumn()
        {
            if (SelectedTemplate == null)
                return;

            SelectedTemplate.TemplateColumns.Add(new TemplateColumn());
        }

        private void RemoveTemplateColumn()
        {
            if (SelectedTemplate == null)
                return;

            TemplateColumn lastItem = SelectedTemplate.TemplateColumns.Last();

            if (lastItem.IsRequired)
                return;

            SelectedTemplate.TemplateColumns.Remove(lastItem);
        }
    }
}