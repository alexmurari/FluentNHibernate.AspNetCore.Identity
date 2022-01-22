namespace FluentNHibernate.AspNetCore.Identity.Tests;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NSubstitute;
using Xunit;

public class StoreRegistrationTests
{
    private readonly ISession _session = Substitute.For<ISession>();

    [Fact]
    public void Assert_simple_store_registration_with_roles_is_successful()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => _session);

        // Act
        serviceCollection.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddNHibernateStores();

        // Assert
        using var serviceProvider = serviceCollection.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IUserStore<ApplicationUser>>());
        Assert.NotNull(serviceProvider.GetService<IRoleStore<ApplicationRole>>());
    }

    [Fact]
    public void Assert_simple_store_registration_without_roles_is_successful()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => _session);

        // Act
        serviceCollection.AddIdentityCore<ApplicationUser>().AddNHibernateStores();

        // Assert
        using var serviceProvider = serviceCollection.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IUserStore<ApplicationUser>>());
        Assert.Null(serviceProvider.GetService<IRoleStore<ApplicationRole>>());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Assert_simple_store_registration_with_store_options_is_successful(bool autoFlushSession)
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => _session);

        // Act
        serviceCollection.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddNHibernateStores(t => t.SetAutoFlushSession(autoFlushSession));

        // Assert
        using var serviceProvider = serviceCollection.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IUserStore<ApplicationUser>>());
        Assert.NotNull(serviceProvider.GetService<IRoleStore<ApplicationRole>>());
        Assert.Equal(autoFlushSession, ((UserStore<ApplicationUser, ApplicationRole, string>)serviceProvider.GetRequiredService<IUserStore<ApplicationUser>>()).AutoFlushSession);
        Assert.Equal(autoFlushSession, ((RoleStore<ApplicationRole, string>)serviceProvider.GetRequiredService<IRoleStore<ApplicationRole>>()).AutoFlushSession);
    }

    [Fact]
    public void Assert_extended_store_registration_with_roles_and_with_default_types_is_successful()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => _session);

        // Act
        serviceCollection.AddIdentityCore<ApplicationUser>()
            .ExtendConfiguration()
            .AddRoles<ApplicationRole>()
            .AddNHibernateStores();

        // Assert
        using var serviceProvider = serviceCollection.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IUserStore<ApplicationUser>>());
        Assert.NotNull(serviceProvider.GetService<IRoleStore<ApplicationRole>>());
    }

    [Fact]
    public void Assert_extended_store_registration_without_roles_and_with_default_types_is_successful()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => _session);

        // Act
        serviceCollection.AddIdentityCore<ApplicationUser>()
            .ExtendConfiguration()
            .AddNHibernateStores();

        // Assert
        using var serviceProvider = serviceCollection.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IUserStore<ApplicationUser>>());
        Assert.Null(serviceProvider.GetService<IRoleStore<ApplicationRole>>());
    }

    [Fact]
    public void Assert_extended_store_registration_with_roles_and_with_custom_types_is_successful()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => _session);

        // Act
        serviceCollection.AddIdentityCore<ApplicationUser>()
            .ExtendConfiguration()
            .AddRoles<ApplicationRole>()
            .AddUserLogin<ApplicationUserLogin>()
            .AddRoleClaim<ApplicationRoleClaim>()
            .AddUserClaim<ApplicationUserClaim>()
            .AddUserRole<ApplicationUserRole>()
            .AddUserToken<ApplicationUserToken>()
            .AddNHibernateStores();

        // Assert
        using var serviceProvider = serviceCollection.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IUserStore<ApplicationUser>>());
        Assert.NotNull(serviceProvider.GetService<IRoleStore<ApplicationRole>>());
    }

    [Fact]
    public void Assert_extended_store_registration_without_roles_and_with_custom_types_is_successful()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => _session);

        // Act
        serviceCollection.AddIdentityCore<ApplicationUser>()
            .ExtendConfiguration()
            .AddUserLogin<ApplicationUserLogin>()
            .AddRoleClaim<ApplicationRoleClaim>()
            .AddUserClaim<ApplicationUserClaim>()
            .AddUserRole<ApplicationUserRole>()
            .AddUserToken<ApplicationUserToken>()
            .AddNHibernateStores();

        // Assert
        using var serviceProvider = serviceCollection.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IUserStore<ApplicationUser>>());
        Assert.Null(serviceProvider.GetService<IRoleStore<ApplicationRole>>());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Assert_extended_store_registration_with_store_options_is_successful(bool autoFlushSession)
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => _session);

        // Act
        serviceCollection.AddIdentityCore<ApplicationUser>()
            .ExtendConfiguration()
            .AddUserClaim<ApplicationUserClaim>()
            .AddUserToken<ApplicationUserToken>()
            .AddNHibernateStores(t => t.SetAutoFlushSession(autoFlushSession));

        // Assert
        using var serviceProvider = serviceCollection.BuildServiceProvider();
        Assert.NotNull(serviceProvider.GetService<IUserStore<ApplicationUser>>());
        Assert.Null(serviceProvider.GetService<IRoleStore<ApplicationRole>>());
        Assert.Equal(autoFlushSession, ((UserOnlyStore<ApplicationUser, string, ApplicationUserClaim, Identity.IdentityUserLogin<string>, ApplicationUserToken>)serviceProvider
            .GetRequiredService<IUserStore<ApplicationUser>>()).AutoFlushSession);
    }
}