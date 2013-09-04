using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Impl;

namespace CCMS.Data.Conventions
{
    internal class AutoMapper : ModelMapper
    {
        private readonly PluralizationService _pluralizationService;


        public AutoMapper() : base(new AutoModelInspector())
        {
            _pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));

            this.BeforeMapClass += new RootClassMappingHandler(AutoMapper_BeforeMapClass);
            this.BeforeMapJoinedSubclass += new JoinedSubclassMappingHandler(AutoMapper_BeforeMapJoinedSubclass);
            this.BeforeMapUnionSubclass += new UnionSubclassMappingHandler(AutoMapper_BeforeMapUnionSubclass);
            this.BeforeMapProperty += new PropertyMappingHandler(AutoMapper_BeforeMapProperty);
            this.BeforeMapManyToOne += new ManyToOneMappingHandler(AutoMapper_BeforeMapManyToOne);
        }

        private void AutoMapper_BeforeMapClass(IModelInspector modelInspector, Type type, IClassAttributesMapper classCustomizer)
        {
            // Create the column name as EntityName + "Id" and pluralize table names
            classCustomizer.Id(x => x.Column(type.Name + "Id"));
            classCustomizer.Table(_pluralizationService.Pluralize(type.Name));
        }

        private void AutoMapper_BeforeMapJoinedSubclass(IModelInspector modelInspector, Type type, IJoinedSubclassAttributesMapper joinedSubclassCustomizer)
        {
            // pluralize table names for inheritance
            joinedSubclassCustomizer.Key(x => x.Column(type.Name + "Id"));
            joinedSubclassCustomizer.Table(_pluralizationService.Pluralize(type.Name));
        }

        private void AutoMapper_BeforeMapUnionSubclass(IModelInspector modelInspector, Type type, IUnionSubclassAttributesMapper unionSubclassCustomizer)
        {
            // pluralize table names for inheritance
            unionSubclassCustomizer.Table(_pluralizationService.Pluralize(type.Name));
        }

        private void AutoMapper_BeforeMapProperty(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
        {
            ////
            //// Treat description as a special case: "txt"+EntityName+"Descr"
            //// but for all property of type string prefix with "txt"
            ////
            //if (member.LocalMember.Name == "Description")
            //{
            //    propertyCustomizer.Column(k =>
            //        {
            //            k.Name("txt" + member.GetContainerEntity(modelInspector).Name + "Descr");
            //            k.SqlType("AnsiString");
            //        }
            //        );
            //}
            //else
            //{
            //    var pi = member.LocalMember as PropertyInfo;
                
            //    if (null != pi && pi.PropertyType == typeof(string))
            //    {
            //        propertyCustomizer.Column(k =>
            //        {
            //            k.Name("txt" + member.LocalMember.Name);
            //            k.SqlType("AnsiString");
            //        }
            //       );
            //    }
            //}
        }
       
        private void AutoMapper_BeforeMapManyToOne(IModelInspector modelInspector, PropertyPath member, IManyToOneMapper propertyCustomizer)
        {
            // name the column for many to one as ForeignEntityName + "Id"
            var propertyInfo = member.LocalMember as PropertyInfo;
            if (propertyInfo != null)
                propertyCustomizer.Column(k => k.Name(propertyInfo.PropertyType.Name + "Id"));
        }

        private void Test()
        {
            //var mapper = new ConventionModelMapper();
            //mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());

            //var pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));

            //mapper.BeforeMapClass += (modelInspector, type, classCustomizer) =>
            //{
            //    // Create the column name as EntityName + "Id" and pluralize table names
            //    classCustomizer.Id(x => x.Column(type.Name + "Id"));
            //    classCustomizer.Table(pluralizationService.Pluralize(type.Name));
            //};

            //mapper.BeforeMapJoinedSubclass += (modelInspector, type, joinedSubclassCustomizer) =>
            //{
            //    // pluralize table names for inheritance
            //    joinedSubclassCustomizer.Key(x => x.Column(type.Name + "Id"));
            //    joinedSubclassCustomizer.Table(pluralizationService.Pluralize(type.Name));
            //};

            //mapper.BeforeMapUnionSubclass += (modelInspector, type, unionSubclassCustomizer) =>
            //{
            //    // pluralize table names for inheritance
            //    unionSubclassCustomizer.Table(pluralizationService.Pluralize(type.Name));
            //};

            //mapper.BeforeMapManyToOne += (modelInspector, member, propertyCustomizer) =>
            //{
            //    // name the column for many to one as ForeignEntityName + "Id"
            //    propertyCustomizer.Column(member.LocalMember.GetPropertyOrFieldType().Name + "Id");
            //};

            //mapper.BeforeMapBag += (modelInspector, member, propertyCustomizer) =>
            //{
            //    // name the column for many to one as ForeignEntityName + "Id"
            //    propertyCustomizer.Key(x => x.Column(member.GetContainerEntity(modelInspector).Name + "Id"));
            //};

            //mapper.BeforeMapSet += (modelInspector, member, propertyCustomizer) =>
            //{
            //    // name the column for many to one as ForeignEntityName + "Id"
            //    propertyCustomizer.Key(x => x.Column(member.GetContainerEntity(modelInspector).Name + "Id"));
            //};
        }
    }
}
