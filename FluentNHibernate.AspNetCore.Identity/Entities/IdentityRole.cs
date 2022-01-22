namespace FluentNHibernate.AspNetCore.Identity;

using System;

public class IdentityRole : IdentityRole<long>
{
}

public class IdentityRole<TKey> : Microsoft.AspNetCore.Identity.IdentityRole<TKey> where TKey : IEquatable<TKey>
{
}