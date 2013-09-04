using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Domain.Models;
using FluentNHibernate.Mapping;

namespace CCMS.Data.Mappings
{
    public class PaymentModeMapping : ClassMap<PaymentMode>
    {
        public PaymentModeMapping()
        {
            Id(x => x.Id);

            Map(x => x.Name);

            //Id(x => x.Id, map =>
            //{
            //    map.Length(2);
            //    map.Access(Accessor.ReadOnly);
            //    map.Generator(Generators.Assigned);
            //});

            //Property(x => x.Name, map =>
            //{
            //    map.Length(150);
            //    map.Unique(true);
            //    map.NotNullable(true);
            //});
        }
    }
}
