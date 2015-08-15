namespace xCM.ConfigMgrLib
{
    using System;
    using Microsoft.ConfigurationManagement.ApplicationManagement;
    using Microsoft.ConfigurationManagement.DesiredConfigurationManagement;
    using Microsoft.ConfigurationManagement.DesiredConfigurationManagement.ExpressionOperators;
    using Microsoft.SystemsManagementServer.DesiredConfigurationManagement.Expressions;
    using Microsoft.SystemsManagementServer.DesiredConfigurationManagement.Rules;

    public class CMDeploymentType
    {
        /// <summary>
        /// Return a standard script installer object using common properties
        /// </summary>
        static ScriptInstaller ReturnStandardScriptInstaller(string installCommand, string uninstallCommand, int postInstallBehaviour, int userInteractMode, int maxExecTime, int estimateExecTime, int execContext)
        {
            ScriptInstaller installer = new ScriptInstaller();

            installer.InstallCommandLine = installCommand;
            if (!string.IsNullOrEmpty(uninstallCommand))
            {
                installer.UninstallCommandLine = uninstallCommand;
            }

            installer.PostInstallBehavior = (PostExecutionBehavior)postInstallBehaviour;
            installer.UserInteractionMode = (UserInteractionMode)userInteractMode;
            installer.MaxExecuteTime = maxExecTime;
            installer.ExecuteTime = estimateExecTime;
            installer.ExecutionContext = (ExecutionContext)execContext;

            return installer;
        }

        /// <summary>
        /// Add a script installer deployment type to an application, using a PowerShell script for the detection logic
        /// </summary>
        public static void AddScriptInstaller(string applicationName, string installName, string powershellDetectionScript, string installCommand, string uninstallCommand, int postInstallBehaviour, int userInteractMode, int maxExecTime, int estimateExecTime, int execContext, string server)
        {
            Application app = CMApplication.GetApplicationByName(applicationName, server);
            ScriptInstaller installer = ReturnStandardScriptInstaller(installCommand, uninstallCommand, postInstallBehaviour, userInteractMode, maxExecTime, estimateExecTime, execContext);

            installer.DetectionMethod = DetectionMethod.Script;
            installer.DetectionScript = new Script { Text = powershellDetectionScript, Language = "PowerShell" };

            DeploymentType deploymentType = new DeploymentType(installer, ScriptInstaller.TechnologyId, NativeHostingTechnology.TechnologyId);
            deploymentType.Title = installName;
            app.DeploymentTypes.Add(deploymentType);

            CMApplication.Save(app, server);
        }

        /// <summary>
        /// Add a script installer deployment type to an application, using registry key in HKLM
        /// </summary>
        public static void AddScriptInstaller(string applicationName, string installName, string hklmKey, bool is64bit, string valueName, string valueNameValue, string dataType, string expressionOperator, string installCommand, string uninstallCommand, int postInstallBehaviour, int userInteractMode, int maxExecTime, int estimateExecTime, int execContext, string server)
        {
            Application app = CMApplication.GetApplicationByName(applicationName, server);
            ScriptInstaller installer = ReturnStandardScriptInstaller(installCommand, uninstallCommand, postInstallBehaviour, userInteractMode, maxExecTime, estimateExecTime, execContext);

            installer.DetectionMethod = DetectionMethod.Enhanced;
            EnhancedDetectionMethod enhancedDetectionMethod = new EnhancedDetectionMethod();

            ConstantValue expectedValue = null;
            RegistrySetting registrySetting = new RegistrySetting(null);
            registrySetting.RootKey = RegistryRootKey.LocalMachine;
            registrySetting.Key = hklmKey;
            registrySetting.Is64Bit = is64bit;
            registrySetting.ValueName = valueName;
            registrySetting.CreateMissingPath = false;

            switch (dataType)
            {
                case "Version":
                    registrySetting.SettingDataType = DataType.Version;
                    expectedValue = new ConstantValue(valueNameValue, DataType.Version);
                    break;
                default:
                    break;
            }

            enhancedDetectionMethod.Settings.Add(registrySetting);

            SettingReference settingReference = new SettingReference(app.Scope, app.Name, app.Version.GetValueOrDefault(), registrySetting.LogicalName, registrySetting.SettingDataType, registrySetting.SourceType, false);
            settingReference.MethodType = ConfigurationItemSettingMethodType.Value;

            CustomCollection<ExpressionBase> operands = new CustomCollection<ExpressionBase>();
            operands.Add(settingReference);
            operands.Add(expectedValue);

            Expression expression = null;

            switch (expressionOperator)
            {
                case "IsEquals":
                    expression = new Expression(ExpressionOperator.IsEquals, operands);
                    break;
                case "LessEquals":
                    expression = new Expression(ExpressionOperator.LessEquals, operands);
                    break;
                default:
                    break;
            }

            Rule rule = new Rule(Guid.NewGuid().ToString("N"), NoncomplianceSeverity.None, null, expression);
            enhancedDetectionMethod.Rule = rule;

            installer.EnhancedDetectionMethod = enhancedDetectionMethod;

            DeploymentType deploymentType = new DeploymentType(installer, ScriptInstaller.TechnologyId, NativeHostingTechnology.TechnologyId);
            deploymentType.Title = installName;
            app.DeploymentTypes.Add(deploymentType);

            CMApplication.Save(app, server);
        }

        /// <summary>
        /// Using the priority 1 deployment types from two applications, create a supersedence relationship
        /// </summary>
        public static void AddSupersedence(string newApplicationName, string supersededApplicationName, bool uninstall, string server)
        {
            Application newApp = CMApplication.GetApplicationByName(newApplicationName, server);
            Application supersededApp = CMApplication.GetApplicationByName(supersededApplicationName, server);

            DeploymentType newAppDeploymentType = newApp.DeploymentTypes[0];
            DeploymentType supersededAppDeploymentType = supersededApp.DeploymentTypes[0];

            DeploymentTypeIntentExpression intentExpression = new DeploymentTypeIntentExpression(supersededApp.Scope, supersededApp.Name, (int)supersededApp.Version, supersededAppDeploymentType.Scope, supersededAppDeploymentType.Name, (int)supersededAppDeploymentType.Version, DeploymentTypeDesiredState.Prohibited, uninstall);
            DeploymentTypeRule deploymentRule = new DeploymentTypeRule(NoncomplianceSeverity.None, null, intentExpression);
            newAppDeploymentType.Supersedes.Add(deploymentRule);
            CMApplication.Save(newApp, server);

        }

        /// <summary>
        /// Add a registry requirement to an existing deployment type
        /// </summary>
        public static void AddRequirement(string applicationName, string authoringScopeId, string logicalName, string settingLogicalName, string value, string dataType, string expressionOperator, string server)
        {
            Application app = CMApplication.GetApplicationByName(applicationName, server);
            DeploymentType deploymentType = app.DeploymentTypes[0];

            CustomCollection<ExpressionBase> settingsOperands = new CustomCollection<ExpressionBase>();
            GlobalSettingReference settingReferences = null;
            ConstantValue constant = null;

            switch (dataType)
            {
                case "Version":
                    settingReferences = new GlobalSettingReference(authoringScopeId, logicalName, DataType.Version, settingLogicalName, ConfigurationItemSettingSourceType.Registry);
                    constant = new ConstantValue(value, DataType.Version);
                    break;
                default:
                    break;
            }

            settingsOperands.Add(settingReferences);
            settingsOperands.Add(constant);

            Expression expression = null;

            switch (expressionOperator)
            {
                case "IsEquals":
                    expression = new Expression(ExpressionOperator.IsEquals, settingsOperands);
                    break;
                case "LessEquals":
                    expression = new Expression(ExpressionOperator.LessEquals, settingsOperands);
                    break;
                default:
                    break;
            }

            Rule rule = new Rule(Guid.NewGuid().ToString("N"), NoncomplianceSeverity.Critical, null, expression);

            deploymentType.Requirements.Add(rule);
            CMApplication.Save(app, server);
        }

        /// <summary>
        /// Add an operating system requirement to an existing deployment type
        /// </summary>
        public static void AddRequirement(string applicationName, OperatingSystemValues os, string server)
        {
            Application app = CMApplication.GetApplicationByName(applicationName, server);
            DeploymentType deploymentType = app.DeploymentTypes[0];

            string ruleExpressionText = string.Empty;
            string ruleAnnotationText = string.Empty;

            switch (os)
            {
                case OperatingSystemValues.Windows81x86:
                    ruleExpressionText = "Windows/All_x86_Windows_8.1_Client";
                    ruleAnnotationText = "Operating system One of {All Windows 8.1(32 - bit)}";
                    break;
                case OperatingSystemValues.Windows81x64:
                    ruleExpressionText = "Windows/All_x64_Windows_8.1_Client";
                    ruleAnnotationText = "Operating system One of {All Windows 8.1(64 - bit)}";
                    break;
                case OperatingSystemValues.Windows10x86:
                    ruleExpressionText = "Windows/All_x86_Windows_10_and_higher_Clients";
                    ruleAnnotationText = "Operating system One of {All Windows 10 Professional/Enterprise and higher (32 - bit)}";
                    break;
                case OperatingSystemValues.Windows10x64:
                    ruleExpressionText = "Windows/All_x64_Windows_10_and_higher_Clients";
                    ruleAnnotationText = "Operating system One of {All Windows 10 Professional/Enterprise and higher (64 - bit)}";
                    break;
            }

            CustomCollection<RuleExpression> ruleCollection = new CustomCollection<RuleExpression>();
            RuleExpression ruleExpression = new RuleExpression(ruleExpressionText);
            ruleCollection.Add(ruleExpression);

            OperatingSystemExpression osExpression = new OperatingSystemExpression(ExpressionOperator.OneOf, ruleCollection);

            Rule rule = new Rule(Guid.NewGuid().ToString("N"), NoncomplianceSeverity.None, new Annotation(ruleAnnotationText, null, null, null), osExpression);

            deploymentType.Requirements.Add(rule);
            CMApplication.Save(app, server);
        }
    }
}
