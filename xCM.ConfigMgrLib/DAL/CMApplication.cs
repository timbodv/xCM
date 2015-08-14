namespace xCM.ConfigMgrLib
{
    using System;
    using Microsoft.ConfigurationManagement.ApplicationManagement;
    using Microsoft.ConfigurationManagement.ManagementProvider;
    using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
    using Microsoft.ConfigurationManagement.AdminConsole.AppManFoundation;

    class CMApplication
    {
        /// <summary>
        /// Return a single Application object
        /// </summary>
        internal static Application GetApplicationByName(string name, string server)
        {
            NamedObject.DefaultScope = "xCM";

            WqlConnectionManager connectionManager = CMConnection.Connect(server);

            IResultObject applications = connectionManager.QueryProcessor.ExecuteQuery("SELECT * FROM SMS_Application WHERE LocalizedDisplayName='" + name + "' AND IsLatest=1");

            // this should only return one result, but we need to perform a Get() to retrieve the lazy properties
            IResultObject application = null;
            foreach (IResultObject result in applications)
            {
                result.Get();
                // tip : make sure the CI_ID is not in single quotes, for example, SMS_Application.CI_ID='123456'
                application = connectionManager.GetInstance(@"SMS_Application.CI_ID=" + result["CI_ID"].IntegerValue);
            }

            ApplicationFactory applicationFactory = new ApplicationFactory();
            AppManWrapper applicationWrapper = AppManWrapper.WrapExisting(application, applicationFactory) as AppManWrapper;
            return applicationWrapper.InnerAppManObject as Application;
        }

        /// <summary>
        /// Update the Application object
        /// </summary>
        internal static void Save(Application app, string server)
        {
            WqlConnectionManager connection = CMConnection.Connect(server);
            ApplicationFactory applicationFactory = new ApplicationFactory();
            AppManWrapper applicationWrapper = AppManWrapper.Create(connection, applicationFactory) as AppManWrapper;
            applicationWrapper.InnerAppManObject = app;
            applicationFactory.PrepareResultObject(applicationWrapper);
            applicationWrapper.InnerResultObject.Put();
        }
    }
}
