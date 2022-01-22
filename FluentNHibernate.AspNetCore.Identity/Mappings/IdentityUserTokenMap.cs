namespace FluentNHibernate.AspNetCore.Identity.Mappings;

using System;
using FluentNHibernate.Mapping;

public sealed class IdentityUserTokenMap : IdentityUserTokenMapBase<IdentityUserToken, long>
{
    public IdentityUserTokenMap() : base(t => t.KeyProperty(x => x.UserId))
    {
    }
}

public abstract class IdentityUserTokenMapBase<TEntity, TKey> : ClassMap<TEntity> where TEntity : IdentityUserToken<TKey> where TKey : IEquatable<TKey>
{
    protected IdentityUserTokenMapBase(Func<CompositeIdentityPart<TEntity>, CompositeIdentityPart<TEntity>> configureCompositeId) : this("dbo", "AspNetUserTokens", configureCompositeId)
    {
    }

    protected IdentityUserTokenMapBase(string tableName, Func<CompositeIdentityPart<TEntity>, CompositeIdentityPart<TEntity>> configureCompositeId)
    {
        configureCompositeId(CompositeId()
            .KeyProperty(t => t.LoginProvider, t => t.Length(32))
            .KeyProperty(t => t.Name, t => t.Length(32)));

        Map(t => t.Value).Length(256).Not.Nullable();

        Table(tableName);
    }

    protected IdentityUserTokenMapBase(string schemaName, string tableName, Func<CompositeIdentityPart<TEntity>, CompositeIdentityPart<TEntity>> configureCompositeId) : this(tableName, configureCompositeId)
    {
        Schema(schemaName);
    }
}