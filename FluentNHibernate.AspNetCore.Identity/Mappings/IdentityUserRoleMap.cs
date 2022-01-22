namespace FluentNHibernate.AspNetCore.Identity.Mappings;

using FluentNHibernate.Mapping;
using System;

public sealed class IdentityUserRoleMap : IdentityUserRoleMapBase<IdentityUserRole<long>, long>
{
    public IdentityUserRoleMap() : base(t => t.KeyProperty(x => x.UserId).KeyProperty(x => x.RoleId))
    {
    }
}

public abstract class IdentityUserRoleMapBase<TEntity, TKey> : ClassMap<TEntity> where TEntity : IdentityUserRole<TKey> where TKey : IEquatable<TKey>
{
    protected IdentityUserRoleMapBase(Action<CompositeIdentityPart<TEntity>> configureCompositeId) : this("dbo", "AspNetUserRoles", configureCompositeId)
    {
    }

    protected IdentityUserRoleMapBase(string tableName, Action<CompositeIdentityPart<TEntity>> configureCompositeId)
    {
        if (configureCompositeId == null)
        {
            throw new ArgumentNullException(nameof(configureCompositeId));
        }

        configureCompositeId(CompositeId());
        Table(tableName);
    }

    protected IdentityUserRoleMapBase(string schemaName, string tableName, Action<CompositeIdentityPart<TEntity>> configureCompositeId) : this(tableName, configureCompositeId)
    {
        Schema(schemaName);
    }
}