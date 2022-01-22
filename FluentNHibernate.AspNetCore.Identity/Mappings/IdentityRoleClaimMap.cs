namespace FluentNHibernate.AspNetCore.Identity.Mappings;

using System;
using FluentNHibernate.Mapping;

public sealed class IdentityRoleClaimMap : IdentityRoleClaimMapBase<IdentityRoleClaim, long>
{
    public IdentityRoleClaimMap() : base(t => t.GeneratedBy.Identity(), t => t.Not.Nullable())
    {
    }
}

public abstract class IdentityRoleClaimMapBase<TEntity, TKey> : ClassMap<TEntity> where TEntity : IdentityRoleClaim<TKey> where TKey : IEquatable<TKey>
{
    protected IdentityRoleClaimMapBase(Action<IdentityPart> configureIdMap, Action<PropertyPart> configureRoleIdMap) : this("dbo", "AspNetRoleClaims", configureIdMap, configureRoleIdMap)
    {
    }

    protected IdentityRoleClaimMapBase(string tableName, Action<IdentityPart> configureIdMap, Action<PropertyPart> configureRoleIdMap)
    {
        configureIdMap(Id(t => t.Id));
        configureRoleIdMap(Map(t => t.RoleId));
        Map(t => t.ClaimType).Length(1024).Not.Nullable();
        Map(t => t.ClaimValue).Length(1024).Not.Nullable();

        Table(tableName);
    }

    protected IdentityRoleClaimMapBase(string schemaName, string tableName, Action<IdentityPart> configureIdMap, Action<PropertyPart> configureRoleIdMap) : this(tableName, configureIdMap, configureRoleIdMap)
    {
        Schema(schemaName);
    }
}