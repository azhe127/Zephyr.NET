using System.Collections.Generic;
using System.Reflection;
using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using Zephyr.Configuration;
using Zephyr.Data.NHib.Mapping.Conventions;
using Zephyr.Domain;

namespace Zephyr.Data.NHib
{
    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {
        public List<Assembly> AutoMappingAssemblies { get; set; }
        public Assembly OverrideAssembly { get; set; }
        public Assembly CoreFrameworkAssembly { get; set; }

        public AutoPersistenceModelGenerator(Assembly overrideAssembly)
        {
            AutoMappingAssemblies = new List<Assembly>();
            OverrideAssembly = overrideAssembly;
        }        

        public AutoPersistenceModel Generate()
        {
            return
                AutoMap.Assemblies(new FrameworkMappingConfiguration(), this.AutoMappingAssemblies)
                    .Conventions
                    .Setup(this.GetConventions())
                    .IgnoreBase<Entity>()
                    .IgnoreBase<DomainEntity>()
                    .UseOverridesFromAssembly(this.OverrideAssembly)
                    .UseOverridesFromAssembly(this.CoreFrameworkAssembly);
        }

        private Action<IConventionFinder> GetConventions()
        {
            return c =>
                {                    
                    c.Add<PrimaryKeyConvention>();
                    c.Add<TableNameConvention>();
                    c.Add<EntityConvention>();
                    //should be used both or none - HasMany and Reference
                    c.Add<HasManyConvention>();
                    c.Add<ReferenceConvention>();
                    
                    c.AddAssembly(this.CoreFrameworkAssembly);
                    c.AddAssembly(this.OverrideAssembly);
                };
        }
    }
}