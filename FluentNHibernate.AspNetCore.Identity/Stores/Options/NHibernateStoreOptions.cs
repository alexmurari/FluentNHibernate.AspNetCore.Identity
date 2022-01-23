namespace FluentNHibernate.AspNetCore.Identity
{
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
        ///     Gets a flag indicating if store changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
        /// </summary>
        public bool? AutoFlushSession { get; private set; }

        /// <summary>
        ///     Gets the GUID format specifier that the identity store uses when converting <see cref="Guid"/> instances to it's string representation.
        /// </summary>
        public GuidFormat? GuidFormat { get; private set; }

        /// <summary>
        ///     Sets a flag indicating if store changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
        /// </summary>
        public NHibernateStoreOptions SetAutoFlushSession(bool value = true)
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
}