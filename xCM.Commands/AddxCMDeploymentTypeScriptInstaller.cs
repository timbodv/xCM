namespace xCM.Commands
{
    using System;
    using System.Management.Automation;
    using xCM.ConfigMgrLib;

    [Cmdlet(VerbsCommon.Add, "xCMDeploymentTypeScriptInstaller", HelpUri = "https://github.com/timbodv/xCM/blob/master/xCM.Commands/Help/AddxCMDeploymentTypeScriptInstaller.md")]
    public class AddxCMDeploymentTypeScriptInstaller : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string ApplicationName { get; set; }

        [Parameter(Mandatory = true)]
        public string DeploymentName { get; set; }

        [Parameter(Mandatory = true)]
        public string InstallCommand { get; set; }

        [Parameter(Mandatory = true)]
        public UserInteractionValues UserInteractMode { get; set; }

        [Parameter(Mandatory = true)]
        public ExecutionContextValues ExecutionContext { get; set; }

        [Parameter(Mandatory = true)]
        public PostExecutionBehaviourValues PostInstallBehaviour { get; set; }

        [Parameter]
        public string UninstallCommand { get; set; }

        [Parameter]
        [ValidateRange(15, 120)]
        public int MaximumExecutionTimeInMinutes { get; set; }

        [Parameter]
        [ValidateRange(15,120)]
        public int EstimatedExecutionTimeInMinutes { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "PsDetectionMethod")]
        public string PowershellDetectionScript { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "HklmDetectionMethod")]
        public string KeyPath { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "HklmDetectionMethod")]
        public SwitchParameter Is64Bit { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "HklmDetectionMethod")]
        public string ValueName { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "HklmDetectionMethod")]
        public string Value { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "HklmDetectionMethod")]
        [ValidateSetAttribute("Version")]
        public string ValueDataType { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "HklmDetectionMethod")]
        [ValidateSetAttribute("IsEquals", "LessEquals")]
        public string Expression { get; set; }

        [Parameter(Mandatory = true)]
        public string ComputerName { get; set; }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName == "PsDetectionMethod")
            {
                this.WriteVerbose("Creating deployment type using a PowerShell script as the detection method");
                try
                {
                    CMDeploymentType.AddScriptInstaller(this.ApplicationName, this.DeploymentName, this.PowershellDetectionScript, this.InstallCommand, this.UninstallCommand, (int)this.PostInstallBehaviour, (int)this.UserInteractMode, this.MaximumExecutionTimeInMinutes, this.EstimatedExecutionTimeInMinutes, (int)this.ExecutionContext, this.ComputerName);
                    this.WriteVerbose("Added deployment type to application");
                }
                catch (Exception ex)
                {
                    this.WriteError(new ErrorRecord(ex, "", ErrorCategory.WriteError, this));
                }
            }

            if (this.ParameterSetName == "HklmDetectionMethod")
            {
                this.WriteVerbose("Creating deployment type using a HKLM registry key as the detection method");
                try
                {
                    CMDeploymentType.AddScriptInstaller(this.ApplicationName, this.DeploymentName, this.KeyPath, this.Is64Bit.ToBool(), this.ValueName, this.Value, this.ValueDataType, this.Expression, this.InstallCommand, this.UninstallCommand, (int)this.PostInstallBehaviour, (int)this.UserInteractMode, this.MaximumExecutionTimeInMinutes, this.EstimatedExecutionTimeInMinutes, (int)this.ExecutionContext, this.ComputerName);
                    this.WriteVerbose("Added deployment type to application");
                }
                catch (Exception ex)
                {
                    this.WriteError(new ErrorRecord(ex, "", ErrorCategory.WriteError, this));
                }
            }
        }
    }
}
