namespace FluentNHibernate.AspNetCore.Identity.Tests;

using NHibernate;
using Xunit;

public class CustomMappingTests : IClassFixture<CustomIdentityMapDatabaseFixture>
{
    private readonly ISessionFactory _sessionFactory;

    public CustomMappingTests(CustomIdentityMapDatabaseFixture databaseFixture)
    {
        _sessionFactory = databaseFixture.SessionFactory;
    }

    [Fact]
    public void Assert_custom_mapping_from_IdentityUserMapBase_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        // Act
        var exception = Record.Exception(() => session.Get<ApplicationUser>("1"));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_custom_mapping_from_IdentityRoleMapBase_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        // Act
        var exception = Record.Exception(() => session.Get<ApplicationRole>("1"));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_custom_mapping_from_IdentityUserLoginMapBase_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        // Act
        var exception = Record.Exception(() => session.Get<ApplicationUserLogin>(new ApplicationUserLogin { LoginProvider = "Foo", ProviderKey = "1" }));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_custom_mapping_from_IdentityUserClaimMapBase_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        // Act
        var exception = Record.Exception(() => session.Get<ApplicationUserClaim>(1));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_custom_mapping_from_IdentityRoleClaimMapBase_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        // Act
        var exception = Record.Exception(() => session.Get<ApplicationRoleClaim>(1));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_custom_mapping_from_IdentityUserRoleMapBase_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        // Act
        var exception = Record.Exception(() => session.Get<ApplicationUserRole>(new ApplicationUserRole { UserId = "1", RoleId = "1" }));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_custom_mapping_from_IdentityUserTokenMapBase_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        // Act
        var exception = Record.Exception(() => session.Get<ApplicationUserToken>(new ApplicationUserToken { UserId = "1", LoginProvider = "Foo", Name = "Bar"}));

        // Assert
        Assert.Null(exception);
    }
}