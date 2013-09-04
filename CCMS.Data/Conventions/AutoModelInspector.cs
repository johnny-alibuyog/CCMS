using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate.Mapping.ByCode;

namespace CCMS.Data.Conventions
{
    internal class AutoModelInspector : IModelInspector
    {
        private readonly HashSet<Type> _tablePerHierarchy = new HashSet<Type>();
        private readonly HashSet<Type> _tablePerConcreteClass = new HashSet<Type>();

        #region IModelInspector Members

        public Type GetDynamicComponentTemplate(MemberInfo member)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetPropertiesSplits(Type type)
        {
            return new string[0];
        }

        public bool IsAny(MemberInfo member)
        {
            return false;
        }

        public bool IsArray(MemberInfo role)
        {
            throw new NotImplementedException();
        }

        public bool IsBag(MemberInfo role)
        {
            throw new NotImplementedException();
        }

        public bool IsComponent(Type type)
        {
            return false;
        }

        public bool IsDictionary(MemberInfo role)
        {
            throw new NotImplementedException();
        }

        public bool IsDynamicComponent(MemberInfo member)
        {
            throw new NotImplementedException();
        }

        public bool IsEntity(Type type)
        {
            return true;
        }

        public bool IsIdBag(MemberInfo role)
        {
            throw new NotImplementedException();
        }

        public bool IsList(MemberInfo role)
        {
            throw new NotImplementedException();
        }

        public bool IsManyToAny(MemberInfo member)
        {
            throw new NotImplementedException();
        }

        public bool IsManyToMany(MemberInfo member)
        {
            throw new NotImplementedException();
        }

        public bool IsManyToOne(MemberInfo member)
        {
            //property referring other entity is considered many-to-ones...
            var propertyInfo = member as PropertyInfo;
            if (propertyInfo != null)
                return propertyInfo.PropertyType.FullName.IndexOf("MappingByCode") != -1;

            return false;
        }

        public bool IsMemberOfComposedId(MemberInfo member)
        {
            return false;
        }

        public bool IsMemberOfNaturalId(MemberInfo member)
        {
            return false;
        }

        public bool IsOneToMany(MemberInfo member)
        {
            throw new NotImplementedException();
        }

        public bool IsOneToOne(MemberInfo member)
        {
            throw new NotImplementedException();
        }

        public bool IsPersistentId(MemberInfo member)
        {
            return member.Name == "Id";
        }

        public bool IsPersistentProperty(MemberInfo member)
        {
            return member.Name != "Id";
        }

        public bool IsProperty(MemberInfo member)
        {
            if (member.Name != "Id") // property named id have to be mapped as keys...
            {
                var propertyInfo = member as PropertyInfo;
                if (propertyInfo != null)
                {
                    // just simple stading that if a property is an entity we have 
                    // a many-to-one relation type, so property is false
                    if (propertyInfo.PropertyType.FullName.IndexOf("MappingByCode") == -1)
                        return true;
                }
            }
            return false;

        }

        public bool IsRootEntity(Type type)
        {
            return type.BaseType == typeof(object);
        }

        public bool IsSet(MemberInfo role)
        {
            throw new NotImplementedException();
        }

        public bool IsTablePerClass(Type type)
        {
            return type.BaseType == null;
        }

        public bool IsTablePerClassHierarchy(Type type)
        {
            //check if root class is declared as table per Hierarchy...
            Type t = type;
            while (t.BaseType != typeof(object))
                t = t.BaseType;
            return _tablePerHierarchy.Contains(t);
        }

        public bool IsTablePerClassSplit(Type type, object splitGroupId, MemberInfo member)
        {
            Type t = type;
            while (t.BaseType != typeof(object))
                t = t.BaseType;
            return !_tablePerHierarchy.Contains(t) && !_tablePerConcreteClass.Contains(t);
        }

        public bool IsTablePerConcreteClass(Type type)
        {
            //check if root class is declared as table per Hierarchy...
            Type t = type;
            while (t.BaseType != typeof(object))
                t = t.BaseType;
            return _tablePerConcreteClass.Contains(t);
        }

        public bool IsVersion(MemberInfo member)
        {
            return false;
        }

        #endregion

        internal void TablePerHierarchy(Type type)
        {
            _tablePerHierarchy.Add(type);
        }

        internal void TablePerConcreteClass(Type type)
        {
            _tablePerConcreteClass.Add(type);
        }
    }
}
