# xCM
Additional cmdlets for Configuration Manager 2012 to be used in conjuction with the existing ones, just to fill in some gaps with what I've been working on around OSD and application automation.

# Requirements
* Configuration Manager 2012 (tested with 2012 R2 SP1) Administration Console
* PowerShell (tested with 4.0)

## TODO
* update AddRequirements to support GLOBAL requirements (focus on OS requirements)
* update help files
* exception handling could use some work
* refactor the expression logic
* use reflection to add ConfigMgr DLL's from their installed path ($env:SMS_ADMIN_UI_PATH)
* (low) is it possible to get server name from the current drive
* (low) can we look for applications by partial names? what do we do if there is more than one - need to handle this
