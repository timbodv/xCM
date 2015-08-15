namespace xCM.ConfigMgrLib
{
    using System;
    using Microsoft.ConfigurationManagement.ManagementProvider;
    using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;

    class CMConnection
    {
        /// <summary>
        /// Get a connection to the Configuration Manager server
        /// </summary>
        internal static WqlConnectionManager Connect(string server)
        {
            try
            {
                SmsNamedValuesDictionary namedValues = new SmsNamedValuesDictionary();
                WqlConnectionManager connection = new WqlConnectionManager(namedValues);
                //connection.Connect(server, user, password);
                connection.Connect(server);
                return connection;
            }
            catch (SmsException ex)
            {
                throw new ApplicationException("Failed to connect", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new ApplicationException("Failed to authenticate", ex);
            }
        }
    }
}
