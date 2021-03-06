﻿using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Zephyr.Data.NHib.Mapping.Conventions
{
    internal class TableNameConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table(instance.EntityType.Name.InflectTo().Pluralized);
        }
    }
}