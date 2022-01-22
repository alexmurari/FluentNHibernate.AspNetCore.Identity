# FluentNHibernate.AspNetCore.Identity

**Highly customizable ASP.NET Core Identity provider for NHibernate with FluentNHibernate mapping support.**

## 1. Getting started

### Install the package

> PM> Install-Package FluentNHibernate.AspNetCore.Identity

### Add the default entity mapping (**only when not using custom entities**)
```csharp
using FluentNHibernate.AspNetCore.Identity;

Fluently.Configure()
    .Database(persistenceConfigurer)
    .Mappings(t =>
    {
        t.FluentMappings.AddIdentityMappings();
    }) 
    // ...
```

### Register the NHibernate stores

```csharp
using FluentNHibernate.AspNetCore.Identity;

services.AddScoped(t => SessionFactory.OpenSession()) // ISession required for resolving store services.

services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddNHibernateStores(t => t.SetAutoFlushSession(false));
```

## 2. Custom entities/mappings

### Define custom entity

```csharp
using FluentNHibernate.AspNetCore.Identity;

public class ApplicationUser : IdentityUser<string> // Configurable PK type
{
    public virtual DateTime BirthDate { get; set; } // Custom properties
}
```

### Define custom mapping

```csharp
using FluentNHibernate.AspNetCore.Identity;

public class ApplicationUserMap : IdentityUserMapBase<ApplicationUser, string>
{
    public ApplicationUserMap() : base(t => t.GeneratedBy.TriggerIdentity().Length(32)) // Primary key config
    {
        Map(t => t.BirthDate).Not.Nullable(); // Custom property map
    }
}
```

### Add custom entity mappings
```csharp
using FluentNHibernate.AspNetCore.Identity;

Fluently.Configure()
    .Database(persistenceConfigurer)
    .Mappings(t =>
    {
        // When all identity entities are customized
        t.FluentMappings.AddFromAssembly(/* Your custom mappings assembly */));

        // OR

        // When only some identy entities are customized
        t.FluentMappings.AddFromAssembly(/* Your custom mappings assembly */))
        // Manually add the default mappings for non-customized entities
        .Add<IdentityRoleClaimMap>()
        .Add<IdentityUserLoginMap>()
        .Add</* other default entities... */>()
    }) 
```

### Register the NHibernate stores for custom entities

```csharp
using FluentNHibernate.AspNetCore.Identity;

services.AddScoped(t => SessionFactory.OpenSession()) // ISession required for resolving store services.

services.AddIdentityCore<ApplicationUser>()
    .AddRoles<ApplicationRoles>()
    .ExtendConfiguration()
    .AddUserClaim<ApplicationUserClaim>()
    .AddUserToken<ApplicationUserToken>()
    // Other custom entities...
    .AddNHibernateStores(t => t.SetAutoFlushSession(true));
```

## 3. Usage

```csharp
using FluentNHibernate.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;

public class RegistrationController
{
    private readonly IUserStore<IdentityUser> _userStore; // Or your custom IdentityUser type that derives from IdentityUser<TKey>

    public RegistrationController(IUserStore<ApplicationUser> userStore)
    {
        _userStore = userStore;
    }

    // ...
}
```