namespace FluentNHibernate.AspNetCore.Identity;

using System;

public enum GuidFormat
{
    Digits = 'N',
    Hyphens = 'D',
    Braces = 'B',
    Parentheses = 'P'
}

public class NHibernateStoreOptions
{
    internal NHibernateStoreOptions()
    {
    }

    /// <summary>
    ///     Gets a flag indicating if store changes should be persisted after <c>CreateAsync</c>, <c>UpdateAsync</c> and <c>DeleteAsync</c> are called.
    /// </summary>
    public bool? AutoFlushSession { get; private set; }

    /// <summary>
    ///     Gets the GUID format specifier used by the identity store when converting <see cref="Guid"/> instances to it's string representation.
    /// </summary>
    public GuidFormat? GuidFormat { get; private set; }

    /// <summary>
    ///     Sets a flag indicating if store changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
    /// </summary>
    public NHibernateStoreOptions SetSessionAutoFlush(bool value = true)
    {
        AutoFlushSession = value;
        return this;
    }

    /// <summary>
    ///     Sets the value of the GUID format specifier.
    /// </summary>
    public NHibernateStoreOptions SetGuidFormat(GuidFormat guidFormat)
    {
        GuidFormat = guidFormat;
        return this;
    }
}