namespace FluentNHibernate.AspNetCore.Identity;

using System;

public class IdentityUserClaim : IdentityUserClaim<long>
{
}

public class IdentityUserClaim<TKey> : Microsoft.AspNetCore.Identity.IdentityUserClaim<TKey> where TKey : IEquatable<TKey>
{
}