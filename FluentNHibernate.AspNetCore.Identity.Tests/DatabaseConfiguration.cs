[assembly: Xunit.CollectionBehavior(DisableTestParallelization = true)]

namespace FluentNHibernate.AspNetCore.Identity.Tests;

using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

internal class DatabaseConfiguration
{
    private readonly ISessionFactory _sessionFactory;

    private SchemaExport? _schemaExport;

    private SchemaUpdate? _schemaUpdate;

    public DatabaseConfiguration(IPersistenceConfigurer persistenceConfigurer, Action<FluentMappingsContainer> configureMapping)
    {
        _sessionFactory = BuildSessionFactory(persistenceConfigurer, configureMapping);
    }

    public ISessionFactory GetSessionFactory() => _sessionFactory;

    public void CreateOrUpdateDatabase() => _schemaUpdate?.Execute(true, true);

    public void DropDatabase() => _schemaExport?.Drop(true, true);

    private ISessionFactory BuildSessionFactory(IPersistenceConfigurer persistenceConfigurer, Action<FluentMappingsContainer> configureMapping)
    {
        var configuration = Fluently.Configure()
            .Database(persistenceConfigurer)
            .Mappings(t =>
            {
                configureMapping(t.FluentMappings);
            })
            .ExposeConfiguration(cfg =>
            {
                _schemaExport = new SchemaExport(cfg);
                _schemaUpdate = new SchemaUpdate(cfg);
            });

        return configuration.BuildSessionFactory();
    }
}