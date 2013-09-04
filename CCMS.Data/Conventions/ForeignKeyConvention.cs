using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;

namespace CCMS.Data.Conventions
{
    public static class ForeignKeyConvention
    {
        public static void ApplyForeignKeyConvention(this ModelMapper mapper)
        {
            mapper.BeforeMapManyToOne += (inspector, member, map) =>
            {
                map.Column(member.LocalMember.GetPropertyOrFieldType().Name + "Id");
            };

            mapper.BeforeMapBag += (inspector, member, map) =>
            {
                map.Key(x => x.Column(member.GetContainerEntity(inspector).Name + "Id"));
            };

            mapper.BeforeMapSet += (inspector, member, map) =>
            {
                map.Key(x => x.Column(member.GetContainerEntity(inspector).Name + "Id"));
            };
        }
    }
}
