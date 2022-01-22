namespace FluentNHibernate.AspNetCore.Identity.Tests;

using System;
using System.Reflection;
using FluentNHibernate.Cfg.Db;
using NHibernate;

public class CustomIdentityMapDatabaseFixture : IDisposable
{
    private bool _disposedValue;

    public CustomIdentityMapDatabaseFixture()
    {
        var databaseConfiguration = new DatabaseConfiguration(SQLiteConfiguration.Standard.ConnectionString("DataSource=file:fnh-aspnet-identity?mode=memory&cache=shared")
            .DefaultSchema("custom-map-tests")
            .ShowSql()
            .FormatSql(), t => t.AddFromAssembly(Assembly.GetExecutingAssembly()));

        SessionFactory = databaseConfiguration.GetSessionFactory();

        databaseConfiguration.DropDatabase();
        databaseConfiguration.CreateOrUpdateDatabase();
    }

    public ISessionFactory SessionFactory { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue) return;

        if (disposing)
        {
            SessionFactory.Dispose();
        }

        _disposedValue = true;
    }
}

public class DefaultIdentityMapDatabaseFixture : IDisposable
{
    private bool _disposedValue;

    public DefaultIdentityMapDatabaseFixture()
    {
        var databaseConfiguration = new DatabaseConfiguration(SQLiteConfiguration.Standard.ConnectionString("DataSource=file:fnh-aspnet-identity?mode=memory&cache=shared")
            .DefaultSchema("default-map-tests")
            .ShowSql()
            .FormatSql(), t => t.AddIdentityMappings());

        SessionFactory = databaseConfiguration.GetSessionFactory();

        databaseConfiguration.DropDatabase();
        databaseConfiguration.CreateOrUpdateDatabase();
    }

    public ISessionFactory SessionFactory { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue) return;

        if (disposing)
        {
            SessionFactory.Dispose();
        }

        _disposedValue = true;
    }
}