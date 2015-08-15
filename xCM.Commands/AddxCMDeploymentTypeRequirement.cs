namespace xCM.Commands
{
    using System;
    using System.Management.Automation;
    using xCM.ConfigMgrLib;

    [Cmdlet(VerbsCommon.Add, "xCMDeploymentTypeRequirement", HelpUri = "https://github.com/timbodv/xCM/blob/master/xCM.Commands/Help/AddxCMDeploymentTypeRequirement.md")]
    public class AddxCMDeploymentTypeRequirement : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string ApplicationName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "RegistryRequirement")]
        public string AuthoringScopeId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "RegistryRequirement")]
        public string LogicalName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "RegistryRequirement")]
        public string SettingLogicalName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "RegistryRequirement")]
        public string Value { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "RegistryRequirement")]
        [ValidateSetAttribute("Version")]
        public string ValueDataType { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "RegistryRequirement")]
        [ValidateSetAttribute("IsEquals", "LessEquals")]
        public string Expression { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "OsRequirement")]
        public OperatingSystemValues OperatingSystem { get; set; }

        [Parameter(Mandatory = true)]
        public string ComputerName { get; set; }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName == "RegistryRequirement")
            {
                try
                {
                    CMDeploymentType.AddRequirement(this.ApplicationName, this.AuthoringScopeId, this.LogicalName, this.SettingLogicalName, this.Value, this.ValueDataType, this.Expression, this.ComputerName);
                    this.WriteVerbose("Updated application with registry requirement information");
                }
                catch (Exception ex)
                {
                    this.WriteError(new ErrorRecord(ex, "", ErrorCategory.WriteError, this));
                }
            }

            if (this.ParameterSetName == "OsRequirement")
            {
                try
                {
                    CMDeploymentType.AddRequirement(this.ApplicationName, this.OperatingSystem, this.ComputerName);
                    this.WriteVerbose("Updated application with OS requirement information");
                }
                catch (Exception ex)
                {
                    this.WriteError(new ErrorRecord(ex, "", ErrorCategory.WriteError, this));
                }
            }
        }
    }
}
