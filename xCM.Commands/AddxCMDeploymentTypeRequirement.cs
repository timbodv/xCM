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

        [Parameter(Mandatory = true)]
        public string AuthoringScopeId { get; set; }

        [Parameter(Mandatory = true)]
        public string LogicalName { get; set; }

        [Parameter(Mandatory = true)]
        public string SettingLogicalName { get; set; }

        [Parameter(Mandatory = true)]
        public string Value { get; set; }

        [Parameter(Mandatory = true)]
        public ConfigItemSettingTypeValues ConfigItemSettingType { get; set; }

        [Parameter(Mandatory = true)]
        [ValidateSetAttribute("Version")]
        public string ValueDataType { get; set; }

        [Parameter(Mandatory = true)]
        [ValidateSetAttribute("IsEquals", "LessEquals")]
        public string Expression { get; set; }

        [Parameter(Mandatory = true)]
        public string ComputerName { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                CMDeploymentType.AddRequirement(this.ApplicationName, this.AuthoringScopeId, this.LogicalName, this.SettingLogicalName, (int)this.ConfigItemSettingType, this.Value, this.ValueDataType, this.Expression, this.ComputerName);
                this.WriteVerbose("Updated application with requirement information");
            }
            catch (Exception ex)
            {
                this.WriteError(new ErrorRecord(ex, "", ErrorCategory.WriteError, this));
            }
        }
    }
}
