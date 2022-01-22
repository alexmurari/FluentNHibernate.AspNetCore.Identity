namespace FluentNHibernate.AspNetCore.Identity;

using System;
using System.Collections.Generic;

public class IdentityUserRole<TKey> : Microsoft.AspNetCore.Identity.IdentityUserRole<TKey> where TKey : IEquatable<TKey>
{
    protected bool Equals(IdentityUserRole<TKey> other) {
        return EqualityComparer<TKey>.Default.Equals(RoleId, other.RoleId)
            && EqualityComparer<TKey>.Default.Equals(UserId, other.UserId);
    }

    public override bool Equals(object? obj) {
        return obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((IdentityUserRole<TKey>)obj));
    }

    public override int GetHashCode() {
        unchecked {
            var hashCode = RoleId.GetHashCode();
            hashCode = (hashCode * 397) ^ UserId.GetHashCode();
            return hashCode;
        }
    }

}
