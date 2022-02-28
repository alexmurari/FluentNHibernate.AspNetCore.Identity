namespace FluentNHibernate.AspNetCore.Identity;

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class IdentityBuilderExtendedExtensions
{
    public static IdentityBuilderExtended AddNHibernateStores(this IdentityBuilderExtended identityBuilder, Action<NHibernateStoreOptions>? configureStoreOptions = null)
    {
        AddStores(identityBuilder, configureStoreOptions);
        return identityBuilder;
    }

    private static void AddStores(IdentityBuilderExtended identityBuilder, Action<NHibernateStoreOptions>? configureStoreOptions)
    {
        var userType = identityBuilder.UserType;
        var roleType = identityBuilder.RoleType;
        var services = identityBuilder.Services;

        var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));

        if (identityUserType == null)
        {
            throw new InvalidOperationException($"The type registered as IdentityUser is not convertible to {typeof(IdentityUser<>).FullName}.");
        }

        var keyType = identityUserType.GenericTypeArguments[0];

        if (roleType != null)
        {
            var identityRoleType = FindGenericBaseType(roleType, typeof(IdentityRole<>));

            if (identityRoleType == null)
            {
                throw new InvalidOperationException($"The type registered as IdentityRole is not convertible to {typeof(IdentityRole<>).FullName}.");
            }

            var userStoreType = typeof(UserStore<,,,,,,,>).MakeGenericType(
                userType,
                roleType,
                keyType,
                identityBuilder.UserClaimType ?? typeof(IdentityUserClaim<>).MakeGenericType(keyType),
                identityBuilder.UserRoleType ?? typeof(IdentityUserRole<>).MakeGenericType(keyType),
                identityBuilder.UserLoginType ?? typeof(IdentityUserLogin<>).MakeGenericType(keyType),
                identityBuilder.UserTokenType ?? typeof(IdentityUserToken<>).MakeGenericType(keyType),
                identityBuilder.RoleClaimType ?? typeof(IdentityRoleClaim<>).MakeGenericType(keyType));

            var roleStoreType = typeof(RoleStore<,,,>).MakeGenericType(
                roleType,
                keyType,
                identityBuilder.UserRoleType ?? typeof(IdentityUserRole<>).MakeGenericType(keyType),
                identityBuilder.RoleClaimType ?? typeof(IdentityRoleClaim<>).MakeGenericType(keyType));

            services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
            services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), roleStoreType);
        }
        else
        {
            var userStoreType = typeof(UserOnlyStore<,,,,>).MakeGenericType(
                userType,
                keyType,
                identityBuilder.UserClaimType ?? typeof(IdentityUserClaim<>).MakeGenericType(keyType),
                identityBuilder.UserLoginType ?? typeof(IdentityUserLogin<>).MakeGenericType(keyType),
                identityBuilder.UserTokenType ?? typeof(IdentityUserToken<>).MakeGenericType(keyType)
            );

            services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
        }

        if (configureStoreOptions != null)
        {
            var storeOptions = new NHibernateStoreOptions();
            configureStoreOptions(storeOptions);
            services.TryAddSingleton(storeOptions);
        }
    }

    private static Type? FindGenericBaseType(Type currentType, Type genericBaseType)
    {
        var type = currentType;

        while (type != null)
        {
            var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;

            if (genericType != null && genericType == genericBaseType)
            {
                return type;
            }

            type = type.BaseType;
        }

        return null;
    }
}