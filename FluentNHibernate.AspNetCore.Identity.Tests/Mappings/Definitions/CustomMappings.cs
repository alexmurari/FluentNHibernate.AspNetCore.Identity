namespace FluentNHibernate.AspNetCore.Identity.Tests;

using FluentNHibernate.AspNetCore.Identity.Mappings;

internal class ApplicationUserMap : IdentityUserMapBase<ApplicationUser, string>
{
    public ApplicationUserMap() : base(t => t.GeneratedBy.TriggerIdentity().Length(32))
    {
        Map(t => t.BirthDate).Not.Nullable();
    }
}

internal class ApplicationRoleMap : IdentityRoleMapBase<ApplicationRole, string>
{
    public ApplicationRoleMap() : base(t => t.GeneratedBy.TriggerIdentity().Length(32))
    {
        Map(t => t.RoleDescription).Length(100).Nullable();
    }
}

internal class ApplicationUserLoginMap : IdentityUserLoginMapBase<ApplicationUserLogin, string>
{
    public ApplicationUserLoginMap() : base(t => t.Length(32).Not.Nullable())
    {
        Map(t => t.ProviderDescription).Length(100).Nullable();
    }
}

internal class ApplicationUserClaimMap : IdentityUserClaimMapBase<ApplicationUserClaim, string>
{
    public ApplicationUserClaimMap() : base(t => t.Length(32).Not.Nullable())
    {
        Map(t => t.ClaimDescription).Length(100).Nullable();
    }
}

internal class ApplicationRoleClaimMap : IdentityRoleClaimMapBase<ApplicationRoleClaim, string>
{
    public ApplicationRoleClaimMap() : base(t => t.GeneratedBy.TriggerIdentity().Length(32), t => t.Length(32).Not.Nullable())
    {
    }
}

internal class ApplicationUserRoleMap : IdentityUserRoleMapBase<ApplicationUserRole, string>
{
    public ApplicationUserRoleMap() : base(t => t.KeyProperty(x => x.UserId, x => x.Length(32)).KeyProperty(x => x.RoleId, x => x.Length(32)))
    {
    }
}

internal class ApplicationUserTokenMap : IdentityUserTokenMapBase<ApplicationUserToken, string>
{
    public ApplicationUserTokenMap() : base(t => t.KeyProperty(x => x.UserId, x => x.Length(32)))
    {
    }
}