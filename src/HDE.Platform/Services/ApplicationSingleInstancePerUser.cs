using System;
using System.Threading;

namespace HDE.Platform.Services
{
    public class ApplicationSingleInstancePerUser: IDisposable
    {
        #region Internal Fields

        private readonly bool _firstInstance;
        private readonly Mutex _mutex;

        #endregion

        #region Public Properties

        /// <summary>
        /// Shows if the current instance of ghost is the first
        /// </summary>
        public bool FirstInstance
        {
            get { return _firstInstance; }
        }

        #endregion

        public ApplicationSingleInstancePerUser(string applicationName)
        {
            var name = applicationName + Environment.UserDomainName;
            try
            {
                //Grab mutex if it exists
                _mutex = Mutex.OpenExisting(name);
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                _firstInstance = true;
            }

            if (_mutex == null)
            {
                _mutex = new Mutex(false, name);
            }
        }

        public void Dispose()
        {
            _mutex.Dispose();
        }
    }
}
