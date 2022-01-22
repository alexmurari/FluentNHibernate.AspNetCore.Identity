namespace FluentNHibernate.AspNetCore.Identity;

using System.Reflection;
using FluentNHibernate.Cfg;

/// <summary>
///     Provides extension methods to the <see cref="FluentMappingsContainer"/> class.
/// </summary>
public static class FluentMappingsContainerExtensions
{
    /// <summary>
    ///     Adds the default identity mappings defined in the <c>FluentNHibernate.AspNetCore.Identity</c> assembly.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Only call this method when using the default identity entities (e.g. <see cref="IdentityUser"/>) provided by <c>FluentNHibernate.AspNetCore.Identity</c>.
    ///     </para>
    ///     <para>
    ///         If custom identity entities are being used (e.g. deriving from <see cref="IdentityUser{TKey}"/>), do not call this method.
    ///         Instead, define and register the custom fluent mappings for the entities manually.
    ///     </para>
    /// </remarks>
    /// <param name="container">The fluent mapping container.</param>
    /// <returns>The fluent mapping container.</returns>
    public static FluentMappingsContainer AddIdentityMappings(this FluentMappingsContainer container) 
        => container.AddFromAssembly(Assembly.GetExecutingAssembly());
}