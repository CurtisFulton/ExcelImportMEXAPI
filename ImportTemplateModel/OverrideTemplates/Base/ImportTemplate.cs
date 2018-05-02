using MEXModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ImportTemplateModel
{
    internal abstract class ImportTemplate<T> : IImportTemplate
    {
        protected T entity;

        public PropertyInfo[] RequiredProperties { get; private set; }
        public PropertyInfo[] IgnoredProperties { get; private set; }

        protected string[] RequiredPropertyNames { get; set; }
        protected string[] IgnoredPropertyNames { get; set; }
        
        /// <summary>
        /// Created the Properties required from the Interface
        /// </summary>
        public void SetRequiredAndIgnoredProperties()
        {
            PropertyInfo[] allProps = typeof(T).GetProperties();

            RequiredProperties = new PropertyInfo[0];
            IgnoredProperties = new PropertyInfo[0];

            // If there are any Required Properties
            if (RequiredPropertyNames != null && RequiredPropertyNames.Length > 0) {
                RequiredProperties = new PropertyInfo[RequiredPropertyNames.Length];

                for (int i = 0; i < RequiredPropertyNames.Length; i++) {
                    RequiredProperties[i] = allProps.Where(p => p.Name.ToLower() == RequiredPropertyNames[i].ToLower()).FirstOrDefault();

                    if (RequiredProperties[i] == null)
                        throw new NullReferenceException($"There is no Property called '{RequiredPropertyNames[i]}' for a class of type {typeof(T).Name}.");
                }
            }

            // If there are any Ignored Properties
            if (IgnoredPropertyNames != null && IgnoredPropertyNames.Length > 0) {
                IgnoredProperties = new PropertyInfo[IgnoredPropertyNames.Length];
                
                for (int i = 0; i < IgnoredPropertyNames.Length; i++) {
                    IgnoredProperties[i] = allProps.Where(p => p.Name.ToLower() == IgnoredPropertyNames[i].ToLower()).FirstOrDefault();

                    if (IgnoredProperties[i] == null)
                        throw new NullReferenceException($"There is no Property called '{IgnoredPropertyNames[i]}' for a class of type {typeof(T).Name}.");
                }
            }
        }

        public abstract object CreateTemplateObject();
    }
}
