namespace FluentNHibernate.AspNetCore.Identity;

using System;

public class IdentityRole : IdentityRole<long>
{
    public IdentityRole()
    {
    }

    public IdentityRole(string roleName) : base(roleName)
    {
    }
}

public class IdentityRole<TKey> : Microsoft.AspNetCore.Identity.IdentityRole<TKey> where TKey : IEquatable<TKey>
{
    public IdentityRole()
    {
    }

    public IdentityRole(string roleName) : base(roleName)
    {
    }
}