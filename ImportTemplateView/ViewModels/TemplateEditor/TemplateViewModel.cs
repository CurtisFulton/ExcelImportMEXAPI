using ImportTemplateModel;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ImportTemplateView
{
    public class TemplateViewModel : BaseViewModel
    {
        public string TemplateName { get; set; }

        private Type _templateType;
        public Type TemplateType {
            get { { return _templateType; } }
            set {
                if (_templateType == value)
                    return;

                _templateType = value;
                OnTemplateTypeChanged();
            }
        }

        public ObservableCollection<TemplateColumn> TemplateColumns { get; set; }

        [JsonIgnore]
        public ObservableCollection<string> PropertyNames { get; set; } = new ObservableCollection<string>();

        public TemplateViewModel(string templateName, Type type)
        {
            TemplateName = templateName;
            TemplateColumns = new ObservableCollection<TemplateColumn>();

            TemplateType = type;
        }

        private void OnTemplateTypeChanged()
        {
            TemplateColumns.Clear();
            PropertyNames.Clear();
            var allProps = ImportFactory.GetEditableProperties(TemplateType);

            foreach (var prop in allProps) {
                PropertyNames.Add(prop.Name);
            }

            var requiredProperties = ImportFactory.GetRequiredProperties(TemplateType);
            foreach (var prop in requiredProperties) {
                TemplateColumns.Add(new TemplateColumn() {
                    IsRequired = true,
                    IsMandatory = true,
                    PropertyName = prop.Name,
                    ColumnName = prop.Name
                });
            }
        }
    }

    public struct TemplateColumn
    {
        public bool IsMandatory { get; set; }
        public bool IsRequired { get; set; }

        public string PropertyName { get; set; }

        public string ColumnName { get; set; }
    }
}