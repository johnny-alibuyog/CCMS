using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;

namespace CCMS.Data.Conventions
{
    public static class PrimaryKeyConvention
    {
        public static void ApplyPrimaryKeyConvention(this ModelMapper mapper)
        {
            mapper.BeforeMapClass += (inspector, type, map) => 
            {
                map.Id(x => x.Column(type.Name + "Id"));
            };

            mapper.BeforeMapJoinedSubclass += (inspector, type, map) =>
            {
                map.Key(x => x.Column(type.Name + "Id"));
            };
        }
    }
}
