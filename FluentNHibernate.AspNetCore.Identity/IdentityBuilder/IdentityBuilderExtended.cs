namespace FluentNHibernate.AspNetCore.Identity;

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public class IdentityBuilderExtended : IdentityBuilder
{
    public IdentityBuilderExtended(Type? user, IServiceCollection services) : base(user, services)
    {
    }

    public IdentityBuilderExtended(Type? user, Type? role, IServiceCollection services) : base(user, role, services)
    {
    }

    public Type? RoleClaimType { get; private set; }

    public Type? UserClaimType { get; private set; }

    public Type? UserLoginType { get; private set; }

    public Type? UserRoleType { get; private set; }

    public Type? UserTokenType { get; private set; }

    public IdentityBuilderExtended AddRoleClaim<TRoleClaim>() where TRoleClaim : class
    {
        RoleClaimType = typeof(TRoleClaim);
        return this;
    }

    public IdentityBuilderExtended AddUserClaim<TUserClaim>() where TUserClaim : class
    {
        UserClaimType = typeof(TUserClaim);
        return this;
    }

    public IdentityBuilderExtended AddUserLogin<TUserLogin>() where TUserLogin : class
    { 
        UserLoginType = typeof(TUserLogin);
        return this;
    }

    public IdentityBuilderExtended AddUserRole<TUserRole>() where TUserRole : class
    {
        UserRoleType = typeof(TUserRole);
        return this;
    }

    public IdentityBuilderExtended AddUserToken<TUserToken>() where TUserToken : class
    {
        UserTokenType = typeof(TUserToken);
        return this;
    }

    public new IdentityBuilderExtended AddRoles<TRole>() where TRole : class
    {
        base.AddRoles<TRole>();
        return this;
    }

    public new IdentityBuilderExtended AddUserStore<TUserStore>() where TUserStore : class
    {
        base.AddUserStore<TUserStore>();
        return this;
    }

    public new IdentityBuilderExtended AddRoleStore<TRoleStore>() where TRoleStore : class
    {
        base.AddRoleStore<TRoleStore>();
        return this;
    }

    public new IdentityBuilderExtended AddUserManager<TUser>() where TUser : class
    {
        base.AddUserManager<TUser>();
        return this;
    }

    public new IdentityBuilderExtended AddRoleManager<TRoleManager>() where TRoleManager : class
    {
        base.AddRoleManager<TRoleManager>();
        return this;
    }

    public new IdentityBuilderExtended AddPersonalDataProtection<TProtector, TKeyRing>() where TProtector : class, ILookupProtector where TKeyRing : class, ILookupProtectorKeyRing
    {
        base.AddPersonalDataProtection<TProtector, TKeyRing>();
        return this;
    }

    public new IdentityBuilderExtended AddPasswordValidator<TValidator>() where TValidator : class
    {
        base.AddPasswordValidator<TValidator>();
        return this;
    }

    public new IdentityBuilderExtended AddErrorDescriber<TDescriber>() where TDescriber : IdentityErrorDescriber
    {
        base.AddErrorDescriber<TDescriber>();
        return this;
    }

    public new IdentityBuilderExtended AddUserValidator<TValidator>() where TValidator : class
    {
        base.AddUserValidator<TValidator>();
        return this;
    }

    public new IdentityBuilderExtended AddUserConfirmation<TUserConfirmation>() where TUserConfirmation : class
    {
        base.AddUserConfirmation<TUserConfirmation>();
        return this;
    }

    public new IdentityBuilderExtended AddTokenProvider<TProvider>(string providerName) where TProvider : class
    {
        base.AddTokenProvider<TProvider>(providerName);
        return this;
    }

    public new IdentityBuilderExtended AddTokenProvider(string providerName, Type provider)
    {
        base.AddTokenProvider(providerName, provider);
        return this;
    }

    public new IdentityBuilderExtended AddClaimsPrincipalFactory<TFactory>() where TFactory : class
    {
        base.AddClaimsPrincipalFactory<TFactory>();
        return this;
    }

    public new IdentityBuilderExtended AddRoleValidator<TRole>() where TRole : class
    {
        base.AddRoleValidator<TRole>();
        return this;
    }
}