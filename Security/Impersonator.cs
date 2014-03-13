using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Civic.Core.Framework.Security
{
    public class Impersonator : IDisposable
    {
        private readonly WindowsImpersonationContext _impersonatedUser = null;
        private readonly IntPtr _userHandle;

        public Impersonator(string domain, string username, string password) : this(domain, username, password, "interactive")
        {
        }

        public Impersonator(string domain, string username, string password, string type)
        {
            _userHandle = new IntPtr(0);

            int nType;
            switch (type.ToLower())
            {
                case "service":
                    nType = LOGON32_LOGON_SERVICE;
                    break;
                case "default":
                    nType = LOGON32_PROVIDER_DEFAULT;
                    break;
                default:
                    nType = LOGON32_PROVIDER_DEFAULT;
                    break;
            }

            bool returnValue = LogonUser(username, domain, password, nType, LOGON32_PROVIDER_DEFAULT, ref _userHandle);
            if (!returnValue)
                throw new ApplicationException("Could not impersonate user");
            var newId = new WindowsIdentity(_userHandle);
            _impersonatedUser = newId.Impersonate();
        }

        #region IDisposable Members
        public void Dispose()
        {
            if (_impersonatedUser != null)
            {
                _impersonatedUser.Undo();
                CloseHandle(_userHandle);
            }
        }
        #endregion

        #region Interop imports/constants
        
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_LOGON_SERVICE = 3;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        public static extern bool LogonUser(String lpszUserName, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);
        
        #endregion
    }
}
