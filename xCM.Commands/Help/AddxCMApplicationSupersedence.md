
# Add-xCMApplicationSupersedence
Create a supersedence relationship between two applications

## Aliases
None

## Syntax

`Add-xCMApplicationSupersedence -ApplicationName <string> -SupersededApplicationName <string> -ComputerName <string> [<CommonParameters>]`

## Examples
### Example 1
Supersede an Office 365 Pro Plus package:

~~~~
Add-xCMApplicationSupersedence -ApplicationName "Microsoft Office 365 Pro Plus 15.0.4745.1001" -SupersededApplicationName "Microsoft Office 365 Pro Plus 15.0.4737.1003" -ComputerName server
~~~~
