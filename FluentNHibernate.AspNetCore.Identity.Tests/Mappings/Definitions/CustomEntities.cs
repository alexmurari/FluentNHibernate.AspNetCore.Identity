namespace FluentNHibernate.AspNetCore.Identity.Tests;

using System;

internal class ApplicationUser : IdentityUser<string>
{
    public virtual DateTime BirthDate { get; set; }
}

internal class ApplicationRole : IdentityRole<string>
{
    public virtual string? RoleDescription { get; set; }
}

internal class ApplicationUserLogin : IdentityUserLogin<string>
{
    public virtual string? ProviderDescription { get; set; }
}

internal class ApplicationUserRole : IdentityUserRole<string>
{
}

internal class ApplicationUserClaim : IdentityUserClaim<string>
{
    public virtual string? ClaimDescription { get; set; }
}

internal class ApplicationRoleClaim : IdentityRoleClaim<string>
{
}

internal class ApplicationUserToken : IdentityUserToken<string>
{
}