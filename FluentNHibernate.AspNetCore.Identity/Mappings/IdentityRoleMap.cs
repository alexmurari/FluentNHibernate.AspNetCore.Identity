namespace FluentNHibernate.AspNetCore.Identity.Mappings;

using System;
using FluentNHibernate.Mapping;

public sealed class IdentityRoleMap : IdentityRoleMapBase<IdentityRole, long>
{
    public IdentityRoleMap() : base(t => t.GeneratedBy.Identity())
    {
    }
}

public abstract class IdentityRoleMapBase<TEntity, TKey> : ClassMap<TEntity> where TEntity : IdentityRole<TKey> where TKey : IEquatable<TKey>
{
    protected IdentityRoleMapBase(Action<IdentityPart> configureIdMap) : this("dbo", "AspNetRoles", configureIdMap)
    {
    }

    protected IdentityRoleMapBase(string tableName, Action<IdentityPart> configureIdMap)
    {
        configureIdMap(Id(t => t.Id));
        Map(t => t.Name).Length(64).Not.Nullable().Unique();
        Map(t => t.NormalizedName).Length(64).Not.Nullable().Unique();
        Map(t => t.ConcurrencyStamp).Length(36).Nullable();

        Table(tableName);
    }

    protected IdentityRoleMapBase(string schemaName, string tableName, Action<IdentityPart> configureIdMap) : this(tableName, configureIdMap)
    {
        Schema(schemaName);
    }
}