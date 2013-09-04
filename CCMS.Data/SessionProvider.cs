using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CCMS.Core.Entities;
using CCMS.Data.Configurations;
using CCMS.Data.Conventions;
using CCMS.Data.EntityDefinitions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Validator.Engine;

namespace CCMS.Data
{
    public class SessionProvider : ISessionProvider
    {
        private AuditResolver _auditResolver;
        private readonly ISessionFactory _sessionFactory;

        //private static readonly object _locker = new object();
        private static ISessionProvider _instance = new SessionProvider();

        public static ISessionProvider Instance
        {
            get 
            {
                //lock (_locker)
                //{
                //    if (_instance == null)
                //        _instance = new SessionProvider();
                //}

                return _instance; 
            }
        }

        public virtual ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }

        public virtual ValidatorEngine ValidatorEngine
        {
            get { return ValidatorConfiguration.ValidatorEngine; }
        }

        public virtual AuditResolver AuditResolver
        {
            get { return _auditResolver; }
            set { _auditResolver = value; }
        }

        public virtual ISession GetSharedSession()
        {
            var session = (ISession)null;
            var sessionFactory = SessionFactory;
            if (CurrentSessionContext.HasBind(sessionFactory))
            {
                session = sessionFactory.GetCurrentSession();
            }
            else
            {
                session = sessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
            }
            return session;
        }

        public virtual ISession ReleaseSharedSession()
        {
            return CurrentSessionContext.Unbind(SessionFactory);
        }

        private ISessionFactory CreateSessionFactory()
        {
            //var config = new Configuration();
            //config.DataBaseIntegration(database =>
            //{
            //    database.Dialect<MsSql2008Dialect>();
            //    database.ConnectionStringName = "ConnectionString";
            //    database.SchemaAction = SchemaAutoAction.Recreate; //SchemaAutoAction.Recreate;
            //    database.BatchSize = 15;
            //    database.LogSqlInConsole = true;
            //    database.LogFormattedSql = true;
            //});

            //// convention
            //var mapper = new ConventionModelMapper();
            //mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
            //mapper.ApplyTableNamingConvention();
            //mapper.ApplyPrimaryKeyConvention();
            //mapper.ApplyForeignKeyConvention();

            //// configuration
            //var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            //config.AddDeserializedMapping(mapping, "DomainMappings");
            //config.ConfigureAudit();
            //config.ConfigureCache();
            //config.ConfigureValidator();
            //config.ConfigureSchema();

            //return config.BuildSessionFactory();

            var schemaExportPath = Path.Combine(System.Environment.CurrentDirectory, "Mappings");

            if (!Directory.Exists(schemaExportPath))
                Directory.CreateDirectory(schemaExportPath);

            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                    .DefaultSchema("dbo")
                    .ConnectionString(x => x.FromConnectionStringWithKey("ConnectionString"))
                    .QuerySubstitutions("true 1, false 0, yes y, no n")
                    .AdoNetBatchSize(15)
                    .FormatSql()
                    //.ShowSql()
                )
                .Mappings(x => x
                    .FluentMappings.AddFromAssemblyOf<AuditMapping>()
                    .Conventions.AddFromAssemblyOf<_CustomJoinedSubclassConvention>()
                    .ExportTo(schemaExportPath)
                        //.Add(DefaultAccess.CamelCaseField(CamelCasePrefix.Underscore))
                        //.AddFromAssemblyOf<_CustomJoinedSubclassConvention>()
                    //.ExportTo(schemaExportPath)
                )
                .ProxyFactoryFactory<DefaultProxyFactoryFactory>()
                .ExposeConfiguration(EventListenerConfiguration.Configure)
                .ExposeConfiguration(CacheConfiguration.Configure)
                .ExposeConfiguration(ValidatorConfiguration.Configure)
                .ExposeConfiguration(IndexForeignKeyConfiguration.Configure)
                .ExposeConfiguration(SchemaConfiguration.Configure)
                .ExposeConfiguration(SessionContextConfiguration.Configure)
                .BuildSessionFactory();
        }

        public SessionProvider()
        {
            _sessionFactory = CreateSessionFactory();
        }
    }
}
