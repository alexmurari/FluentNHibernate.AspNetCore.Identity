# FluentNHibernate.AspNetCore.Identity

**Highly customizable ASP.NET Core Identity provider for NHibernate with FluentNHibernate mapping support.**

<a href="https://www.nuget.org/packages/Fluent.NHibernate.AspNetCore.Identity/absoluteLatest">
    <img alt="Nuget (with prereleases)" src="https://img.shields.io/nuget/vpre/Fluent.NHibernate.AspNetCore.Identity">
</a>

## 1. Getting started

### Install the package

> PM> Install-Package Fluent.NHibernate.AspNetCore.Identity

### Add the default entity mappings (only when not using custom entities)
```csharp
using FluentNHibernate.AspNetCore.Identity;

Fluently.Configure()
    .Database(persistenceConfigurer)
    .Mappings(t =>
    {
        // Only use this method when there are no custom identity entities.
        t.FluentMappings.AddIdentityMappings();
    }) 
    // ...
```

### Register the NHibernate stores

```csharp
using FluentNHibernate.AspNetCore.Identity;

services.AddScoped(t => SessionFactory.OpenSession()) // ISession required for resolving store services.

services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>() // Optional
    .AddNHibernateStores(t => t.SetSessionAutoFlush(false));
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
        // Add your custom mappings.
        t.FluentMappings.AddFromAssembly(/* Your custom mappings assembly */))
        // Manually add the default mappings for non-customized entities
        .Add<IdentityRoleClaimMap>()
        .Add<IdentityUserLoginMap>()
        // ...
    }) 
```

**NOTE:** **Do not** use the ```FluentMappings.AddIdentityMappings()``` method when there are custom identity entities defined.
Mappings for default entities must be added **manually** since calling ```AddIdentityMappings()``` would add unnecessary mappings for base types
of custom identity entities  that derive from those types.

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
    .AddNHibernateStores(t => t.SetSessionAutoFlush(true));
```

**NOTE:** Identity entities **omitted** during service registration (```IdentityUserClaim<Tkey>```, ```IdentityUserToken<TKey>```, etc.) 
are automatically registered with the default type and the ```TKey``` generic argument representing the entity's primary key is inferred from the registered 
```IdentityUser<TKey>``` type.
E.g. registered ```IdentityUser<long>``` will automatically cause a default ```IdentityUserToken<long>``` to be registered, when this
specific entity registration is omitted.

## 3. Extra options

### i. Overriding schema/table names

#### a. Override the default schema or table names by constructor parameter.

```csharp
using FluentNHibernate.AspNetCore.Identity;

public class ApplicationUserMap : IdentityUserMapBase<ApplicationUser, string>
{
    public ApplicationUserMap() : base("MyTableName", t => t.GeneratedBy.TriggerIdentity().Length(32))
    {
        // Custom property maps...
    }
}

// OR

public class ApplicationUserMap : IdentityUserMapBase<ApplicationUser, string>
{
    public ApplicationUserMap() : base("MySchemaName", "MyTableName", t => t.GeneratedBy.TriggerIdentity().Length(32))
    {
        // Custom property maps...
    }
}
```

#### b. Override the default schema or table names by calling configuration methods from the base map class.

```csharp
using FluentNHibernate.AspNetCore.Identity;

public class ApplicationUserMap : IdentityUserMapBase<ApplicationUser, string>
{
    public ApplicationUserMap() : base(t => t.GeneratedBy.TriggerIdentity().Length(32))
    {
        Schema("MySchemaName");
        Table("MyTableName");
    }
}

```

**Default schema name: ```"dbo"``` (SQL Server default)**

### ii. Controlling session auto flush in store services

By default, after calling create, update or delete methods in the identity stores, the NHibernate session is automatically flushed.

The auto flush behavior can be controlled through the ```SetSessionAutoFlush(bool)``` method.

```csharp
services.AddIdentityCore<ApplicationUser>()
    // ...
    .AddNHibernateStores(t => t.SetSessionAutoFlush(false));
```

**Default: ```true```**

### iii. Controlling store GUID format

The identity store automatically generate GUIDs for determined string properties (e.g. ```ConcurrencyStamp```).
The format of the GUID's string representation can be defined through the ```SetGuidFormat(GuidFormat)``` method.

```csharp
services.AddIdentityCore<ApplicationUser>()
    // ...
    .AddNHibernateStores(t => t.SetGuidFormat(GuidFormat.Digits));
```

**Default: ```GuidFormat.Hyphens``` (```"D"```)**

### iv. Adding custom identity entities through extended configuration

Custom identity entities (apart from ``` IdentityUser<TKey> ``` and ``` IdentityRole<TKey> ```) can be defined during store service registration
through the ```ExtendConfiguration``` method.

```csharp
services.AddIdentityCore<ApplicationUser>()
    .AddRoles<ApplicationRoles>()
    // -----------------------------------
    .ExtendConfiguration()
    .AddUserClaim<ApplicationUserClaim>()
    .AddUserToken<ApplicationUserToken>()
    // Other custom entities...
    // -----------------------------------
    .AddNHibernateStores(t => t.SetSessionAutoFlush(true));
```

## 4. Usage  example

```csharp
using FluentNHibernate.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;

public class RegistrationController
{
    private readonly IUserStore<IdentityUser> _userStore; // Or your custom IdentityUser type that derives from IdentityUser<TKey>

    public RegistrationController(IUserStore<IdentityUser> userStore)
    {
        _userStore = userStore;
    }

    // ...
}
```

## 5. License

[MIT License (MIT)](./license.txt)
