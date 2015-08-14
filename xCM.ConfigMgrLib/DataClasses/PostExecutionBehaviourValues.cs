namespace xCM.ConfigMgrLib
{
    public enum PostExecutionBehaviourValues
    {
        BasedOnExitCode = 0,
        NoAction = 1,
        ProgramReboot = 2,
        ForceReboot = 3,
        ForceLogoff = 4
    }
}
