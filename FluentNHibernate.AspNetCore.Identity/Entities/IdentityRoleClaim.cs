namespace FluentNHibernate.AspNetCore.Identity;

using System;

public class IdentityRoleClaim<TKey> : Microsoft.AspNetCore.Identity.IdentityRoleClaim<TKey> where TKey : IEquatable<TKey>
{
}