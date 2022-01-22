namespace FluentNHibernate.AspNetCore.Identity;

using System;

public class IdentityUserLogin<TKey> : Microsoft.AspNetCore.Identity.IdentityUserLogin<TKey> where TKey : IEquatable<TKey>
{
    protected bool Equals(IdentityUserLogin<TKey> other) {
        return LoginProvider == other.LoginProvider && ProviderKey == other.ProviderKey;
    }

    public override bool Equals(object? obj) {
        return obj is not null && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((IdentityUserLogin<TKey>)obj));
    }

    public override int GetHashCode() {
        unchecked {
            var hashCode = LoginProvider.GetHashCode();
            hashCode = (hashCode * 397) ^ ProviderKey.GetHashCode();
            return hashCode;
        }
    }
}