using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace CCMS.Data.Conventions
{
   public class CustomPropertyAccessConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            var entityType = instance.EntityType;
            var camelCaseUnderscoreName = ConvertToCamelCaseUnderscore(instance.Name);

            bool hasBackingField = HasField(entityType, camelCaseUnderscoreName);

            // Default is to use property setter, so only modify mapping
            // if there is a backing field

            if (hasBackingField)
                instance.Access.CamelCaseField(CamelCasePrefix.Underscore);
        }

        private static string ConvertToCamelCaseUnderscore(string propertyName)
        {
            return "_" +
                propertyName[0].ToString().ToLower() +
                propertyName.Substring(1);
        }

        private bool HasField(Type type, string fieldName)
        {
            var backingField = type.GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            return backingField != null;
        }
    }
}

