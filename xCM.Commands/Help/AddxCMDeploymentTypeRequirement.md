
# Add-xCMDeploymentTypeRequirement
Add a requirement to a deployment type

## Aliases
None

## Syntax
### Registry
`Add-xCMDeploymentTypeRequirement -ApplicationName <string> -AuthoringScopeId <string> -LogicalName <string> -SettingLogicalName <string> -Value <string> -ValueDataType <string> {Version} -Expression <string> {IsEquals | LessEquals} -ComputerName <string> [<CommonParameters>]`

### Operating System
`Add-xCMDeploymentTypeRequirement -ApplicationName <string> -OperatingSystem <OperatingSystemValues> {Windows81x86 | Windows81x64 | Windows10x86 | Windows10x64} -ComputerName <string>  [<CommonParameters>]`

## Examples
### Example 1
Find a custom global condition:

~~~~
Get-CMGlobalCondition | ? { $_.DataType -eq 'Version' } | select LocalizedDisplayName, ModelName, SDMPackageXML
~~~~

### Example 2
Add a custom global condition registry requirement. 

~~~~
Add-xCMDeploymentTypeRequirement -ApplicationName "Microsoft Office 365 Pro Plus 15.0.4745.1001" -AuthoringScopeId "ScopeId_480B40CE-C6FF-4CEA-A02D-13037CE8AEE2" -LogicalName "GlobalSettings_f7ee8bcc-a199-465e-9c55-5cab30ccde00" -SettingLogicalName "RegSetting_599724bb-8714-49e8-9411-0d376a741c14" -ValueDataType Version -Expression LessEquals -Value '15.0.4745.1001'  -ComputerName server
~~~~

### Example 3
Add a Windows 8.1 x64 operating system requirement:

~~~~
Add-xCMDeploymentTypeRequirement -ApplicationName "Microsoft Office 365 Pro Plus 15.0.4745.1001" -OperatingSystem Windows81x64 -ComputerName server
~~~~
