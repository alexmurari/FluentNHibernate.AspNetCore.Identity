namespace FluentNHibernate.AspNetCore.Identity.Tests;

using NHibernate;
using Xunit;

public class DefaultMappingsTests : IClassFixture<DefaultIdentityMapDatabaseFixture>
{
    private readonly ISessionFactory _sessionFactory;

    public DefaultMappingsTests(DefaultIdentityMapDatabaseFixture databaseFixture)
    {
        _sessionFactory = databaseFixture.SessionFactory;
    }

    [Fact]
    public void Assert_IdentityUser_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        // Act
        var exception = Record.Exception(() => session.Get<IdentityUser>(1L));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_IdentityRole_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        // Act
        var exception = Record.Exception(() => session.Get<IdentityRole>(1L));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_IdentityRoleClaim_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        // Act
        var exception = Record.Exception(() => session.Get<IdentityRoleClaim<long>>(1));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_IdentityUserClaim_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();

        // Act
        var exception = Record.Exception(() => session.Get<IdentityUserClaim<long>>(1));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_IdentityUserLogin_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        var id = new IdentityUserLogin<long> { LoginProvider = "Foo", ProviderKey = "Bar" };

        // Act
        var exception = Record.Exception(() => session.Get<IdentityUserLogin<long>>(id));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_IdentityUserRole_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        var id = new IdentityUserRole<long> { RoleId = 1, UserId = 2 };

        // Act
        var exception = Record.Exception(() => session.Get<IdentityUserRole<long>>(id));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Assert_IdentityUserToken_is_mapped_correctly()
    {
        // Arrange
        var session = _sessionFactory.OpenSession();
        using var transaction = session.BeginTransaction();
        var id = new IdentityUserToken<long> { UserId = 1, LoginProvider = "Foo", Name = "Bar" };

        // Act
        var exception = Record.Exception(() => session.Get<IdentityUserToken<long>>(id));

        // Assert
        Assert.Null(exception);
    }
}