namespace FluentNHibernate.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NHibernate;
using NHibernate.Linq;

public class UserOnlyStore<TUser, TKey> : UserOnlyStore<TUser, TKey, IdentityUserClaim<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>> where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
{
    public UserOnlyStore(ISession session, NHibernateStoreOptions? storeOptions = null, IdentityErrorDescriber? describer = null) : base(session, storeOptions, describer)
    {
    }
}

public class UserOnlyStore<TUser, TKey, TUserClaim, TUserLogin, TUserToken> : UserStoreBase<TUser, TKey, TUserClaim, TUserLogin, TUserToken>, IProtectedUserStore<TUser>
    where TUser : IdentityUser<TKey>
    where TKey : IEquatable<TKey>
    where TUserClaim : IdentityUserClaim<TKey>, new()
    where TUserLogin : IdentityUserLogin<TKey>, new()
    where TUserToken : IdentityUserToken<TKey>, new()
{
    private readonly ISession _session;

    private IQueryable<TUser> UsersSet => _session.Query<TUser>();
    private IQueryable<TUserClaim> UserClaims => _session.Query<TUserClaim>();
    private IQueryable<TUserLogin> UserLogins => _session.Query<TUserLogin>();
    private IQueryable<TUserToken> UserTokens => _session.Query<TUserToken>();

    public override IQueryable<TUser> Users => UsersSet;

    public bool AutoFlushSession { get; set; }

    public UserOnlyStore(ISession session, NHibernateStoreOptions? storeOptions = null, IdentityErrorDescriber? describer = null) : base(describer ?? new IdentityErrorDescriber())
    {
        _session = session;
        AutoFlushSession = storeOptions?.AutoFlushSession ?? true;
    }

    public override async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        await _session.SaveAsync(user, cancellationToken);
        await FlushSessionAsync(cancellationToken);

        return IdentityResult.Success;
    }

    public override async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var exists = await Users.AnyAsync(u => u.Id.Equals(user.Id), cancellationToken);

        if (!exists)
        {
            return IdentityResult.Failed(
                new IdentityError
                {
                    Code = "UserNotExist",
                    Description = $"User with id {user.Id} does not exists!"
                }
            );
        }

        user.ConcurrencyStamp = Guid.NewGuid().ToString();

        await _session.MergeAsync(user, cancellationToken);
        await FlushSessionAsync(cancellationToken);

        return IdentityResult.Success;
    }

    public override async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        await _session.DeleteAsync(user, cancellationToken);
        await FlushSessionAsync(cancellationToken);

        return IdentityResult.Success;
    }

    public override async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        var id = ConvertIdFromString(userId);
        var user = await _session.GetAsync<TUser>(id, cancellationToken);

        return user;
    }

    public override async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        var user = await Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);

        return user;
    }

    protected override async Task<TUser> FindUserAsync(TKey userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        var user = await _session.GetAsync<TUser>(userId, cancellationToken);

        return user;
    }

    protected override async Task<TUserLogin> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        var userLogin = await UserLogins.FirstOrDefaultAsync(ul => ul.UserId.Equals(userId) && ul.LoginProvider == loginProvider && ul.ProviderKey == providerKey, cancellationToken);

        return userLogin;
    }

    protected override async Task<TUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        var userLogin = await UserLogins.FirstOrDefaultAsync(ul => ul.LoginProvider == loginProvider && ul.ProviderKey == providerKey, cancellationToken);

        return userLogin;
    }

    public override async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var claims = await UserClaims.Where(uc => uc.UserId.Equals(user.Id))
            .Select(c => c.ToClaim())
            .ToListAsync(cancellationToken);

        return claims;
    }

    public override async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (claims == null)
        {
            throw new ArgumentNullException(nameof(claims));
        }

        foreach (var claim in claims)
        {
            await _session.SaveAsync(CreateUserClaim(user, claim), cancellationToken);
        }

        await FlushSessionAsync(cancellationToken);
    }

    public override async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }

        if (newClaim == null)
        {
            throw new ArgumentNullException(nameof(newClaim));
        }

        var matchedClaims = await UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type)
            .ToListAsync(cancellationToken);

        foreach (var matchedClaim in matchedClaims)
        {
            matchedClaim.ClaimType = newClaim.Type;
            matchedClaim.ClaimValue = newClaim.Value;
            await _session.UpdateAsync(matchedClaim, cancellationToken);
        }

        await FlushSessionAsync(cancellationToken);
    }

    public override async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (claims == null)
        {
            throw new ArgumentNullException(nameof(claims));
        }

        foreach (var claim in claims)
        {
            var matchedClaims =
                await UserClaims
                    .Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type)
                    .ToListAsync(cancellationToken);

            foreach (var matchedClaim in matchedClaims)
            {
                await _session.DeleteAsync(matchedClaim, cancellationToken);
            }
        }

        await FlushSessionAsync(cancellationToken);
    }

    public override async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }

        var query = from userClaim in UserClaims
            join user in Users on userClaim.UserId equals user.Id
            where userClaim.ClaimValue == claim.Value && userClaim.ClaimType == claim.Type
            select user;

        return await query.ToListAsync(cancellationToken);
    }

    protected override async Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var token = await UserTokens.FirstOrDefaultAsync(ut => ut.UserId.Equals(user.Id) && ut.LoginProvider == loginProvider && ut.Name == name, cancellationToken);

        return token;
    }

    protected override async Task AddUserTokenAsync(TUserToken token)
    {
        ThrowIfDisposed();

        if (token == null)
        {
            throw new ArgumentNullException(nameof(token));
        }

        await _session.SaveAsync(token);
        await FlushSessionAsync();
    }

    protected override async Task RemoveUserTokenAsync(TUserToken token)
    {
        ThrowIfDisposed();

        if (token == null)
        {
            throw new ArgumentNullException(nameof(token));
        }

        await _session.DeleteAsync(token);
        await FlushSessionAsync();
    }

    public override async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (login == null)
        {
            throw new ArgumentNullException(nameof(login));
        }

        await _session.SaveAsync(CreateUserLogin(user, login), cancellationToken);
        await FlushSessionAsync(cancellationToken);
    }

    public override async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var login = await FindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);

        await _session.DeleteAsync(login, cancellationToken);
    }

    public override async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var userId = user.Id;
        var logins = await UserLogins.Where(l => l.UserId.Equals(userId))
            .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName))
            .ToListAsync(cancellationToken);

        return logins;
    }

    public override async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        ThrowIfDisposed();

        return await Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
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