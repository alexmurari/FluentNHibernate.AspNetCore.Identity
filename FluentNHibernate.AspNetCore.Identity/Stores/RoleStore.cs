namespace FluentNHibernate.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

public class RoleStore<TRole, TKey> : RoleStore<TRole, TKey, IdentityUserRole<TKey>, IdentityRoleClaim<TKey>> 
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    public RoleStore(ISession session, NHibernateStoreOptions? storeOptions = null, IdentityErrorDescriber? describer = null) : base(session, storeOptions, describer)
    {
    }
}

public class RoleStore<TRole, TKey, TUserRole, TRoleClaim> : RoleStoreBase<TRole, TKey, TUserRole, TRoleClaim>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
    where TUserRole : IdentityUserRole<TKey>, new()
    where TRoleClaim : IdentityRoleClaim<TKey>, new()
{
    private readonly ISession _session;

    private IQueryable<TRoleClaim> RoleClaims => _session.Query<TRoleClaim>();

    public bool AutoFlushSession { get; set; }

    public override IQueryable<TRole> Roles => _session.Query<TRole>();

    public RoleStore(ISession session, NHibernateStoreOptions? storeOptions = null, IdentityErrorDescriber? describer = null) : base(describer ?? new IdentityErrorDescriber())
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
        AutoFlushSession = storeOptions?.AutoFlushSession ?? true;
    }

    public override async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        await _session.SaveAsync(role, cancellationToken);
        await FlushSessionAsync(cancellationToken);

        return IdentityResult.Success;
    }

    public override async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        var exists = await Roles.AnyAsync(r => r.Id.Equals(role.Id), cancellationToken);

        if (!exists)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "RoleNotExist",
                Description = $"Role with {role.Id} does not exists."
            });
        }

        role.ConcurrencyStamp = Guid.NewGuid().ToString();

        await _session.MergeAsync(role, cancellationToken);
        await FlushSessionAsync(cancellationToken);

        return IdentityResult.Success;
    }

    public override async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        await _session.DeleteAsync(role, cancellationToken);
        await FlushSessionAsync(cancellationToken);

        return IdentityResult.Success;
    }

    public override Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        return Task.FromResult(role.Id.ToString());
    }

    public override Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        return Task.FromResult(role.Name);
    }

    public override async Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        var role = await _session.GetAsync<TRole>(id, cancellationToken);

        return role;
    }

    public override async Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        var role = await Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, cancellationToken);

        return role;
    }

    public override async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        var claims = await RoleClaims
            .Where(rc => rc.RoleId.Equals(role.Id))
            .Select(c => new Claim(c.ClaimType, c.ClaimValue))
            .ToListAsync(cancellationToken);

        return claims;
    }

    public override async Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }

        var roleClaim = CreateRoleClaim(role, claim);

        await _session.SaveAsync(roleClaim, cancellationToken);
        await FlushSessionAsync(cancellationToken);
    }

    public override async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }

        var claims = await RoleClaims
            .Where(rc => rc.RoleId.Equals(role.Id) && rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type)
            .ToListAsync(cancellationToken);

        foreach (var c in claims)
        {
            await _session.DeleteAsync(c, cancellationToken);
        }

        await FlushSessionAsync(cancellationToken);
    }

    private async Task FlushSessionAsync(CancellationToken cancellationToken = default)
    {
        if (AutoFlushSession)
        {
            await _session.FlushAsync(cancellationToken);
            _session.Clear();
        }
    }
}