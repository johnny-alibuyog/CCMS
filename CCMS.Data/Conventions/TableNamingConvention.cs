using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;

namespace CCMS.Data.Conventions
{
    internal static class TableNamingConvention
    {
        private static readonly PluralizationService _pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));

        public static void ApplyTableNamingConvention(this ModelMapper mapper)
        {

            mapper.BeforeMapClass += (inspector, type, map) =>
            {
                map.Table(_pluralizationService.Pluralize(type.Name));
            };

            mapper.BeforeMapJoinedSubclass += (inspector, type, map) =>
            {
                map.Table(_pluralizationService.Pluralize(type.Name));
            };

            mapper.BeforeMapUnionSubclass += (inspector, type, map) =>
            {
                map.Table(_pluralizationService.Pluralize(type.Name));
            };
        }
    }
}
