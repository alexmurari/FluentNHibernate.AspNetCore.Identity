namespace FluentNHibernate.AspNetCore.Identity;

using System;
using System.Collections.Generic;

public class IdentityUserToken : IdentityUserToken<long>
{
}

public class IdentityUserToken<TKey> : Microsoft.AspNetCore.Identity.IdentityUserToken<TKey> where TKey : IEquatable<TKey>
{
    protected bool Equals(IdentityUserToken<TKey> other) {
        return EqualityComparer<TKey>.Default.Equals(UserId, other.UserId)
               && LoginProvider == other.LoginProvider
               && Name == other.Name;
    }

    public override bool Equals(object? obj) {
        return obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((IdentityUserToken<TKey>)obj));
    }

    public override int GetHashCode() {
        unchecked {
            var hashCode = UserId.GetHashCode();
            hashCode = (hashCode * 397) ^ LoginProvider.GetHashCode();
            hashCode = (hashCode * 397) ^ Name.GetHashCode();
            return hashCode;
        }
    }
}
