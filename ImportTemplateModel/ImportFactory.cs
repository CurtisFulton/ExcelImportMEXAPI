using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ImportTemplateModel
{
    public static class ImportFactory
    {
        private static Dictionary<Type, IImportTemplate> ImportTemplates { get; set; } = GetImportTemplates();
        public static List<Type> AllTypes { get { return ImportTemplates.Keys.ToList(); } }

        private static Dictionary<Type, PropertyInfo[]> EditableProperties { get; set; } = new Dictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Gets all the properties required for a type
        /// </summary>
        /// <param name="type">The type to Test</param>
        public static PropertyInfo[] GetRequiredProperties(Type type)
        {
            if (!ImportTemplates.ContainsKey(type))
                throw new NotSupportedException($"'{type.Name}' is not a supported Import Type");

            return ImportTemplates[type].RequiredProperties;
        }

        /// <summary>
        /// Gets all the properties of a type that can be edited
        /// </summary>
        /// <param name="type">Type to get the editable properties from</param>
        public static PropertyInfo[] GetEditableProperties(Type type)
        {
            if (!ImportTemplates.ContainsKey(type))
                throw new NotSupportedException($"'{type.Name}' is not a supported Import Type");
            
            if (!EditableProperties.ContainsKey(type)) {
                PropertyInfo[] allProps = type.GetProperties();
                List<PropertyInfo> editableProps = new List<PropertyInfo>();

                foreach (var prop in allProps) {
                    if (IsEditableProperty(prop))
                        editableProps.Add(prop);
                }

                EditableProperties.Add(type, editableProps.ToArray());
            }

            return EditableProperties[type];
        }

        /// <summary>
        /// Checks if the property should be editable (Excludes Views, Extentions, etc).
        /// </summary>
        private static bool IsEditableProperty(PropertyInfo prop)
        {
            Type type = prop.DeclaringType;

            if (prop.Name == $"{type.Name}ID")
                return false;
            
            if (prop.PropertyType != typeof(string) && prop.PropertyType.IsClass)
                return false;

            foreach (var ignorableProp in ImportTemplates[type].IgnoredProperties)
            {
                if (ignorableProp == prop)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Create an object of the specified Type using the values provided
        /// </summary>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <param name="values">Values to assign to the object</param>
        /// <returns>An Object of type T initialized with the specified Values</returns>
        public static T CreateObject<T>(Dictionary<string, object> values)
        {   // Helper function to call it using Generics so you don't have to convert it yourself
            try {
                return (T)CreateObject(values, typeof(T));
            } catch (InvalidCastException) {
                Console.WriteLine($"Could not convert to type '{typeof(T).Name}'");
                return default(T);
            }
        }

        /// <summary>
        /// Create an object of the specified Type using the values provided
        /// </summary>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <param name="values">Values to assign to the object</param>
        /// <returns>An Object of type T initialized with the specified Values</returns>
        public static T CreateObject<T>(Dictionary<PropertyInfo, object> values)
        {   // Helper function to call it using Generics so you don't have to convert it yourself
            try {
                return (T)CreateObject(values, typeof(T));
            } catch (InvalidCastException) {
                Console.WriteLine($"Could not convert to type '{typeof(T).Name}'");
                return default(T);
            }
        }

        /// <summary>
        /// Create an object of the specified Type using the values provided
        /// </summary>
        /// <param name="values">Values to assign to the object</param>
        /// <param name="type">The type of object to create</param>
        /// <returns>An Object of the type initialized with the specified Values</returns>
        public static object CreateObject(Dictionary<string, object> values, Type type)
        {
            PropertyInfo[] allProps = type.GetProperties();
            Dictionary<PropertyInfo, object> newVals = new Dictionary<PropertyInfo, object>();

            foreach (var valuePair in values) {
                string propName = valuePair.Key;
                object val = valuePair.Value;

                PropertyInfo prop = allProps.Where(p => p.Name == propName).FirstOrDefault();
                
                if (prop == null)
                    throw new NullReferenceException($"There is no property '{propName}' for class '{type.Name}'");

                newVals.Add(prop, val);
            }

            return CreateObject(newVals, type);
        }

        /// <summary>
        /// Create an object of the specified Type using the values provided
        /// </summary>
        /// <param name="values">Values to assign to the object</param>
        /// <param name="type">The type of object to create</param>
        /// <returns>An Object of the type initialized with the specified Values</returns>
        public static object CreateObject(Dictionary<PropertyInfo, object> values, Type type)
        {   
            // Check that an import template exists for this type
            if (!ImportTemplates.ContainsKey(type))
                throw new NotSupportedException($"Cannot create an import for type '{type}'");

            IImportTemplate importTemplate = ImportTemplates[type];

            // Check that the values contain all required properties
            if (importTemplate.RequiredProperties != null && importTemplate.RequiredProperties.Length == 0) {
                foreach (var property in importTemplate.RequiredProperties) {
                    if (!values.ContainsKey(property))
                        throw new NullReferenceException($"You have not set all required values for an object of type '{type}'");
                }
            }

            object entity = importTemplate.CreateTemplateObject();

            SetDatesOnTemplateObject(entity, type);
            
            // Set all of the values specified
            foreach (var valuePair in values) {
                var property = valuePair.Key;
                var value = valuePair.Value;

                // Check to make sure the ID of this field is not being set
                if (property.Name == $"{type.Name}ID")
                    throw new ArgumentException($"You cannot set the ID for a {type.Name}");

                // Make sure that they are not overriding an automatic value
                if (importTemplate.IgnoredProperties.Contains(property))
                    continue;

                property.SetValue(entity, value);
            }

            return entity;
        }
        
        /// <summary>
        /// Creates an object with all the default and automatic values set from the import template
        /// </summary>
        /// <param name="type">The type of object to create</param>
        /// <param name="importTemplate">Import template to use</param>
        /// <returns></returns>
        private static object SetDatesOnTemplateObject(object entity, Type type)
        {
            // Set all non nullable dates to DateTime.Now by default
            var allDateProperties = type.GetProperties().Where(p => p.PropertyType == typeof(DateTime)).ToArray();
            
            if (allDateProperties != null && allDateProperties.Length > 0) {
                foreach (var dateProperty in allDateProperties) {
                    dateProperty.SetValue(entity, DateTime.Now);
                }
            }

            return entity;
        }

        /// <summary>
        /// Gets all the Import templates that have been written
        /// </summary>
        /// <returns></returns>
        private static Dictionary<Type, IImportTemplate> GetImportTemplates()
        {
            Dictionary<Type, IImportTemplate> importTemplates = new Dictionary<Type, IImportTemplate>();

            // Check all types in the current assembly and return all any that implement ImportTemplate<>
            var importTemplateTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => IsImportTemplate(type)).ToArray();

            foreach (var importTemplateType in importTemplateTypes) {
                // This is the type of object the import template creates (E.G Asset)
                var importType = importTemplateType.BaseType.GetGenericArguments()[0];

                // The import template class itself
                IImportTemplate importTemplate = Activator.CreateInstance(importTemplateType) as IImportTemplate;

                importTemplates.Add(importType, importTemplate);

                // This converts the property names into property info's
                importTemplate.SetRequiredAndIgnoredProperties();
            }

            return importTemplates;
        }

        /// <summary>
        /// Helper function to see if a type implements the ImportTemplate class
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if the type is an Import Template</returns>
        private static bool IsImportTemplate(Type type)
        {
            return !type.IsInterface && type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(ImportTemplate<>);
        }
    }
}
