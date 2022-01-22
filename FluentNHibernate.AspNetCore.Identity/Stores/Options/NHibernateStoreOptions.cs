namespace FluentNHibernate.AspNetCore.Identity
{
    public class NHibernateStoreOptions
    {
        internal NHibernateStoreOptions()
        {
        }

        /// <summary>
        ///     Gets a flag indicating if store changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
        /// </summary>
        /// <returns>
        ///     True if changes should be automatically persisted, otherwise false.
        /// </returns>
        public bool AutoFlushSession { get; private set; }

        /// <summary>
        ///     Sets a flag indicating if store changes should be persisted after CreateAsync, UpdateAsync and DeleteAsync are called.
        /// </summary>
        public NHibernateStoreOptions SetAutoFlushSession(bool value = true)
        {
            AutoFlushSession = value;
            return this;
        }
    }
}