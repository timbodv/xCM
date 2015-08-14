namespace xCM.Commands
{
    using System;
    using System.Management.Automation;
    using xCM.ConfigMgrLib;

    [Cmdlet(VerbsCommon.Add, "xCMApplicationSupersedence", HelpUri = "https://github.com/timbodv/xCM/blob/master/xCM.Commands/Help/AddxCMApplicationSupersedence.md")]
    public class AddxCMApplicationSupersedence : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string ApplicationName { get; set; }

        [Parameter(Mandatory = true)]
        public string SupersededApplicationName { get; set; }

        [Parameter(Mandatory = true)]
        public string ComputerName { get; set; }

        public SwitchParameter Uninstall { get; set; }

        protected override void ProcessRecord()
        {
            try
            {
                CMDeploymentType.AddSupersedence(this.ApplicationName, this.SupersededApplicationName, this.Uninstall.ToBool(), this.ComputerName);
                this.WriteVerbose("Updated application with superseded application information");
            }
            catch (Exception ex)
            {
                this.WriteError(new ErrorRecord(ex, "", ErrorCategory.WriteError, this));
            }
        }
    }
}
