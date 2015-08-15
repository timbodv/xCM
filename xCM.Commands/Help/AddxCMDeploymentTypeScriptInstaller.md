
# Add-xCMDeploymentTypeScriptInstaller
Add a script installer deployment type to an application

## Aliases
None

## Syntax

### PowerShell Script Detection Logic
`Add-xCMDeploymentTypeScriptInstaller -ApplicationName <string> -DeploymentName <string> -InstallCommand <string> -UserInteractMode <UserInteractionValues> {Normal | Minimised | Maximised | Hidden}-ExecutionContext <ExecutionContextValues> {Any | User | System} -PostInstallBehaviour <PostExecutionBehaviourValues> {BasedOnExitCode | NoAction | ProgramReboot | ForceReboot | ForceLogoff) -PowershellDetectionScript <string> -ComputerName <string> [-UninstallCommand <string>] [-MaximumExecutionTimeInMinutes <int>] [-EstimatedExecutionTimeInMinutes <int>]  [<CommonParameters>]`

### Registry Detection Logic
`Add-xCMDeploymentTypeScriptInstaller -ApplicationName <string> -DeploymentName <string> -InstallCommand <string> -UserInteractMode <UserInteractionValues> {Normal | Minimised | Maximised | Hidden} -ExecutionContext <ExecutionContextValues> {Any | User | System} -PostInstallBehaviour <PostExecutionBehaviourValues> {BasedOnExitCode | NoAction | ProgramReboot | ForceReboot | ForceLogoff} -KeyPath <string> -Is64Bit -ValueName <string> -Value <string> -ValueDataType <string> {Version} -Expression <string> {IsEquals | LessEquals} -ComputerName <string> [-UninstallCommand <string>] [-MaximumExecutionTimeInMinutes <int>] [-EstimatedExecutionTimeInMinutes <int>]  [<CommonParameters>]`

## Examples
### Example 1
