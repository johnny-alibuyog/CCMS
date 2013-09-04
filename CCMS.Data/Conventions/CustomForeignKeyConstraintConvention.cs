using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace CCMS.Data.Conventions
{
    public class CustomForeignKeyConstraintConvention : IHasManyConvention, IHasManyToManyConvention, IReferenceConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            //instance.Key.ForeignKey(string.Format("FK_{0}_{1}",
            //    instance.Member.Name,
            //    instance.EntityType.Name)
            //);
        }

        public void Apply(IManyToOneInstance instance)
        {
            //instance.ForeignKey(string.Format("FK_{0}_{1}",
            //    instance.EntityType.Name,
            //    instance.Name)
            //);
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {
            //instance.Key.ForeignKey(string.Format("FK_{0}_{1}",
            //    instance.Member.Name,
            //    instance.EntityType.Name)
            //);


            ////instance.Key.ForeignKey(string.Format("FK_{0}_{1}",
            ////    instance.TableName,
            ////    instance.EntityType.Name)
            ////);

            ////instance.Key.Column(instance.EntityType.Name + "Id");
            ////instance.Relationship.Column(instance.OtherSide.EntityType.Name + "Id");
        }
    }
}
