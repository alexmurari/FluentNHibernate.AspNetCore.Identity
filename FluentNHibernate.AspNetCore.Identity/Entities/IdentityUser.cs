namespace FluentNHibernate.AspNetCore.Identity;

using System;

public class IdentityUser : IdentityUser<long>
{
}

public class IdentityUser<TKey> : Microsoft.AspNetCore.Identity.IdentityUser<TKey> where TKey : IEquatable<TKey>
{
    public virtual long? LockoutEndUnixTimeSeconds { get; set; }

    public override DateTimeOffset? LockoutEnd
    {
        get
        {
            if (!LockoutEndUnixTimeSeconds.HasValue)
                return null;

            var offset = DateTimeOffset.FromUnixTimeSeconds(LockoutEndUnixTimeSeconds.Value);
            return TimeZoneInfo.ConvertTime(offset, TimeZoneInfo.Local);
        }
        set => LockoutEndUnixTimeSeconds = value?.ToUnixTimeSeconds();
    }
}