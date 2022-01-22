namespace FluentNHibernate.AspNetCore.Identity.Mappings;

using System;
using FluentNHibernate.Mapping;

public sealed class IdentityUserMap : IdentityUserMapBase<IdentityUser, long>
{
    public IdentityUserMap() : base(t => t.GeneratedBy.Identity())
    {
    }
}

public abstract class IdentityUserMapBase<TEntity, TKey> : ClassMap<TEntity> where TEntity : IdentityUser<TKey> where TKey : IEquatable<TKey>
{
    protected IdentityUserMapBase(Action<IdentityPart> configureUserIdMap) : this("dbo", "AspNetUsers", configureUserIdMap)
    {
    }

    protected IdentityUserMapBase(string tableName, Action<IdentityPart> configureIdMap)
    {
        configureIdMap(Id(t => t.Id));
        Map(t => t.UserName).Unique().Not.Nullable().Length(64);
        Map(t => t.NormalizedUserName).Unique().Not.Nullable().Length(64);
        Map(t => t.Email).Not.Nullable().Length(256);
        Map(t => t.NormalizedEmail).Not.Nullable().Length(256);
        Map(t => t.EmailConfirmed).Not.Nullable();
        Map(t => t.PasswordHash).Nullable();
        Map(t => t.SecurityStamp).Nullable();
        Map(t => t.ConcurrencyStamp).Nullable();
        Map(t => t.PhoneNumber).Nullable().Length(128);
        Map(t => t.PhoneNumberConfirmed).Not.Nullable();
        Map(t => t.TwoFactorEnabled).Not.Nullable();
        Map(t => t.LockoutEndUnixTimeSeconds).Nullable();
        Map(t => t.LockoutEnabled).Not.Nullable();
        Map(t => t.AccessFailedCount).Not.Nullable();

        Table(tableName);
    }

    protected IdentityUserMapBase(string schemaName, string tableName, Action<IdentityPart> configureUserIdMap) : this(tableName, configureUserIdMap)
    {
        Schema(schemaName);
    }
}