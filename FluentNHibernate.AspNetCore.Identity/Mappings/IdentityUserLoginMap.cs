namespace FluentNHibernate.AspNetCore.Identity.Mappings;

using System;
using FluentNHibernate.Mapping;

public sealed class IdentityUserLoginMap : IdentityUserLoginMapBase<IdentityUserLogin, long>
{
    public IdentityUserLoginMap() : base(t => t.Not.Nullable())
    {
    }
}

public abstract class IdentityUserLoginMapBase<TEntity, TKey> : ClassMap<TEntity> where TEntity : IdentityUserLogin<TKey> where TKey : IEquatable<TKey>
{
    protected IdentityUserLoginMapBase(Action<PropertyPart> configureUserIdMap) : this("dbo", "AspNetUserLogins", configureUserIdMap)
    {
    }

    protected IdentityUserLoginMapBase(string tableName, Action<PropertyPart> configureUserIdMap)
    {
        CompositeId()
            .KeyProperty(t => t.LoginProvider, t => t.Length(32))
            .KeyProperty(t => t.ProviderKey, t => t.Length(32));

        Map(t => t.ProviderDisplayName).Length(32).Not.Nullable();
        configureUserIdMap(Map(t => t.UserId));

        Table(tableName);
    }

    protected IdentityUserLoginMapBase(string schemaName, string tableName, Action<PropertyPart> configureUserIdMap) : this(tableName, configureUserIdMap)
    {
        Schema(schemaName);
    }
}