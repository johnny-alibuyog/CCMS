using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCMS.Core.Entities;
using FluentNHibernate.Mapping;

namespace CCMS.Data.EntityDefinitions
{
    public class PersonMapping : ComponentMap<Person>
    {
        public PersonMapping()
        {
            Map(x => x.FirstName);

            Map(x => x.MiddleName);

            Map(x => x.LastName);

            Map(x => x.BirthDate);

            //Property(x => x.FirstName, map =>
            //  {
            //      map.Length(50);
            //      map.NotNullable(true);
            //  });

            //Property(x => x.MiddleName, map =>
            //{
            //    map.Length(50);
            //});

            //Property(x => x.LastName, map =>
            //{
            //    map.Length(50);
            //    map.NotNullable(true);
            //});

            //Property(x => x.BirthDate, map =>
            //{
            //    map.NotNullable(true);
            //});
        }
    }
}
