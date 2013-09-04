using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace CCMS.Data.Conventions
{
    public class CustomTableNameConvention : IClassConvention, IClassConventionAcceptance
    {
        private readonly PluralizationService _pluralizationService;

        public CustomTableNameConvention()
        {
            _pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));
        }

        public void Apply(IClassInstance instance)
        {
            instance.Table(_pluralizationService.Pluralize(instance.EntityType.Name));
        }

        public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
        {
            //criteria.Expect(x => x.TableName, Is.Not.Set);
        }
    }

}
