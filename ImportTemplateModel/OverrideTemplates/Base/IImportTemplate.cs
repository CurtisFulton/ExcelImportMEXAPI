using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ImportTemplateModel
{
    internal interface IImportTemplate
    {
        PropertyInfo[] RequiredProperties { get; }
        PropertyInfo[] IgnoredProperties { get; }

        object CreateTemplateObject();
        void SetRequiredAndIgnoredProperties();
    }
}
