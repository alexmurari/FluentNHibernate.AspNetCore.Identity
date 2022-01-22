namespace FluentNHibernate.AspNetCore.Identity;

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class IdentityBuilderExtensions
{
    public static IdentityBuilder AddNHibernateStores(this IdentityBuilder identityBuilder, Func<NHibernateStoreOptions, NHibernateStoreOptions>? configureStoreOptions = null)
    {
        AddStores(identityBuilder.Services, identityBuilder.UserType, identityBuilder.RoleType, configureStoreOptions);
        return identityBuilder;
    }

    public static IdentityBuilderExtended ExtendConfiguration(this IdentityBuilder identityBuilder)
    {
        return new IdentityBuilderExtended(identityBuilder.UserType, identityBuilder.RoleType, identityBuilder.Services);
    }

    private static void AddStores(IServiceCollection services, Type userType, Type? roleType, Func<NHibernateStoreOptions, NHibernateStoreOptions>? configureStoreOptions)
    {
        var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));

        if (identityUserType == null)
        {
            throw new InvalidOperationException($"The type registered as IdentityUser is not convertible to {typeof(IdentityUser<>).FullName}.");
        }

        var keyType = identityUserType.GenericTypeArguments[0];

        var userStoreServiceType = typeof(IUserStore<>).MakeGenericType(userType);

        if (roleType != null)
        {
            var userStoreImplType = typeof(UserStore<,,>).MakeGenericType(userType, roleType, keyType);
            services.TryAddScoped(userStoreServiceType, userStoreImplType);

            var roleStoreSvcType = typeof(IRoleStore<>).MakeGenericType(roleType);
            var roleStoreImplType = typeof(RoleStore<,>).MakeGenericType(roleType, keyType);
            services.TryAddScoped(roleStoreSvcType, roleStoreImplType);
        }
        else
        {
            var userStoreImplType = typeof(UserOnlyStore<,>).MakeGenericType(userType, keyType);
            services.TryAddScoped(userStoreServiceType, userStoreImplType);
        }

        if (configureStoreOptions != null)
        {
            services.TryAddSingleton(configureStoreOptions(new NHibernateStoreOptions()));
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