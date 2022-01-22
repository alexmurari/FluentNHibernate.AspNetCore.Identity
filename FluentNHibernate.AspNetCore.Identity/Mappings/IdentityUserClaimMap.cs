namespace FluentNHibernate.AspNetCore.Identity.Mappings;

using System;
using FluentNHibernate.Mapping;

public sealed class IdentityUserClaimMap : IdentityUserClaimMapBase<IdentityUserClaim, long>
{
    public IdentityUserClaimMap() : base(t => t.Not.Nullable())
    {
    }
}

public abstract class IdentityUserClaimMapBase<TEntity, TKey> : ClassMap<TEntity> where TEntity : IdentityUserClaim<TKey> where TKey : IEquatable<TKey>
{
    protected IdentityUserClaimMapBase(Action<PropertyPart> configureUserIdMap) : this("dbo", "AspNetUserClaims", configureUserIdMap)
    {
    }

    protected IdentityUserClaimMapBase(string tableName, Action<PropertyPart> configureUserIdMap)
    {
        Id(t => t.Id).GeneratedBy.Identity();
        configureUserIdMap(Map(t => t.UserId));
        Map(t => t.ClaimType).Length(1024).Not.Nullable();
        Map(t => t.ClaimValue).Length(1024).Not.Nullable();

        Table(tableName);
    }

    protected IdentityUserClaimMapBase(string schemaName, string tableName, Action<PropertyPart> configureUserIdMap) : this(tableName, configureUserIdMap)
    {
        Schema(schemaName);
    }
}